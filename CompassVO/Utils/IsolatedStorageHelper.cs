using Microsoft.Phone;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

namespace CompassVO.Utils
{
  public class IsolatedStorageHelper
  {
    public static void SaveToLocalStorage(string imageFileName, string imageFolder, byte[] content)
    {
      if (content == null)
      {
        return;
      }

      var isoFile = IsolatedStorageFile.GetUserStoreForApplication();
      if (!isoFile.DirectoryExists(imageFolder))
      {
        isoFile.CreateDirectory(imageFolder);
      }

      string filePath = Path.Combine(imageFolder, imageFileName);
      using (var stream = isoFile.CreateFile(filePath))
      {
        stream.Write(content, 0, content.Length);
      }
    }

    public static WriteableBitmap LoadFromLocalStorage(string imageFileName, string imageFolder)
    {
      var isoFile = IsolatedStorageFile.GetUserStoreForApplication();
      if (!isoFile.DirectoryExists(imageFolder))
      {
        isoFile.CreateDirectory(imageFolder);
      }
      string filePath = Path.Combine(imageFolder, imageFileName);
      if (!isoFile.FileExists(filePath))
      {
        return null;
      }
      using (var imageStream = isoFile.OpenFile(filePath, FileMode.Open, FileAccess.Read))
      {
        var imageSource = PictureDecoder.DecodeJpeg(imageStream);
        return imageSource;
      }
    }

    public static byte[] LoadFromLocalStorageArray(string imageFileName, string imageFolder)
    {
      var isoFile = IsolatedStorageFile.GetUserStoreForApplication();
      if (!isoFile.DirectoryExists(imageFolder))
      {
        isoFile.CreateDirectory(imageFolder);
      }
      string filePath = Path.Combine(imageFolder, imageFileName);
      if (!isoFile.FileExists(filePath))
      {
        return null;
      }
      using (var imageStream = isoFile.OpenFile(filePath, FileMode.Open, FileAccess.Read))
      {
        byte[] buffer = new byte[imageStream.Length];
        imageStream.Read(buffer, 0, buffer.Length);
        return buffer;
      }
    }

    public static void CopyFile(string fileSource, string fileDestination)
    {
      var isoFile = IsolatedStorageFile.GetUserStoreForApplication();
      if (!isoFile.FileExists(fileSource))
      {
        return;
      }
      using (var streamSource = isoFile.OpenFile(fileSource, FileMode.Open, FileAccess.Read))
      {
        using (var streamDestination = isoFile.CreateFile(fileDestination))
        {
          byte[] buffer = new byte[4096];
          int len = 0;
          while ((len = streamSource.Read(buffer, 0, buffer.Length)) > 0)
          {
            streamDestination.Write(buffer, 0, len);
          }
        }
      }
    }

    public static void DeleteFile(string fileSource)
    {
      var isoFile = IsolatedStorageFile.GetUserStoreForApplication();
      if (isoFile.FileExists(fileSource))
      {
        isoFile.DeleteFile(fileSource);
      }
    }

    public static bool FileExist(string fileSource)
    {
      var isoFile = IsolatedStorageFile.GetUserStoreForApplication();
      return isoFile.FileExists(fileSource);
    }
  }
}