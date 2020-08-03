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

        //public HomePageView(Object Data)
        //{
            
        //    // this doesnt work WOW
        //    //string sURL = "D:\\PS kursov proekt\\kursov proekt-20190424T122106Z-001\\kursov proekt\\Geocaching\\GeoCacheGame\\GeoGacheApp\\html\\map.html";
        //    //Uri map_location = new Uri(@".\html\map.html");

        //    string curDir = Directory.GetCurrentDirectory();
          

            
        //    Currentuser = (User)Data;
        //    InitializeComponent();
        //    browser.Navigate((String.Format("file:///{0}/html/map.html", curDir)));
        //    HomePg = this;
        //    GeocachingContext context = new GeocachingContext();
        //    Markers = (from marker in context.MarkerInfos join treasure in context.Treasures on marker.TreasureId equals treasure.ID where treasure.UserId != Currentuser.ID select marker).ToList(); 
        //}

        //public User Currentuser { get => currentuser; set => currentuser = value; }
        //public List<Treasure> Treasures { get => treasures; set => treasures = value; }
        //public List<Treasure> FilteredTreasures { get => filteredTreasures; set => filteredTreasures = value; }
        //public List<MarkerInfo> Markers { get => markers; set => markers = value; }

        //private void ManageAccountClick(object sender, RoutedEventArgs e)
        //{
        //    //NavigationService nav = NavigationService.GetNavigationService(this);
        //    UserPageView mud = new UserPageView(Currentuser);
        //    //nav.Navigate(mud);
        //}

        //private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    UserPageView mud = new UserPageView(Currentuser);
        //    //this.NavigationService.Navigate(mud);
        //}
        

        //private void SetDefaultLocation(object sender, RoutedEventArgs e)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    stringBuilder.Append(Currentuser.Country + ", " + Currentuser.City + ", " + Currentuser.Adress);
        //    string str = stringBuilder.ToString();
        //    browser.InvokeScript("codeAdress", str);
        //}

        ////private void HideTreasureBttn_Click(object sender, RoutedEventArgs e)
        ////{
        ////    HideTreasurePageView ht = new HideTreasurePageView(Currentuser);
        ////    //this.NavigationService.Navigate(ht);
        ////}
        
        //private void SetupObjectForScripting(object sender, RoutedEventArgs e)
        //{
        //    ((WebBrowser)sender).ObjectForScripting = new HtmlInteropInternalClass(this);
        //}

        //private void FindTreasureBttn_Click(object sender, RoutedEventArgs e)
        //{
        //    foreach(MarkerInfo marker in markers)
        //    {
        //        GeocachingContext context = new GeocachingContext();
        //        Treasure treasr = (from trs in context.Treasures where trs.ID == marker.TreasureId select trs).FirstOrDefault(); 
        //        browser.InvokeScript("showTreasures", marker.Latitude, marker.Longtitude,
        //           treasr.ID, treasr.Name,treasr.TreasureType.ToString(),
        //           treasr.TreasureSize.ToString(),treasr.Description,
        //           treasr.Rating,treasr.isChained.ToString());
        //    }
            
        //}

        //[System.Runtime.InteropServices.ComVisibleAttribute(true)]
        //public class HtmlInteropInternalClass
        //{
        //    private readonly HomePageView hp;

        //    public HtmlInteropInternalClass(HomePageView hp)

        //    {
        //        this.hp = hp;
        //    }

            

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

