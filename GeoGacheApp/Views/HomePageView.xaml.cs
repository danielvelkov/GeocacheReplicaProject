using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Geocache.Views
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePageView : UserControl
    {
        
        public HomePageView()
        {
            InitializeComponent();
        }

       
        //    public void StartHuntCS(Decimal lat, Decimal lng, string name)
        //    {
        //        if(MessageBox.Show("are you sure you wanna search for" +
        //            "treasure at coords:" + lat.ToString() +
        //            " " + lng.ToString() + " " + "\nwith name:" + name, "confirmation", MessageBoxButton.YesNo)==
        //            MessageBoxResult.Yes)
        //        {
                    
        //            //string sURL = "D:\\PS kursov proekt\\kursov proekt-20190424T122106Z-001\\kursov proekt\\Geocaching\\GeoCacheGame\\GeoGacheApp\\html\\map_route.html";
        //            Uri map_location = new Uri("pack://siteoforigin:,,,/html/map_route.html");
        //            HomePg.browser.Navigate(map_location);
        //            if (HomePg.browser.IsLoaded) {
        //                HomePg.browser.InvokeScript("setDestination", new Object[] { lat, lng });

        //                HomePg.browser.InvokeScript("setOrigin", HomePg.currentuser.Country + ", "
        //                                    + HomePg.currentuser.City + ", " + HomePg.currentuser.Adress);
        //                HomePg.browser.InvokeScript("initMap1");
        //            }
                    
        //        } 
        //    }
        
    }
}

