using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Media.Imaging;

namespace Q42.Rijksmuseum.WP7.Services
{
    public static class ImageCacheService
    {
        public static ImageSource GetImage()
        {
            string file = "Images\\last.jpg"; 
            
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store == null)
                        return null; 
                    
                    if (!store.FileExists(file))
                        return null; 
                    
                    using (IsolatedStorageFileStream isfs = store.OpenFile(file, FileMode.Open))
                    {
                        if (isfs.Length > 0)
                        {
                            BitmapImage image = new BitmapImage();
                            image.SetSource(isfs);
                            isfs.Close();
                            return image;
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void SaveImage(Stream s)
        {
            string file = "Images\\last.jpg"; 
            
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!store.DirectoryExists("Images"))
                        store.CreateDirectory("Images"); using (IsolatedStorageFileStream isfs = store.CreateFile(file))
                    {
                        int count = 0;
                        byte[] buffer = new byte[1024];
                        s.Seek(0, SeekOrigin.Begin);
                        while (0 < (count = s.Read(buffer, 0, buffer.Length)))
                        {
                            isfs.Write(buffer, 0, count);
                        }
                        isfs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
