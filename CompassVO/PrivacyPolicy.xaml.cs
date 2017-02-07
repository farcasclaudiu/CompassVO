﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Resources;
using System.IO.IsolatedStorage;
using System.IO;

namespace CompassVO
{
  public partial class PrivacyPolicy : PhoneApplicationPage
  {
    public PrivacyPolicy()
    {
      InitializeComponent();
    }

    private void webPolicy_Loaded(object sender, RoutedEventArgs e)
    {
      SaveFilesToIsoStore();
      webPolicy.Navigate(new Uri("/Assets/Docs/privacy_policy.html", UriKind.Relative));
    }


    private void SaveFilesToIsoStore()
    {
      //These files must match what is included in the application package,
      //or BinaryStream.Dispose below will throw an exception.
      string[] files = {
            "Assets/Docs/privacy_policy.html"
        };

      IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

      if (false == isoStore.FileExists(files[0]))
      {
        foreach (string f in files)
        {
          StreamResourceInfo sr = Application.GetResourceStream(new Uri(f, UriKind.Relative));
          using (BinaryReader br = new BinaryReader(sr.Stream))
          {
            byte[] data = br.ReadBytes((int)sr.Stream.Length);
            SaveToIsoStore(f, data);
          }
        }
      }
    }

    private void SaveToIsoStore(string fileName, byte[] data)
    {
      string strBaseDir = string.Empty;
      string delimStr = "/";
      char[] delimiter = delimStr.ToCharArray();
      string[] dirsPath = fileName.Split(delimiter);

      //Get the IsoStore.
      IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

      //Re-create the directory structure.
      for (int i = 0; i < dirsPath.Length - 1; i++)
      {
        strBaseDir = System.IO.Path.Combine(strBaseDir, dirsPath[i]);
        isoStore.CreateDirectory(strBaseDir);
      }

      //Remove the existing file.
      if (isoStore.FileExists(fileName))
      {
        isoStore.DeleteFile(fileName);
      }

      //Write the file.
      using (BinaryWriter bw = new BinaryWriter(isoStore.CreateFile(fileName)))
      {
        bw.Write(data);
        bw.Close();
      }
    }
  }
}