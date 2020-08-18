using CefSharp;
using CefSharp.Wpf;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Geocache.Enums;
using Geocache.Helper;
using Geocache.Models;
using Geocache.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Ioc;
using Geocache.ViewModel.BrowserVM;

namespace Geocache.ViewModel
{
    public class HideTreasurePageVM : ViewModelBase
    {
        private TaskCompletionSource<bool> tcs;
        public HideTreasurePageVM(UserDataService userdata)
        {
            UserData = userdata;
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;
            MessengerInstance.Register<Treasure>(this, "ChangeTreasure", async Treasur =>
            { Treasure = Treasur; MarkerInfo = Treasur.MarkerInfo;
                Latitude = MarkerInfo.Latitude;
                Longtitude = MarkerInfo.Longtitude;
                Address = MarkerInfo.Address;
                City = MarkerInfo.City;
                Country = MarkerInfo.Country;
                Key = Treasur.Key;
                SelectedTreasureSize = Treasur.TreasureSize;
                SelectedTreasureType = Treasur.TreasureType;
                UserTreasures = UserData.GetUnchainedUserTreasures(Treasur.ID);
                // when the mainframe of the browser loads this runs
                 tcs= new TaskCompletionSource<bool>();
                await tcs.Task;
                WebBrowser.ExecuteScriptAsync("setMarker", Latitude, Longtitude);
                
            });
        }

        #region fields
        private TreasureType selectedTreasureType = TreasureType.NORMAL;
        private TreasureSizes selectedTreasureSize = Enums.TreasureSizes.MEDIUM;
        private Treasure treasure;
        private MarkerInfo markerInfo;
        private Treasure treasureChain;
        private List<Treasure> userTreasures;
        #endregion

        #region Params
        public UserDataService UserData { get; set; }

        public TreasureType SelectedTreasureType
        {
            get
            {
                return selectedTreasureType;
            }
            set
            {
                selectedTreasureType = value;
                RaisePropertyChanged("SelectedTreasureType");
            }
        }

        public IEnumerable<TreasureType> TreasureTypes
        {
            get
            {
                return Enum.GetValues(typeof(TreasureType))
                    .Cast<TreasureType>().Except(new TreasureType[] { TreasureType.ANY });
            }
        }

        public TreasureSizes SelectedTreasureSize
        {
            get { return selectedTreasureSize; }
            set
            {
                selectedTreasureSize = value;
                RaisePropertyChanged("SelectedTreasureSize");
            }
        }

        public IEnumerable<TreasureSizes> TreasureSizes
        {
            get
            {
                return Enum.GetValues(typeof(TreasureSizes))
                    .Cast<TreasureSizes>().Except(new TreasureSizes[] { Enums.TreasureSizes.ANY});
            }
        }

        public Treasure Treasure
        {
            get
            {
                if (treasure == null)
                    treasure = new Treasure();
                return treasure;
            }
            set
            {
                if (treasure != value)
                    treasure = value;
            }
        }

        public MarkerInfo MarkerInfo
        {
            get
            {
                if (markerInfo == null)
                    markerInfo = new MarkerInfo();
                return markerInfo;
            }
            set
            {
                if (markerInfo != value)
                    markerInfo = value;
                RaisePropertyChanged("MarkerInfo");
            }
        }

        public List<Treasure> UserTreasures
        {
            get
            {
                if (userTreasures == null)
                {
                    userTreasures= UserData.GetUnchainedUserTreasures();
                }
                return userTreasures;
            }
            set
            {
                userTreasures = value;
            }
        }

        public Treasure TreasureChain
        {
            get
            {
                if (treasureChain == null)
                    treasureChain = new Treasure();
                return treasureChain;
            }
            set
            {
                if (treasureChain != value)
                    treasureChain = value;
                RaisePropertyChanged("TreasureChain");
            }
        }

        private double latitude;
        public const string LatitudePropertyName = "Latitude";
        
        public double Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                if (latitude == value)
                {
                    return;
                }

                latitude = value;
                RaisePropertyChanged(LatitudePropertyName);
            }
        }

        private double longitude;
        public const string LongtitudeProperyName = "Longtitude";
        
        public double Longtitude
        {
            get
            {
                return longitude;
            }

            set
            {
                if (longitude == value)
                {
                    return;
                }

                longitude = value;
                RaisePropertyChanged(LongtitudeProperyName);
            }
        }
        private string key;
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                if (key != value)
                {
                    key = value;
                    RaisePropertyChanged("Key");
                }
            }
        }

        private string address;
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                if (address != value)
                {
                    address = value;
                    RaisePropertyChanged("Address");
                }
            }
        }

        private string city;
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                if (city != value)
                {
                    city = value;
                    RaisePropertyChanged("City");
                }
            }
        }

        private string country;
        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                if (country != value)
                {
                    country = value;
                    RaisePropertyChanged("Country");
                }
            }
        }

        private ChromiumWebBrowser webBrowser;
        public const string WebBrowserPropertyName = "WebBrowser";

        ///// <summary>
        ///// Sets and gets the WebBrowser property.
        ///// Changes to that property's value raise the PropertyChanged event. 
        ///// </summary>
        public ChromiumWebBrowser WebBrowser
        {
            get
            {
                return webBrowser;
            }

            set
            {
                if (webBrowser == value)
                {
                    return;
                }

                webBrowser = value;
                // second check is when the destruction is called
                if (webBrowser != null && webBrowser.Address == null)
                {
                    webBrowser.Address = "localfolder://cefsharp/map_hiding.html";
                    webBrowser.JavascriptObjectRepository.Register("hideTreasureVM",
                        this, true, BindingOptions.DefaultBinder);
                    if (tcs!=null)
                    webBrowser.FrameLoadEnd += WebBrowser_FrameLoadEnd;
                }
                RaisePropertyChanged(WebBrowserPropertyName);
            }
        }

        private void WebBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            //that means v8 context is loaded so we can actually stop the task
            tcs.SetResult(true);
            webBrowser.FrameLoadEnd -= WebBrowser_FrameLoadEnd;
        }
        #endregion

        #region Commands
        ICommand generateKey;
        public ICommand GenerateKey
        {
            get
            {
                return generateKey ?? (generateKey =
                  new RelayCommand(() =>
                  {
                      Key = Treasure.GenerateKey();
                  }));
            }
        }
        private ICommand goBack;
        public ICommand GoBack
        {
            get
            {
                return goBack ?? (goBack =
                  new RelayCommand((() =>
                  {
                      SimpleIoc.Default.Unregister(this);
                      MessengerInstance.Send<Type>(typeof(HomePageVM), "ChangePage");
                  })
                ));

            }
        }
        ICommand dropMarker;
        public ICommand DropMarker
        {
            get
            {
                return dropMarker ?? (dropMarker =
                  new RelayCommand(() =>
                  {
                      MarkerInfo.Address = Address;
                      MarkerInfo.Country = Country;
                      MarkerInfo.City = City;
                      WebBrowser.ExecuteScriptAsync("codeAdress", MarkerInfo.GetMarkerAddress());
                  }));
            }
        }
        ICommand saveTreasure;
        public ICommand SaveTreasure
        {
            get
            {
                return saveTreasure ?? (saveTreasure =
                    new RelayCommand(() =>
                    {
                        //coordinates are vital so we check first
                        if(!Double.IsNaN(Latitude))
                        {
                            using(var UnitOfWork= new UnitOfWork(new GeocachingContext()))
                            {
                                // if we came to change treasure
                                if (Treasure.ID != 0)
                                {
                                    var changedMarker = UnitOfWork.Treasures.GetTreasureInfo(Treasure.ID);
                                    changedMarker.Address = Address;
                                    changedMarker.City = City;
                                    changedMarker.Country = Country;
                                    changedMarker.Latitude = Latitude;
                                    changedMarker.Longtitude = Longtitude;
                                    UnitOfWork.Complete();
                                    // wish i  could have operand overloading
                                    var changedTreasure = UnitOfWork.Treasures.Get(Treasure.ID);
                                    changedTreasure.Name = Treasure.Name;
                                    changedTreasure.Description = Treasure.Description;
                                    changedTreasure.Difficulty = Treasure.Difficulty;
                                    //TODO handle chained
                                    changedTreasure.TreasureSize = SelectedTreasureSize;
                                    changedTreasure.TreasureType = SelectedTreasureType;
                                    changedTreasure.Key = Key;
                                    UnitOfWork.Complete();
                                    MessageBox.Show("Changes saved");
                                    GoBack.Execute(null);
                                }
                                else //if we are hiding the treasure now
                                {
                                    Treasure.Key = Key;
                                    Treasure.Rating = 0;
                                    Treasure.UserId = UserData.CurrentUser.ID;
                                    Treasure.TreasureSize = SelectedTreasureSize;
                                    Treasure.TreasureType = SelectedTreasureType;
                                    //only lat nad long are actually needed for the position of the marker
                                    MarkerInfo.Latitude = Latitude;
                                    MarkerInfo.Longtitude = Longtitude;

                                    UnitOfWork.Treasures.Add(Treasure);
                                    Treasure.MarkerInfo = MarkerInfo;

                                    UnitOfWork.Complete();

                                    if (Treasure.IsChained && TreasureChain.ID != 0)
                                    {
                                        UnitOfWork.ChainedTreasures.Add(
                                            new Chained_Treasures(TreasureChain.ID, Treasure.ID));
                                        UnitOfWork.Complete();
                                        TreasureChain.IsChained = true;
                                        UnitOfWork.Complete();
                                    }
                                    // always set the new treasure to ischained=false so you can chain it
                                    Treasure.IsChained = false;
                                    UnitOfWork.Complete();

                                    MessageBoxResult result = MessageBox.Show("Treasure added succesfully", "Success", MessageBoxButton.OK);
                                    if (result == MessageBoxResult.OK)
                                        MessageBox.Show(string.Format("The key can always be changed."+
                                            " If you have your own key for the treasure, go to MyTreasures and change it!", Treasure.Key), "Key generated", MessageBoxButton.OK);
                                    GoBack.Execute(null);
                                }
                            }
                        }
                    }));
            }
        }
        
        #endregion

        #region methods
        public void endDragMarkerCS(double Lat, double Lng,string address)
        {
            Latitude = Lat;
            Longtitude = Lng;
            string[] names = address.Split(',');
            if (names.Length > 3)
            {
                Address = "";
                int index = 0;
                while (((names.Length) - 2) > index)
                    Address += names[index++];
            }
            else
            Address = names[0];
            City = names[names.Length - 2];
            Country= names[names.Length - 1];

            MarkerInfo.Address = Address;
            MarkerInfo.City = City;
            MarkerInfo.Country = Country;
        }
        #endregion
    }
}
