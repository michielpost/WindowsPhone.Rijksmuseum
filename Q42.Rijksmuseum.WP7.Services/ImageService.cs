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
using System.Windows.Threading;

namespace Q42.Rijksmuseum.WP7.Services
{
    public class ImageService
    {
        public event EventHandler ReadFinished;
        public event EventHandler ReadError;


        public void Read(string url)
        {
           
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);

        }

        void ReadCallback(IAsyncResult asynchronousResult)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);

                ImageCacheService.SaveImage(response.GetResponseStream());
              
            }
            catch (Exception e)
            {

                SmartDispatcher.BeginInvoke(delegate
                {
                    if (ReadError != null)
                        ReadError(this, null);
                });
            }


            SmartDispatcher.BeginInvoke(delegate
            {
                if (ReadFinished != null)
                    ReadFinished(this, null);
            });



        }
    }
}
