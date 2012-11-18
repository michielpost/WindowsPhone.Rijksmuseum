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
using System.Windows.Media.Imaging;
using Q42.Rijksmuseum.WP7.Services;
using Q42.Rijksmuseum.WP7.Services.Model;

namespace Q42.Rijksmuseum.WP7
{
    public partial class MainPage : PhoneApplicationPage
    {

        ImageService imageService = new ImageService();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            DataService.DataAvailable += new DataService.DataAvailableDelegate(DataService_DataAvailable);
            DataService.StartLoad += new EventHandler(DataService_StartLoad);
            DataService.EndLoad += new DataService.EndLoadDelegate(DataService_EndLoad);

            imageService.ReadFinished += new EventHandler(imageService_ReadFinished);
           
        }

       

        void DataService_EndLoad(bool isSucces)
        {
            LoadingGrid.Visibility = System.Windows.Visibility.Collapsed;
            PPB.IsIndeterminate = false;
            PPB.Visibility = Visibility.Collapsed;

            if (!isSucces)
            {
                MessageBox.Show("Unable to contact Rijksmuseum. Please check your internet connection.");

                ErrorBlock.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ErrorBlock.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void DataService_StartLoad(object sender, EventArgs e)
        {
            LoadingGrid.Visibility = System.Windows.Visibility.Visible;
            PPB.IsIndeterminate = true;
            PPB.Visibility = Visibility.Visible;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            RijksDataModel model = DataService.LoadData();

            ModelUpdated(model);
        }

        private void ModelUpdated(RijksDataModel model)
        {
            if (model != null)
            {
                this.DataContext = model;

               

                if (MainImage.Source == null)
                {
                    ImageSource img = ImageCacheService.GetImage();
                    if (img != null)
                        MainImage.Source = img;
                }

                //BitmapImage image = new BitmapImage();
                //image.ImageOpened += new EventHandler<RoutedEventArgs>(image_ImageOpened);
                //image.UriSource = new Uri(url);
                //MainImage.Source = image;
            }
        }

        void DataService_DataAvailable(Services.Model.RijksDataModel model)
        {
            ModelUpdated(model);

            //Download and Save new image
            //http://rijksmuseum.nl/assetimage.jsp?id=SK-A-1610&widget/size260&r1969
            string url = model.Path + model.ObjectId + "&widget/size260";

            imageService.Read(url);
        }

        void imageService_ReadFinished(object sender, EventArgs e)
        {
            ImageSource img = ImageCacheService.GetImage();
            if (img != null)
                MainImage.Source = img;
        }


        private void MainImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
        }
    }
}