using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace mHealth
{
    public partial class Sponsors : PhoneApplicationPage
    {
        public Sponsors()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.ContainsKey("item"))
            {
                var index = NavigationContext.QueryString["item"];
                var indexParsed = int.Parse(index);
                beneficios.SelectedIndex = indexParsed;
            }
        }

        private void Descargar(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            Uri myUri = new Uri("https://drive.google.com/file/d/0B_5XdxwasalCVktzVEVQdThscG8/view?usp=sharing", UriKind.Absolute);
            wbt.Uri = myUri;
            wbt.Show();
        }
    }
}