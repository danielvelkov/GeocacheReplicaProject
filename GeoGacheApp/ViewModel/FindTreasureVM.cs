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
using Geocache.Models.WrappedModels;
using Geocache.ViewModel.PopUpVM;
using Geocache.Views.PopUpViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Geocache.ViewModel
{
    public class FindTreasureVM:ViewModelBase
    {
        public FindTreasureVM(UserDataService userdata,PopUpWindowController popUp,SearchedTreasureArgs args)
        {
            UserData = userdata;
            TreasureArgs = args;
            PopUp = popUp;
            MessengerInstance.Register<object>(this, "Refresh", obj => { RefreshTreasureComments(); }); //when mod deletes comments
        }

        #region properties

        private SearchedTreasureArgs treasureArgs;
        private ObservableCollection<Treasures_Comments> treasure_Comments;
        
        public ObservableCollection<Treasures_Comments> TreasureComments
        {
            get
            {
                return treasure_Comments;
            }
            set
            {
                treasure_Comments = value;
                RaisePropertyChanged("TreasureComments");
            }
        }

        public UserDataService UserData { get; private set; }

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
        public SearchedTreasureArgs TreasureArgs
        {
            get
            {
                return treasureArgs;
            }
            set
            {
                treasureArgs = value;
            }
        }

        private string commentText;
        public string CommentText
        {
            get
            {
                return commentText;
            }
            set
            {
                if (commentText == value)
                    return;
                commentText = value;
                RaisePropertyChanged("CommentText");
            }
        }
        private int rating;
        public int Rating
        {
            get
            {
                return rating;
            }
            set
            {
                if (rating == value)
                    return;
                rating = value;
                RaisePropertyChanged("Rating");
            }
        }
        #endregion

        #region Commands

        private ICommand showRoute;
        private ICommand goBack;
        private ICommand foundTreasure;
        private ICommand comment;

        public ICommand ShowRoute
        {
            get
            {
                return showRoute ?? (showRoute = new RelayCommand(() => {

                    WebBrowser.ExecuteScriptAsync("setOrigin",
                   UserData.UserLocation.Lat, UserData.UserLocation.Lon);
                    WebBrowser.ExecuteScriptAsync("setDestination",
                        TreasureArgs.SearchedTreasureLocation.Lat,
                        TreasureArgs.SearchedTreasureLocation.Lon);
                    WebBrowser.ExecuteScriptAsync("showRouteMarkers", "OK");
                }));
            }
        }
        public ICommand GoBack
        {
            get
            {
                return goBack ?? (goBack = new RelayCommand(() => {
                    Rating = 0;  CommentText = "";
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
        public ICommand Comment
        {
            get
            {
                return comment ?? (comment = new RelayCommand(() =>
                {
                    if (!string.IsNullOrWhiteSpace(CommentText))
                    {
                        using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                        {
                            if (!unitOfWork.TreasureComments.HasUserCommented(UserData.CurrentUser.ID, TreasureArgs.SearchedTreasureID))
                            {
                                Treasures_Comments tc = new Treasures_Comments(
                                    TreasureArgs.SearchedTreasureID,
                                    UserData.CurrentUser.ID,
                                    CommentText,
                                    DateTime.Now,
                                    CommentType.COMMENT,
                                    Rating);
                                unitOfWork.TreasureComments.Add(tc);
                                unitOfWork.Complete();
                                //TreasureComments.Add(tc);
                                MessengerInstance.Send<object>(new object(), "Refresh");
                            }
                            else
                            {
                                MessageBox.Show("You have already commented.");
                            }
                            CommentText = "";
                        }
                    }

                }));
            }
        }
        #endregion
        private void RefreshTreasureComments()
        {
            using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
            {
                TreasureComments = new ObservableCollection<Treasures_Comments>
                    (unitOfWork.TreasureComments.Find(tc => tc.TreasureID == TreasureArgs.SearchedTreasureID));
                if (unitOfWork.TreasureComments.HasUserCommented(UserData.CurrentUser.ID, TreasureArgs.SearchedTreasureID))
                    Rating = unitOfWork.TreasureComments.GetUserRating(UserData.CurrentUser.ID, TreasureArgs.SearchedTreasureID);
            }
        }
    }
}
