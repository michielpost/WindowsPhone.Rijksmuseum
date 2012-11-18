using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using Q42.Rijksmuseum.WP7.Services.Model;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.ComponentModel;

namespace Q42.Rijksmuseum.WP7.Services
{
    public class ReadXmlService
    {
        public RijksDataModel DataModel { get; set; }

        // event declaration 
        public delegate void ReadFinishedDelegate(RijksDataModel model);
        public event ReadFinishedDelegate ReadFinished;
        public event EventHandler ReadError;

        public ReadXmlService()
        {
            DataModel = new RijksDataModel();
        }

        public void Read(string lang)
        {
            if (string.IsNullOrEmpty(lang))
                lang = "en";

            string url = "http://www.rijksmuseum.nl/data/widget3.jsp?lang=" + lang;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);

        }

        void ReadCallback(IAsyncResult asynchronousResult)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                using (StreamReader streamReader1 = new StreamReader(response.GetResponseStream()))
                {
                    string resultString = streamReader1.ReadToEnd();

                    System.Xml.Linq.XElement MyXElement = System.Xml.Linq.XElement.Parse(resultString);

                    DataModel = new RijksDataModel
                                {
                                    Exension = MyXElement.Element("config").Elements("image").Attributes("extension").First().Value,
                                    Path = MyXElement.Element("config").Elements("image").Attributes("path").First().Value,

                                    ArtistId = MyXElement.Element("artobject").Elements("artist").Attributes("id").First().Value,
                                    ArtistName = MyXElement.Element("artobject").Elements("artist").First().Value,
                                    CreationDate = MyXElement.Element("artobject").Elements("creationdate").Attributes("value").First().Value,
                                     Description = MyXElement.Element("artobject").Elements("description").First().Value,
                                    Link = MyXElement.Element("artobject").Elements("link").Attributes("href").First().Value,
                                    ObjectId = MyXElement.Element("artobject").Attributes("id").First().Value,
                                    Title = MyXElement.Element("artobject").Elements("title").First().Value.ToLower(),
                                    ReadDate = DateTime.Now
                                };

                    DataModel.Description = DataModel.Description.Replace("\r", string.Empty).Replace("\n", string.Empty);               

                }
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
                    ReadFinished(this.DataModel);
            });



        }
    }
}
