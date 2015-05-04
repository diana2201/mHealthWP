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


namespace mHealth
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
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

        }
        //Fin enviar correo electrónico

        //Inicio IrTwitter
        private void IrTwitter(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("http://twitter.com/ClusterCreaTIC", UriKind.Absolute);
            wbt.Uri= myUri;
            wbt.Show();
        }
        //Fin IrTwitter
        
        //Inicio Ir a Página Web
        private void IrWeb(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("http://parquesoftpopayan.com", UriKind.Absolute);
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
        
       
    }
}