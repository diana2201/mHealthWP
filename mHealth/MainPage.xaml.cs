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
using TestTwitter.Net;
using mHealth.Models;


namespace mHealth
{
    public partial class MainPage : PhoneApplicationPage, TwitterClient.TwitterClientI
    {
        DataModel dataM;

        //TOKENS
        String consumerKey = "nshMf9JK3d4ggXitEHsY89RHx";
        String consumerSecret = "350JARBf0l1Mmcb5I2y93HY8EvBasIQmGPZvsbJqZk7DUx2srj";
        String accessToken = "3019866874-F8WLd1y5nwWJYJMZAwH4qBdXEUnQuukH62KMs1Q";
        String accesstokenSecret = "XJBcVG6Dmf4gxSCAVhGgBAF8imoaHJJHbJEWul902hEkA";
        String verifier = "uw7NjWHT6OJ1MpJOXsHfNxoAhPKpgI8BlYDhxEjIBY";

        //Twitter Api
        const String screenName = "ClusterCreaTIC";
        const String count = "30";

        TwitterClient twitter;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            twitter = new TwitterClient() { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret, AccessToken = accessToken, AccessTokenSecret = accesstokenSecret, OAuthVerifier = verifier };
            
            //CreatePushPins();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            
        }
        

        //Metodo para traer tweets
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                twitter.getTweets(screenName, count, this);
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

        private void irEric(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Expertos.xaml?item=1", UriKind.Relative));

        }

        private void irKenneth(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Expertos.xaml?item=0", UriKind.Relative));

        }

        private void irJuan(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Expertos.xaml?item=2", UriKind.Relative));

        }

        private void irDiego(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Expertos.xaml?item=3", UriKind.Relative));

        }

        private void goUnicauca(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("http://unicauca.edu.com", UriKind.Absolute);
            wbt.Uri = myUri;
            wbt.Show();

        }

        private void goAcopi(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("http://www.acopicauca.org.co/", UriKind.Absolute);
            wbt.Uri = myUri;
            wbt.Show();

        }

        private void goSusana(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("http://www.hosusana.gov.co/", UriKind.Absolute);
            wbt.Uri = myUri;
            wbt.Show();

        }

        private void goSanJose(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("http://www.hospitalsanjose.gov.co/", UriKind.Absolute);
            wbt.Uri = myUri;
            wbt.Show();

        }

        private void irPremios(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Premios.xaml", UriKind.Relative));
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



        public void userTweets(List<TestTwitter.Models.Tweet> tweets)
        {
            dataM = Application.Current.Resources["dataModel"] as DataModel;

            for (int i = 0; i < tweets.Count; i++)
            {
                dataM.Data.Add(tweets.ElementAt(i));
            }
        }
    }
}