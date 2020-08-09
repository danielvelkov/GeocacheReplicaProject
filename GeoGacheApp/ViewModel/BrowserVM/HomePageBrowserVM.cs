using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.Wpf;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
        
        /// <summary>
        /// Sets and gets the WebBrowser property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
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
                if (webBrowser != null && webBrowser.Address ==null)
                    //webBrowser.Address="localfolder://cefsharp/";

                RaisePropertyChanged(WebBrowserPropertyName);
            }
        }

        private object evaluateJavaScriptResult;
        public object EvaluateJavaScriptResult
        {
            get { return evaluateJavaScriptResult; }
            set { Set(ref evaluateJavaScriptResult, value); }
        }
        

        public HomePageBrowserVM(UserDataService userData)
        {
            Markers = new List<MarkerInfo>();
            UserData = userData;
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

        private double radius;

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
                    currentLocation = new Location(UserData.GetUserAddress());
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
                        using (var UnitofWork = new UnitOfWork(new GeocachingContext()))
                        {
                            foreach (var treas in UnitofWork.Treasures.GetTreasures(UserData.GetUser().ID))
                            {
                                if((SelectedTreasureSize==Enums.TreasureSizes.ANY || treas.TreasureSize==SelectedTreasureSize) &&
                                (SelectedTreasureType==TreasureType.ANY || treas.TreasureType==SelectedTreasureType))
                                {
                                    if(treas.MarkerInfo.IsInRadius(CurrentLocation.Lat, CurrentLocation.Lon,Radius))
                                        Markers.Add(treas.MarkerInfo);
                                }
                            }

                            foreach (MarkerInfo marker in Markers)
                            {
                                Treasure treasr = UnitofWork.Treasures.Get(marker.TreasureId);
                                WebBrowser.ExecuteScriptAsync("showTreasures", marker.Latitude, marker.Longtitude,
                                treasr.ID, treasr.Name, treasr.TreasureType.ToString(),
                                treasr.TreasureSize.ToString(), treasr.Description,
                                treasr.Rating, treasr.isChained.ToString());
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
                          string address = UserData.GetUserAddress();
                          WebBrowser.ExecuteScriptAsync("codeAdress", address);
                      });
                return goToHomeLocation;
            }
        }

        private async void EvaluateJavaScript(string s)
        {
            try
            {
                var response = await webBrowser.EvaluateScriptAsync(s);
                if (response.Success && response.Result is IJavascriptCallback)
                {
                    response = await ((IJavascriptCallback)response.Result).ExecuteAsync("This is a callback from EvaluateJavaScript");
                }

                EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while evaluating Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteJavaScript(string s)
        {
            try
            {
                webBrowser.ExecuteScriptAsync(s);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while executing Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
