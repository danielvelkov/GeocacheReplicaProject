using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GeoGacheApp.Views
{
    /// <summary>
    /// Interaction logic for HideTreasure.xaml
    /// </summary>
    public partial class HideTreasure : Page,INotifyPropertyChanged
    {
        private double difficultyRating;
        private string difficulties;

        private string country;
        private string city;
        private string adress;
        private User currentUser;

        public double DifficultyRating
        {
            get { return difficultyRating; }
            set { if (difficultyRating != value)
                {
                    difficultyRating = value;
                    if(difficultyRating>= 1 && difficultyRating < 2)
                    {
                        Difficulties = "easy";
                    }
                    else if (difficultyRating >= 2 && difficultyRating < 3)
                    {
                        Difficulties = "normal";
                    }
                    else if (difficultyRating >= 3 && difficultyRating < 4)
                    {
                        Difficulties = "hard";
                    }
                    else if (difficultyRating >= 4 && difficultyRating < 5)
                    {
                        Difficulties = "impossible";
                    }
                    OnPropertyChanged("DifficultyRating");
                } }
        }

        public string Difficulties { get => difficulties;
            set { difficulties = value; OnPropertyChanged("Difficulties"); } }

        public string Country { get => country; set { country = value; OnPropertyChanged("Country"); DropMarker(this, new RoutedEventArgs()); } }
        public string City { get => city; set { city = value; OnPropertyChanged("City"); DropMarker(this,new RoutedEventArgs()); } }
        public string Adress { get => adress; set { adress = value; OnPropertyChanged("Adress"); DropMarker(this, new RoutedEventArgs()); } }

        public User CurrentUser { get => currentUser; set => currentUser = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            types.ItemsSource = Enum.GetNames(typeof(TreasureType));
            sizes.ItemsSource = Enum.GetNames(typeof(TreasureSizes));
            
        }
        

        public HideTreasure(Object user)
        {
            //string sURL = "D:\\PS kursov proekt\\kursov proekt-20190424T122106Z-001\\kursov proekt\\Geocaching\\GeoCacheGame\\GeoGacheApp\\html\\map_hiding.html";
            Uri map_location = new Uri("pack://siteoforigin:,,,/html/map_hiding.html");
            InitializeComponent();
            browser.Navigate(map_location);
            this.DataContext = this;
            city = string.Empty;
            adress = string.Empty;
            country = string.Empty;
            currentUser = user as User;
            foreach(UIElement elemnt in markerInfo.Children)
            {
                if(elemnt.GetType()==typeof(TextBox) || elemnt.GetType() == typeof(Button))
                {
                    elemnt.IsEnabled = false;
                }
            }

        }

        private void SetupObjectForScripting(object sender, RoutedEventArgs e)
        {
            ((WebBrowser)sender).ObjectForScripting = new HtmlInteropInternalTestClass(this);
        }

        [System.Runtime.InteropServices.ComVisibleAttribute(true)]
        public class HtmlInteropInternalTestClass
        {
            private HideTreasure hidTreasure;

            public HtmlInteropInternalTestClass()
            {

            }

            public HtmlInteropInternalTestClass(HideTreasure HideTreas)
            {
                hidTreasure = HideTreas;
            }

            public void EndDragMarkerCS(Decimal Lat, Decimal Lng)
            {
                hidTreasure.lat.Text = Math.Round(Lat, 5).ToString();
                hidTreasure.@long.Text = Math.Round(Lng, 5).ToString();   
            }
        }

        private void DropMarker(object sender, RoutedEventArgs e)
        {
            if(adress!="" || city != "" || country != "")
            {
                browser.InvokeScript("codeAdress", country + ", " + city + ", " + adress);
            }
        }

        private void EnableMarkerInfo(object sender, RoutedEventArgs e)
        {
            foreach(UIElement elemnt in markerInfo.Children)
            {
                if (elemnt.GetType() == typeof(TextBox) || elemnt.GetType() == typeof(Button))
                {
                    elemnt.IsEnabled = true;
                }
            }
        }

        private void SaveTreasure(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("" +
                "are you sure you wanna hide this treasure",
                "confirmation",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                UserContext context = new UserContext();
                var name = (from treasure in context.Treasures where treasureNameTxtBox.Text == treasure.Name select treasure).FirstOrDefault();
                if (name == null && treasureNameTxtBox.Text!=null && treasureNameTxtBox.Text.Length > 5 && types.SelectedItem != null && sizes.SelectedItem != null && lat.Text!=null && lat.Text.Length>0)
                {
                    TreasureType type;
                    Enum.TryParse(types.SelectedItem.ToString(), out type);
                    TreasureSizes size;
                    Enum.TryParse(sizes.SelectedItem.ToString(), out size);

                    context.Treasures.Add(
                        new Treasure(
                            treasureNameTxtBox.Text,
                            type,
                            size,
                            (new TextRange(descriptionTxtBox.Document.ContentStart, descriptionTxtBox.Document.ContentEnd).Text),
                            difficultyRating,
                            0,
                            currentUser.ID,
                            (bool)isChainedCheckBox.IsChecked
                            ));
                    context.SaveChanges();

                    var treasureId = context.Treasures
                       .OrderByDescending(t => t.ID)
                       .FirstOrDefault().ID;

                    context.MarkerInfos.Add(
                        new MarkerInfo(
                            treasureId,
                            Double.Parse(lat.Text),
                            Double.Parse(@long.Text),
                            City,
                            Country,
                            Adress));

                    context.SaveChanges();
                    MessageBox.Show("Treasure added!");
                    this.NavigationService.GoBack();
                }
                
                if(types.SelectedItem == null && sizes.SelectedItem == null)
                {
                    MessageBox.Show("select size or type");
                }
                if (lat.Text == null)
                {
                    MessageBox.Show("Drop the marker >:(");
                }
            }
        }    
            
        }
    }

