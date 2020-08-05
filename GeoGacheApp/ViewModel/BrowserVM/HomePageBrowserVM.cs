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

        private string addressEditable;
        public string AddressEditable
        {
            get { return addressEditable; }
            set { Set(ref addressEditable, value); }
        }

        private string outputMessage;
        public string OutputMessage
        {
            get { return outputMessage; }
            set { Set(ref outputMessage, value); }
        }

        private string statusMessage;
        public string StatusMessage
        {
            get { return statusMessage; }
            set { Set(ref statusMessage, value); }
        }
        

        private ChromiumWebBrowser webBrowser;
        /// <summary>
            /// The <see cref="WebBrowser" /> property's name.
            /// </summary>
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
                    webBrowser.Address="localfolder://cefsharp/";

                RaisePropertyChanged(WebBrowserPropertyName);
            }
        }

        private object evaluateJavaScriptResult;
        public object EvaluateJavaScriptResult
        {
            get { return evaluateJavaScriptResult; }
            set { Set(ref evaluateJavaScriptResult, value); }
        }

        private bool showSidebar;
        public bool ShowSidebar
        {
            get { return showSidebar; }
            set { Set(ref showSidebar, value); }
        }

        private bool showDownloadInfo;
        public bool ShowDownloadInfo
        {
            get { return showDownloadInfo; }
            set { Set(ref showDownloadInfo, value); }
        }

        private string lastDownloadAction;
        public string LastDownloadAction
        {
            get { return lastDownloadAction; }
            set { Set(ref lastDownloadAction, value); }
        }

        private DownloadItem downloadItem;
        public DownloadItem DownloadItem
        {
            get { return downloadItem; }
            set { Set(ref downloadItem, value); }
        }

        private bool legacyBindingEnabled;
        

        public bool LegacyBindingEnabled
        {
            get { return legacyBindingEnabled; }
            set { Set(ref legacyBindingEnabled, value); }
        }

        public ICommand GoCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        public ICommand ExecuteJavaScriptCommand { get; private set; }
        public ICommand EvaluateJavaScriptCommand { get; private set; }
        public ICommand ShowDevToolsCommand { get; private set; }
        public ICommand CloseDevToolsCommand { get; private set; }
        public ICommand JavascriptBindingStressTest { get; private set; }

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
                                if((treas.TreasureSize==Enums.TreasureSizes.ANY || treas.TreasureSize==SelectedTreasureSize) &&
                                (treas.TreasureType==TreasureType.ANY || treas.TreasureType==SelectedTreasureType))
                                Markers.Add(treas.MarkerInfo);
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
        //public BrowserTabViewModel(string address)
        //{
        //    Address = address;
        //    AddressEditable = Address;

        //    GoCommand = new RelayCommand(Go, () => !String.IsNullOrWhiteSpace(Address));
        //    HomeCommand = new RelayCommand(() => AddressEditable = Address = CefExample.DefaultUrl);
        //    ExecuteJavaScriptCommand = new RelayCommand<string>(ExecuteJavaScript, s => !String.IsNullOrWhiteSpace(s));
        //    EvaluateJavaScriptCommand = new RelayCommand<string>(EvaluateJavaScript, s => !String.IsNullOrWhiteSpace(s));
        //    ShowDevToolsCommand = new RelayCommand(() => webBrowser.ShowDevTools());
        //    CloseDevToolsCommand = new RelayCommand(() => webBrowser.CloseDevTools());
        //    JavascriptBindingStressTest = new RelayCommand(() =>
        //    {
        //        WebBrowser.Load(CefExample.BindingTestUrl);
        //        WebBrowser.LoadingStateChanged += (e, args) =>
        //        {
        //            if (args.IsLoading == false)
        //            {
        //                Task.Delay(10000).ContinueWith(t =>
        //                {
        //                    if (WebBrowser != null)
        //                    {
        //                        WebBrowser.Reload();
        //                    }
        //                });
        //            }
        //        };
        //    });

        //    PropertyChanged += OnPropertyChanged;

        //    var version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
        //    OutputMessage = version;
        //}

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

        //private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    switch (e.PropertyName)
        //    {
        //        case "Address":
        //            AddressEditable = Address;
        //            break;

        //        case "WebBrowser":
        //            if (WebBrowser != null)
        //            {
        //                WebBrowser.ConsoleMessage += OnWebBrowserConsoleMessage;
        //                WebBrowser.StatusMessage += OnWebBrowserStatusMessage;

        //                // TODO: This is a bit of a hack. It would be nicer/cleaner to give the webBrowser focus in the Go()
        //                // TODO: method, but it seems like "something" gets messed up (= doesn't work correctly) if we give it
        //                // TODO: focus "too early" in the loading process...
        //                WebBrowser.FrameLoadEnd += (s, args) =>
        //                {
        //                    //Sender is the ChromiumWebBrowser object 
        //                    var browser = s as ChromiumWebBrowser;
        //                    if (browser != null && !browser.IsDisposed)
        //                    {
        //                        browser.Dispatcher.BeginInvoke((Action)(() => browser.Focus()));
        //                    }
        //                };
        //            }

        //            break;
        //    }
        //}

        private void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            OutputMessage = e.Message;
        }

        private void OnWebBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
            StatusMessage = e.Value;
        }

        private void Go()
        {
            Address = AddressEditable;

            // Part of the Focus hack further described in the OnPropertyChanged() method...
            Keyboard.ClearFocus();
        }

        //public void LoadCustomRequestExample()
        //{
        //    var postData = System.Text.Encoding.Default.GetBytes("test=123&data=456");

        //    WebBrowser.LoadUrlWithPostData("https://cefsharp.com/PostDataTest.html", postData);
        //}
    }
}
