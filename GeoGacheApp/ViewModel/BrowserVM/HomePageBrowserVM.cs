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
using Geocache.Models.WrappedModels;
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
        public HomePageBrowserVM(UserDataService userData)
        {
            Markers = new List<MarkerInfo>();
            UserData = userData;
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;
            // when the user wants to check out his own treasure
            MessengerInstance.Register<Treasure>(this, "ShowTreasure",treasr =>
            {
                try
                {
                    WebBrowser.ExecuteScriptAsync("removeMarkers", "removed");
                    WebBrowser.ExecuteScriptAsync("showTreasures", treasr.MarkerInfo.Latitude, treasr.MarkerInfo.Longtitude,
                                treasr.ID, treasr.Name, treasr.TreasureType.ToString(),
                                treasr.TreasureSize.ToString(), treasr.Description,
                                treasr.Rating, treasr.IsChained.ToString(), true);
                }
                catch(Exception e)
                {
                    MessageBox.Show("Wait for the browser to finish loading");
                }

            });
        }
        public UserDataService UserData { get; set; }
        private TreasureType selectedTreasureType= TreasureType.ANY;
        private TreasureSizes selectedTreasureSize= Enums.TreasureSizes.ANY;
        
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

        public const string RatingPropertyName = "Rating";

        private int rating=3;
        
        public int Rating
        {
            get
            {
                return rating;
            }

            set
            {
                if (rating == value)
                {
                    return;
                }

                rating = value;
                RaisePropertyChanged(RatingPropertyName);
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
        /// This property is set when the user wants to find chained treasures.
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
        /// This property is when the user wants to see treasures he's found.
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
        /// </summary>
        public Location CurrentLocation
        {
            get
            {
                if (currentLocation == null)
                    currentLocation = new Location(UserData.GetUserHomeAddress());
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
                            List<Treasure> mapTreasures;
                            if (FoundByUser == true)
                                mapTreasures = UnitofWork.Treasures.GetTreasuresAndThoseFoundByUser(UserData.CurrentUser.ID);
                            else mapTreasures= UnitofWork.Treasures.GetTreasuresNotFoundByUser(UserData.CurrentUser.ID);

                            foreach (var treas in mapTreasures)
                            {
                                double rating = UnitofWork.TreasureComments.GetTreasureRating(treas.ID);
                                treas.Rating = rating;
                                UnitofWork.Complete();

                                if(( SelectedTreasureSize == Enums.TreasureSizes.ANY || treas.TreasureSize==SelectedTreasureSize) &&
                                ( SelectedTreasureType == TreasureType.ANY || treas.TreasureType == SelectedTreasureType) &&
                                ( Difficulty==0 || Difficulty>= treas.Difficulty) && treas.IsChained==FindChainedTreasures &&
                                ( Rating==0 || Rating>=Math.Round(rating) ))
                                {
                                    if(treas.MarkerInfo.IsInRadius(CurrentLocation.Lat, CurrentLocation.Lon, Radius))
                                    {
                                        var marker = treas.MarkerInfo;
                                        WebBrowser.ExecuteScriptAsync("showTreasures", marker.Latitude, marker.Longtitude,
                                            treas.ID, treas.Name, treas.TreasureType.ToString(),
                                            treas.TreasureSize.ToString(), treas.Description,
                                            treas.Rating, treas.IsChained.ToString(),false);
                                    }
                                }
                            }
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

        public void startHuntCS(double lat, double lng, string name, int id,string description)
        {
            if (MessageBox.Show("are you sure you wanna search for " +
                   "treasure at coordinates:" + lat.ToString().Substring(0,5) +
                   ": " + lng.ToString().Substring(0, 5) + " " + "\n With name: " + name, "confirmation", MessageBoxButton.YesNo) ==
                   MessageBoxResult.Yes)
            {
                SimpleIoc.Default.GetInstance<UserDataService>().UserLocation = CurrentLocation;
                if (!SimpleIoc.Default.IsRegistered<SearchedTreasureArgs>())
                {
                    SimpleIoc.Default.Register<SearchedTreasureArgs>(() =>
                    {
                        return new SearchedTreasureArgs(new Location(lat, lng), id);
                    });
                }
                else
                {
                    SimpleIoc.Default.GetInstance<SearchedTreasureArgs>().SearchedTreasureID = id;
                    SimpleIoc.Default.GetInstance<SearchedTreasureArgs>().SearchedTreasureLocation = new Location(lat, lng);
                    SimpleIoc.Default.GetInstance<SearchedTreasureArgs>().Name = name;
                    SimpleIoc.Default.GetInstance<SearchedTreasureArgs>().Description = description;
                }
                if(!SimpleIoc.Default.IsRegistered<FindTreasureVM>())
                    SimpleIoc.Default.Register<FindTreasureVM>();
                
                MessengerInstance.Send<Type>(typeof(FindTreasureVM), "ChangePage");
                MessengerInstance.Send<object>(new object(), "Refresh"); //show the comments
            }
        }

        public void getAllTreasuresCS(string address)
        {
            string[] names = address.Split(',');
            if (names.Length >= 2)
            {
                string country = names[names.Length - 1].Trim();
                string city = names[names.Length - 2].Trim();
                WebBrowser.ExecuteScriptAsync("removeMarkers", "removed");
                using (var UnitofWork = new UnitOfWork(new GeocachingContext()))
                {
                    List<Treasure> mapTreasures;
                    if (FoundByUser == true)
                        mapTreasures = UnitofWork.Treasures.GetTreasuresAndThoseFoundByUser(UserData.CurrentUser.ID);
                    else mapTreasures = UnitofWork.Treasures.GetTreasuresNotFoundByUser(UserData.CurrentUser.ID);

                    foreach (var treas in mapTreasures)
                    {
                        
                        if (treas.MarkerInfo.Country.Contains(country, StringComparison.OrdinalIgnoreCase) &&
                            treas.MarkerInfo.City.Contains(city, StringComparison.OrdinalIgnoreCase))
                            Markers.Add(treas.MarkerInfo);
                        
                    }

                    if (Markers.Count != 0)
                        foreach (MarkerInfo marker in Markers)
                        {
                            Treasure treasr = UnitofWork.Treasures.Get(marker.TreasureId);
                            WebBrowser.ExecuteScriptAsync("showTreasures", marker.Latitude, marker.Longtitude,
                            treasr.ID, treasr.Name, treasr.TreasureType.ToString(),
                            treasr.TreasureSize.ToString(), treasr.Description,
                            treasr.Rating, treasr.IsChained.ToString(),false);
                        }
                    Markers.Clear();
                }
            }
        }
        #endregion
    }
}
