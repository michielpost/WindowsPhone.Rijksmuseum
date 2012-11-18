using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using Q42.Rijksmuseum.WP7.Services;
using Q42.Rijksmuseum.WP7.Services.Model;
using Microsoft.Phone.Tasks;

namespace Q42.Rijksmuseum.WP7
{
    public partial class InfoPage : PhoneApplicationPage
    {
        private RijksDataModel _model;
        public InfoPage()
        {
            InitializeComponent();

            DataService.DataAvailable += new DataService.DataAvailableDelegate(DataService_DataAvailable);
            DataService.StartLoad += new EventHandler(DataService_StartLoad);
            DataService.EndLoad+=new DataService.EndLoadDelegate(DataService_EndLoad);

            var bgc = App.Current.Resources["PhoneBackgroundColor"].ToString();
            if (bgc != "#FF000000")
            {
                LoadingGrid.Background = new SolidColorBrush(Colors.White);
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            RijksDataModel model = DataService.LoadData();
            ModelUpdated(model);

            SetLabels();
        }

        void DataService_EndLoad(bool isSucces)
        {
            LoadingGrid.Visibility = System.Windows.Visibility.Collapsed;
            PPB.IsIndeterminate = false;
            PPB.Visibility = Visibility.Collapsed;
        }

        void DataService_StartLoad(object sender, EventArgs e)
        {
            LoadingGrid.Visibility = System.Windows.Visibility.Visible;
            PPB.IsIndeterminate = true;
            PPB.Visibility = Visibility.Visible;
        }

        private void ModelUpdated(RijksDataModel model)
        {
            if (model != null)
            {
                this.DataContext = model;

                _model = model;
            }
        }

        private void SetLabels()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("lang") && IsolatedStorageSettings.ApplicationSettings["lang"].ToString() == "NL")
            {
                MoreInfoButton.Content = "Meer Info";
            }
            else
            {
                MoreInfoButton.Content = "More Info";
            }
        }


        void DataService_DataAvailable(Services.Model.RijksDataModel model)
        {
            ModelUpdated(model);
        }

        private void ENMenuItem_Click(object sender, EventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["lang"] = "EN";

            SetLabels();
            DataService.UpdateData();
        }

        private void NLMenuItem_Click(object sender, EventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["lang"] = "NL";

            SetLabels();
            DataService.UpdateData();

        }

        private void MoreInfoButton_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.URL = "http://www.rijksmuseum.nl" + _model.Link;
            browser.Show();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
    }
}