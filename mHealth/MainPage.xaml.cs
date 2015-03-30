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


        private void IrActividades(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Actividades.xaml", UriKind.Relative));

        }

      
        private void IrPlatinium(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SponsorsPlatinium.xaml", UriKind.Relative));
        }

        private void IrGolden(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SponsorsGolden.xaml", UriKind.Relative));
        }

        private void IrSilver(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SponsorsPlata.xaml", UriKind.Relative));
        }


        //Metodo para traer twits
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //Obtain keys by registering your app on https://dev.twitter.com/apps
                //                                 costumer key             ,   costumer key secret
                var service = new TwitterService("VPRQWK37J2z2EIntchiWFT4Nt", "GCPwQOG91rAtQHY8VuByzT0GNqjqFXP8Dob6w3ZbdibxzlOU43");
                //                                token                                     ,        token secret
                service.AuthenticateWith("435393700-T7HqdOi4eg2TKeDLwD9CA11vbpuKOYlQ5psVwFPb", "CsOG3mEZYB5rT3TcUHp3Wgi8lpQ8zCMp2GFsUTQbSzo3S");

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

       

        //Metodo para enviar correos

        

        

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}