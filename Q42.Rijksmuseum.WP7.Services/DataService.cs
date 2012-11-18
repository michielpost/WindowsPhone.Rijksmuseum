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
using Q42.Rijksmuseum.WP7.Services.Model;
using mpost.WP7.Client.Storage;
using System.IO.IsolatedStorage;

namespace Q42.Rijksmuseum.WP7.Services
{
    public static class DataService
    {
        private static ReadXmlService reader = new ReadXmlService();

        public delegate void DataAvailableDelegate(RijksDataModel model);
        public static event DataAvailableDelegate DataAvailable;

        public static event EventHandler StartLoad;

        public delegate void EndLoadDelegate(bool isSucces);
        public static event EndLoadDelegate EndLoad;
       
        public static void UpdateData()
        {
            //Subscribe to event
            reader.ReadFinished += new ReadXmlService.ReadFinishedDelegate(reader_ReadFinished);
            reader.ReadError += new EventHandler(reader_ReadError);

            string lang = "en";
            if (IsolatedStorageSettings.ApplicationSettings.Contains("lang"))
            {
                lang = IsolatedStorageSettings.ApplicationSettings["lang"].ToString();
            }

            if (StartLoad != null)
                StartLoad(null, null);

            reader.Read(lang);

        }

        static void reader_ReadError(object sender, EventArgs e)
        {
            reader.ReadFinished -= new ReadXmlService.ReadFinishedDelegate(reader_ReadFinished);
            reader.ReadError -= new EventHandler(reader_ReadError);

            if (EndLoad != null)
                EndLoad(false);
        }

        private static void reader_ReadFinished(RijksDataModel model)
        {
            //Unsubscribe
            reader.ReadFinished -= new ReadXmlService.ReadFinishedDelegate(reader_ReadFinished);
            reader.ReadError -= new EventHandler(reader_ReadError);

            if (DataAvailable != null)
                DataAvailable(model);

            if (EndLoad != null)
                EndLoad(true);

            SaveData(model);

        }



        public static void SaveData(RijksDataModel model)
        {
            IsolatedStorageCacheManager<RijksDataModel>.Store("RijksDataModel.xml", model);
        }

        public static RijksDataModel LoadData()
        {
            RijksDataModel model = IsolatedStorageCacheManager<RijksDataModel>.Retrieve("RijksDataModel.xml");

            if (model == null || (DateTime.Now.Date - model.ReadDate.Date) >= new TimeSpan(2,0,0))
                UpdateData();

            return model;
        }
    }

}
