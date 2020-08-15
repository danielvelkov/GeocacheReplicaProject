﻿using CefSharp;
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

namespace Geocache.ViewModel
{
    public class HideTreasurePageVM : ViewModelBase
    {
        public HideTreasurePageVM(UserDataService userdata)
        {
            UserData = userdata;
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;
            Treasure = new Treasure();
            MarkerInfo = new MarkerInfo();
            TreasureChain = new Treasure();
        }

        #region fields
        private TreasureType selectedTreasureType = TreasureType.NORMAL;
        private TreasureSizes selectedTreasureSize = Enums.TreasureSizes.MEDIUM;
        private Treasure treasure;
        private MarkerInfo markerInfo;
        private Treasure treasureChain;
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
                return UserData.GetUnchainedUserTreasures();
            }
        }

        public Treasure TreasureChain
        {
            get
            {
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

        /// <summary>
        /// Sets and gets the CurrentUser property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
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
                        this, true,BindingOptions.DefaultBinder);
                }
                RaisePropertyChanged(WebBrowserPropertyName);
            }
        }
        #endregion

        #region Commands
        private ICommand goBack;
        public ICommand GoBack
        {
            get
            {
                return goBack ?? (goBack =
                  new RelayCommand((() =>
                  {
                      SimpleIoc.Default.Unregister<HideTreasurePageVM>();
                      MessengerInstance.Send<ViewModelBase>(ViewModelLocator.HomePageVM, "ChangePage");
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
                        if(!Double.IsNaN(Latitude) ||
                        !Double.IsNaN(Longtitude))
                        {
                            using(var UnitOfWork= new UnitOfWork(new GeocachingContext()))
                            {
                                Treasure.Key = Treasure.GenerateKey();
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

                                if (Treasure.IsChained && TreasureChain.ID!=0)
                                {
                                    UnitOfWork.ChainedTreasures.Add(
                                        new Chained_Treasures(TreasureChain.ID, Treasure.ID));
                                    UnitOfWork.Complete();
                                    // set the previous treasure to chained=>meaning it cant be chained to anymore
                                    Treasure updatedTreasure = UnitOfWork.Treasures.Get(TreasureChain.ID);
                                    updatedTreasure.IsChained = true;
                                    UnitOfWork.Complete();
                                }
                                // always set the new treasure to ischained=false so you can chain it
                                Treasure updatedTreasure2 = UnitOfWork.Treasures.Get(Treasure.ID);
                                updatedTreasure2.IsChained = false;
                                UnitOfWork.Complete();
                                
                                MessageBoxResult result=MessageBox.Show("Treasure added succesfully", "Treasure added", MessageBoxButton.OK);
                                if (result == MessageBoxResult.OK)
                                    MessageBox.Show(string.Format("The key:{0} was generated.if you have your own key for the treasure go to MyTreasures and change it!", Treasure.Key), "Key generated", MessageBoxButton.OK);
                                GoBack.Execute(null);
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
