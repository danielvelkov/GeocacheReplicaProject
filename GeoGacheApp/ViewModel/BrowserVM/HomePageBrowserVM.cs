using CefSharp;
using CefSharp.Wpf;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Geocache.Database;
using Geocache.Enums;
using Geocache.Helper;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Geocache.ViewModel.BrowserVM
{
    public class HomePageBrowserVM : ViewModelBase
    {
        public UserDataService UserData { get; }


        private TreasureType selectedTreasureType= TreasureType.ANY;
        private TreasureSizes selectedTreasureSize= Enums.TreasureSizes.ANY;

        // good idea to bind it 
        private string address;
        public string Address
        {
            get { return address; }
            set { Set(ref address, value); }
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
                    webBrowser.Address = "localfolder://cefsharp/";
                    webBrowser.JavascriptObjectRepository.Register("homePageVM",
                            this, true, BindingOptions.DefaultBinder);
                }
                RaisePropertyChanged(WebBrowserPropertyName);
            }
        }
        

        public HomePageBrowserVM(UserDataService userData)
        {
            Markers = new List<MarkerInfo>();
            UserData = userData;
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;
        }
        

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
                    .Cast<TreasureType>();
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
                    .Cast<TreasureSizes>();
            }
        }
        public const string DifficultyPropertyName = "Difficulty";

        private double difficulty ;

        /// <summary>
        /// Sets and gets the Difficulty property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double Difficulty
        {
            get
            {
                return difficulty;
            }

            set
            {
                if (difficulty == value)
                {
                    return;
                }

                difficulty = value;
                RaisePropertyChanged(DifficultyPropertyName);
            }
        }
        
        public const string RadiusPropertyName = "Radius";

        private double radius=1;

        /// <summary>
        /// Sets and gets the Radius property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double Radius
        {
            get
            {
                return radius;
            }

            set
            {
                if (radius == value)
                {
                    return;
                }

                radius = value;
                RaisePropertyChanged(RadiusPropertyName);
            }
        }

        public const string FindChainedTreasuresName = "FindChainedTreasures";

        private bool findChainedTreasures = false;

        /// <summary>
        /// Sets and gets the findChainedTreasures property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool FindChainedTreasures
        {
            get
            {
                return findChainedTreasures;
            }

            set
            {
                if (findChainedTreasures == value)
                {
                    return;
                }

                findChainedTreasures = value;
                RaisePropertyChanged(FindChainedTreasuresName);
            }
        }
        // if the user wants to see found by him treasures
        public const string FoundByUserName = "FoundByUser";

        private bool foundByUser=false;

        /// <summary>
        /// Sets and gets the foundByUser property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool FoundByUser
        {
            get
            {
                return foundByUser;
            }

            set
            {
                if (foundByUser == value)
                {
                    return;
                }

                foundByUser = value;
                RaisePropertyChanged(FoundByUserName);
            }
        }


        /// <summary>
        /// The <see cref="CurrentLocation" /> property's name.
        /// </summary>
        public const string CurrentLocationPropertyName = "CurrentLocation";

        private Location currentLocation;

        /// <summary>
        /// Sets and gets the CurrentLocation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Location CurrentLocation
        {
            get
            {
                if (currentLocation == null)
                    currentLocation = new Location(0,0);
                return currentLocation;
            }

            set
            {
                if (currentLocation == value)
                {
                    return;
                }

                currentLocation = value;
                RaisePropertyChanged(CurrentLocationPropertyName);
            }
        }

        List<MarkerInfo> markers;
        public List<MarkerInfo> Markers { get => markers; set => markers = value; }

        private ICommand filterTreasures;
        private ICommand goToHomeLocation;
        public ICommand FilterTreasures
        {
            get
            {
                if (filterTreasures == null)
                    filterTreasures = new RelayCommand(() =>
                    {
                        WebBrowser.ExecuteScriptAsync("removeMarkers","removed");
                        using (var UnitofWork = new UnitOfWork(new GeocachingContext()))
                        {
                            foreach (var treas in UnitofWork.Treasures.GetOthersTreasures(UserData.CurrentUser.ID))
                            {
                                if((SelectedTreasureSize==Enums.TreasureSizes.ANY || treas.TreasureSize==SelectedTreasureSize) &&
                                (SelectedTreasureType==TreasureType.ANY || treas.TreasureType==SelectedTreasureType) &&
                                (Difficulty==0 || treas.Difficulty<=Difficulty) && treas.IsChained==FindChainedTreasures
                                )
                                {
                                    if(treas.MarkerInfo.IsInRadius(CurrentLocation.Lat, CurrentLocation.Lon,Radius))
                                        Markers.Add(treas.MarkerInfo);
                                }
                            }

                            if (Markers.Count != 0)
                            foreach (MarkerInfo marker in Markers)
                            {
                                Treasure treasr = UnitofWork.Treasures.Get(marker.TreasureId);
                                WebBrowser.ExecuteScriptAsync("showTreasures", marker.Latitude, marker.Longtitude,
                                treasr.ID, treasr.Name, treasr.TreasureType.ToString(),
                                treasr.TreasureSize.ToString(), treasr.Description,
                                treasr.Rating, treasr.IsChained.ToString());
                            }
                            Markers.Clear();
                        }
                    });
                return filterTreasures;
            }
        }

        public ICommand GoToHomeLocation
        {
            get
            {
                if (goToHomeLocation == null)
                    goToHomeLocation = new RelayCommand(() =>
                      {
                          CurrentLocation = new Location(UserData.GetUserHomeAddress());
                          string address = UserData.GetUserHomeAddress();
                          WebBrowser.ExecuteScriptAsync("codeHomeAdress", address);
                      });
                return goToHomeLocation;
            }
        }

        #region methods
        public void endDragMarkerCS(double Lat, double Lng)
        {
            CurrentLocation = new Location(Lat, Lng);
        }

        public void startHuntCS(double lat, double lng, string name, int id)
        {
            if (MessageBox.Show("are you sure you wanna search for " +
                   "treasure at coordinates:" + lat.ToString().Substring(0,4) +
                   "- " + lng.ToString().Substring(0, 4) + " " + "\nwith name: " + name, "confirmation", MessageBoxButton.YesNo) ==
                   MessageBoxResult.Yes)
            {
                SimpleIoc.Default.GetInstance<UserDataService>().UserLocation = CurrentLocation;
                if (!SimpleIoc.Default.IsRegistered<FoundTreasureArgs>())
                {
                    SimpleIoc.Default.Register<FoundTreasureArgs>(() =>
                    {
                        return new FoundTreasureArgs
                        {
                            FoundTreasureId = id,
                            FoundTreasureLocation = new Location(lat, lng)
                        };
                    });
                    SimpleIoc.Default.Register<FoundTreasureWindowController>();
                }
                else
                {
                    SimpleIoc.Default.GetInstance<FoundTreasureArgs>().FoundTreasureId = id;
                    SimpleIoc.Default.GetInstance<FoundTreasureArgs>().FoundTreasureLocation = new Location(lat, lng);
                }
                MessengerInstance.Send<ViewModelBase>(ViewModelLocator.FindTreasureVM, "ChangePage");
            }
        }

        public void getAllTreasuresCS(string address)
        {
            string[] names = address.Split(',');
            if (names.Length >= 2)
            {
                string country = names[names.Length-1];
                string city = names[names.Length - 2];
                WebBrowser.ExecuteScriptAsync("removeMarkers", "removed");
                using (var UnitofWork = new UnitOfWork(new GeocachingContext()))
                {
                    foreach (var treas in UnitofWork.Treasures.GetOthersTreasures(UserData.CurrentUser.ID))
                    {
                        if ((SelectedTreasureSize == Enums.TreasureSizes.ANY || treas.TreasureSize == SelectedTreasureSize) &&
                        (SelectedTreasureType == TreasureType.ANY || treas.TreasureType == SelectedTreasureType) &&
                        (Difficulty == 0 || treas.Difficulty <= Difficulty) && treas.IsChained == FindChainedTreasures
                        )
                        {
                            if (treas.MarkerInfo.Country.Contains(country, StringComparison.OrdinalIgnoreCase) &&
                                treas.MarkerInfo.City.Contains(city, StringComparison.OrdinalIgnoreCase))
                                Markers.Add(treas.MarkerInfo);
                        }
                    }

                    if (Markers.Count != 0)
                        foreach (MarkerInfo marker in Markers)
                        {
                            Treasure treasr = UnitofWork.Treasures.Get(marker.TreasureId);
                            WebBrowser.ExecuteScriptAsync("showTreasures", marker.Latitude, marker.Longtitude,
                            treasr.ID, treasr.Name, treasr.TreasureType.ToString(),
                            treasr.TreasureSize.ToString(), treasr.Description,
                            treasr.Rating, treasr.IsChained.ToString());
                        }
                    Markers.Clear();
                }
            }
        }
        #endregion
    }
}
