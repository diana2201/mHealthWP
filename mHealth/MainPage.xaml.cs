using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using mHealth.Resources;
using System.Net.NetworkInformation;
using TweetSharp;
using Windows.UI.Popups;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;


namespace mHealth
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //CreatePushPins();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
              
        private void IrPlatinium(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Sponsors.xaml?item=2", UriKind.Relative));
        }

        private void IrGolden(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Sponsors.xaml?item=1", UriKind.Relative));
        }

        private void IrSilver(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Sponsors.xaml?item=0", UriKind.Relative));
        }


        //Metodo para traer tweets
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //Obtain keys by registering your app on https://dev.twitter.com/apps
                //                                 costumer key             ,   costumer key secret
                var service = new TwitterService("YXO8K2jFXbiKyLNDdn7ydFRPU", "VkVSDQ44Z9oJUm2PLHUJl9OW3y30J5xNjwlrLW4Bzp0MNta68S");
                service.AuthenticateWith("3170504927-kcmyI4GV9ZMUvU9sYxJR6xoXHthE6DlmUsGVdiI", "EEeTKnuc5VwYIi6vygKu7WVznItuavm5G6lBY2CXwkfkO");
                //ScreenName is the profile name of the twitter user.
                service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions() { ScreenName = "ClusterCreaTic" }, (ts, rep) =>
                {
                    if (rep.StatusCode == HttpStatusCode.OK)
                    {

                        ObservableCollection<TwitterStatus> data = new ObservableCollection<TwitterStatus>(ts); 
                        //bind
                        this.Dispatcher.BeginInvoke(() => { tweetList.ItemsSource = data; });
                    }
                });
            }
            else
            {

                MessageBox.Show("Please check your internet connestion.");
                
            }

        }


        //private async void ComposeEmail(Windows.ApplicationModel.Contacts.Contact recipient, string messageBody)

        private void EnviarCorreo(object sender, RoutedEventArgs e)
        {
            ComposeEmail();
        }

        //Inicio código enviar correo electrónico
        private async void ComposeEmail()
        {
            var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
            emailMessage.Body = msjcorreo.Text;
            emailMessage.Subject = "Contacto mHealth";
            var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient("dianasamboni22@gmail.com");
            emailMessage.To.Add(emailRecipient);            
            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);
            msjcorreo.Text = " ";

        }
        //Fin enviar correo electrónico

        //Inicio IrTwitter
        private void IrTwitter(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("http://twitter.com/mHealthCO", UriKind.Absolute);
            wbt.Uri= myUri;
            wbt.Show();
        }
        //Fin IrTwitter
        
        //Inicio Ir a Página Web
        private void IrWeb(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("http://mhealth.com.co", UriKind.Absolute);
            wbt.Uri = myUri;
            wbt.Show();
        }

        private void irEntrenamiento(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Actividades.xaml?item=0", UriKind.Relative));
        }
        private void irHackaton(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Actividades.xaml?item=1", UriKind.Relative));
        }

        private void irSana(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Actividades.xaml?item=2", UriKind.Relative));
        }

        private void irIncubadora(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Actividades.xaml?item=3", UriKind.Relative));
        }

        private void myMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "5e328859-ceac-4982-a76b-e518b022ecc7";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "nKwcC-RhskoUqHpf1Psh2A";
        }

        private void goCsterPage(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("http://parquesoftpopayan.com", UriKind.Absolute);
            wbt.Uri = myUri;
            wbt.Show();
        }

        private void goMicrosoftPage(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("https://www.microsoft.com/es-co/", UriKind.Absolute);
            wbt.Uri = myUri;
            wbt.Show();
        }

        private void goGobernacionPage(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("https://www.cauca.gov.co", UriKind.Absolute);
            wbt.Uri = myUri;
            wbt.Show();
        }

        private void goNucleoInovacionPage(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("https://www.parquesoftpopayan.com", UriKind.Absolute);
            wbt.Uri = myUri;
            wbt.Show();
        }

        //private void CreatePushPins()
        //{

        //    var overlay = new MapOverlay();
        //    overlay.GeoCoordinate = new System.Device.Location.GeoCoordinate()
        //    {
        //        //2.4524619261148266
        //        //-76.59806044358595
        //        Latitude =  2.453608,
        //        Longitude = -76.598292,
        //    };

        //    BitmapImage image = new BitmapImage(new Uri("/Images/puntero1.png", UriKind.Relative));
        //    Rectangle ellipse = new Rectangle
        //    {
        //        Height = 75,
        //        Width = 75,
        //    };
        //    ImageBrush profileImageBrush = new ImageBrush();
        //    profileImageBrush.ImageSource = image;
        //    ellipse.Fill = profileImageBrush;
        //    overlay.Content = new Rectangle()
        //    {
        //        Width = 50,
        //        Height = 50,
        //        Fill = profileImageBrush
        //    };

        //    MapLayer layer = new MapLayer();
        //    layer.Add(overlay);
        //    MyMap.Layers.Add(layer);

        //}
        
       
    }
}