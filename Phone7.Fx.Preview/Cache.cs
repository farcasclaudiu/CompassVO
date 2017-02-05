using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;

namespace Phone7.Fx.Preview
{
    /// <summary>
    /// 
    /// </summary>
    public class Cache
    {
        public static readonly DateTime NoAbsoluteExpiration = DateTime.MaxValue;
        public static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;

        readonly IsolatedStorageFile _myStore = IsolatedStorageFile.GetUserStoreForApplication();

        private object _sync = new object();


        private static Cache _current;
        /// <summary>
        /// Gets the current instance of the cache
        /// </summary>
        /// <value>The current.</value>
        public static Cache Current
        {
            get { return _current ?? (_current = new Cache()); }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        public void Add(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            lock (_sync)
            {
                if (Contains(key))
                    Remove(key);

                if (absoluteExpiration == NoAbsoluteExpiration)
                    Add(key, DateTime.UtcNow + slidingExpiration, value);
                if (slidingExpiration == NoSlidingExpiration)
                    Add(key, absoluteExpiration, value);
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="expirationDate">The expiration date.</param>
        /// <param name="value">The value.</param>
        private void Add(string key, DateTime expirationDate, object value)
        {
            lock (_sync)
            {
                if (!_myStore.DirectoryExists(key))
                    _myStore.CreateDirectory(key);
                else
                {
                    string currentFile = GetFileNames(key).FirstOrDefault();
                    if (currentFile != null)
                        _myStore.DeleteFile(string.Format("{0}\\{1}", key, currentFile));
                    _myStore.DeleteDirectory(key);
                    _myStore.CreateDirectory(key);
                }

                string fileName = string.Format("{0}\\{1}.cache", key, expirationDate.ToFileTimeUtc());

                if (_myStore.FileExists(fileName))
                    _myStore.DeleteFile(fileName);

                NormalWrite(fileName, value);
            }

        }

        /// <summary>
        /// Determines whether the cache contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string key)
        {
            lock (_sync)
            {
                if (_myStore.DirectoryExists(key) && GetFileNames(key).Any())
                {
                    string currentFile = GetFileNames(key).FirstOrDefault();
                    if (currentFile != null)
                    {
                        var expirationDate =
                            DateTime.FromFileTimeUtc(long.Parse(Path.GetFileNameWithoutExtension(currentFile)));
                        if (expirationDate >= DateTime.UtcNow)
                            return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            lock (_sync)
            {
                if (!Contains(key))
                    throw new AccessViolationException("The key does not exist in the cache");
                string currentFile = GetFileNames(key).FirstOrDefault();
                if (currentFile != null)
                    _myStore.DeleteFile(string.Format("{0}\\{1}", key, currentFile));
                _myStore.DeleteDirectory(key);
            }
        }

        /// <summary>
        /// Gets the file names.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private IEnumerable<string> GetFileNames(string key)
        {
            return _myStore.GetFileNames(string.Format("{0}\\*.cache", key));
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            lock (_sync)
            {
                string currentFile = GetFileNames(key).FirstOrDefault();
                if (currentFile != null)
                {
                    var expirationDate =
                        DateTime.FromFileTimeUtc(long.Parse(Path.GetFileNameWithoutExtension(currentFile)));
                    if (expirationDate >= DateTime.UtcNow)
                    {
                        return NormalRead<T>(string.Format(@"{0}\{1}", key, currentFile));
                    }
                    Remove(key);
                }
                return default(T);
            }

        }

        #region Serialization

        private T NormalRead<T>(string fileName)
        {
            using (var isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, FileMode.Open, _myStore))
            {
                DataContractSerializer s = new DataContractSerializer(typeof(T));

                var value = s.ReadObject(isolatedStorageFileStream);
                isolatedStorageFileStream.Close();
                return (T)value;
            }
        }

        private void NormalWrite(string fileName, object value)
        {
            using (var isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, _myStore))
            {
                DataContractSerializer s = new DataContractSerializer(value.GetType());

                s.WriteObject(isolatedStorageFileStream, value);
            }
        }

        #endregion
    }
}