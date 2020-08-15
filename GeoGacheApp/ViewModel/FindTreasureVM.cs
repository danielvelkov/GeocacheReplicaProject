using CefSharp;
using CefSharp.RenderProcess;
using CefSharp.Wpf;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Geocache.Database;
using Geocache.Enums;
using Geocache.Helper;
using Geocache.Models;
using Geocache.ViewModel.PopUpVM;
using Geocache.Views.PopUpViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Geocache.ViewModel
{
    public class FindTreasureVM:ViewModelBase
    {
        public FindTreasureVM(UserDataService userdata,PopUpWindowController popUp,FoundTreasureArgs args)
        {
            UserData = userdata;
            TreasureArgs = args;
            PopUp = popUp;
        }

        #region properties

        private FoundTreasureArgs treasureArgs;
        public UserDataService UserData { get; private set; }
        public FoundTreasureArgs TreasureArgs { get => treasureArgs; set => treasureArgs = value; }
        public PopUpWindowController PopUp { get; private set; }

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
                if (webBrowser != null && webBrowser.Address == null)
                {
                    webBrowser.Address = "localfolder://cefsharp/map_route.html";
                    webBrowser.FrameLoadEnd += (sender, args) =>
                    {
                        //Wait for the MainFrame to finish loading
                        if (args.Frame.IsMain)
                        {
                            ShowRoute.Execute(null);
                        }
                    };
                }
                RaisePropertyChanged(WebBrowserPropertyName);
            }
        }

        private string reportContent;
        public string ReportContent
        {
            get
            {
                return reportContent;
            }
            set
            {
                if (reportContent != value)
                {
                    reportContent = value;
                    RaisePropertyChanged("ReportContent");
                }
            }
        }

        #endregion

        #region Commands

        private ICommand showRoute;
        private ICommand goBack;
        private ICommand foundTreasure;
        private ICommand submitReport;
        public ICommand ShowRoute
        {
            get
            {
                return showRoute ?? (showRoute = new RelayCommand(() => {

                    WebBrowser.ExecuteScriptAsync("setOrigin",
                   UserData.UserLocation.Lat, UserData.UserLocation.Lon);
                    WebBrowser.ExecuteScriptAsync("setDestination",
                        TreasureArgs.FoundTreasureLocation.Lat,
                        TreasureArgs.FoundTreasureLocation.Lon);
                    WebBrowser.ExecuteScriptAsync("initMap1", "ok");
                }));
            }
        }
        public ICommand GoBack
        {
            get
            {
                return goBack ?? (goBack = new RelayCommand(() => {

                    MessengerInstance.Send<Type>(typeof(HomePageVM), "ChangePage");
                }));
            }
        }
        public ICommand FoundTreasure
        {
            get
            {
                return foundTreasure ?? (foundTreasure = new RelayCommand(() =>
                {
                    if(!SimpleIoc.Default.IsRegistered<TreasureFoundVM>())
                    SimpleIoc.Default.Register<TreasureFoundVM>();
                    PopUp.ShowPopUp(new TreasureFoundView());
                    
                }));
            }
        }

        public ICommand SubmitReport
        {
            get
            {
                if (submitReport == null)
                    submitReport = new RelayCommand(() =>
                    {
                        using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                        {
                            var report = new Treasures_Comments(
                                TreasureArgs.FoundTreasureId, UserData.CurrentUser.ID, ReportContent,
                                DateTime.Now, CommentType.REPORT, 0);
                        }

                    });
                return submitReport;
            }
        }
        #endregion

    }
}
