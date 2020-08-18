using CefSharp;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Geocache.Enums;
using Geocache.Helper;
using Geocache.ViewModel.BrowserVM;
using Geocache.ViewModel.PopUpVM;
using Geocache.Views.PopUpViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Geocache.ViewModel
{
    public class HomePageVM : ViewModelBase
    {
        public HomePageVM(UserDataService userData,PopUpWindowController popUp)
        {
            PopUp = popUp;
            UserData = userData;
        }
        
        #region fields

        #endregion

        #region Parameters
        public UserDataService UserData { get; }
        public PopUpWindowController PopUp { get; private set; }
        //welcome message
        public string Welcome
        {
            get
            {
                return "Welcome " + UserData.CurrentUser.FirstName;
            }
        }
        public const string UserRolePropertyName = "UserRole";

        private UserRoles userRole;

        /// <summary>
        /// Sets and gets the UserRole property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public UserRoles UserRole
        {
            get
            {
                if (userRole == 0)
                    userRole=UserData.CurrentUser.Role;
                return userRole;
            }

            set
            {
                if (userRole == value)
                {
                    return;
                }

                userRole = value;
                RaisePropertyChanged(UserRolePropertyName);
            }
        }

        #endregion

        #region Commands
        private ICommand logOut;
        private ICommand goToUserPage;
        private ICommand hideTreasure;
        private ICommand showUserTreasures;
        private ICommand showLeaderBoards;
        private ICommand changeUserRole;

        public ICommand LogOut
        {
            get
            {
                if (logOut == null)
                    logOut = new RelayCommand(() =>
                    {
                        //remove the user specific instances of pages
                        SimpleIoc.Default.Unregister<HomePageVM>();
                        SimpleIoc.Default.GetInstance<UserDataService>().CurrentUser = null;
                        
                        //ViewModelLocator.Cleanup();
                        // change to login page
                        MessengerInstance.Send<Type>(typeof(LoginPageVM), "ChangePage");
                    });
                return logOut;
            }
        }

        public ICommand GoToUserPage
        {
            get
            {
                if (goToUserPage == null)
                    goToUserPage = new RelayCommand(() =>
                    {
                        MessengerInstance.Send<Type>(typeof(UserPageVM), "ChangePage");
                    });
                return goToUserPage;
            }
        }

        public ICommand HideTreasure
        {
            get
            {
                if (hideTreasure == null)
                    hideTreasure = new RelayCommand(() =>
                    {
                        if(!SimpleIoc.Default.IsRegistered<HideTreasurePageVM>())
                        SimpleIoc.Default.Register<HideTreasurePageVM>();
                        MessengerInstance.Send<Type>(typeof(HideTreasurePageVM), "ChangePage");
                    });
                return hideTreasure;
            }
        }

        public ICommand ShowLeaderBoards
        {
            get
            {
                if (showLeaderBoards == null)
                    showLeaderBoards = new RelayCommand(() =>
                     {
                         if (!SimpleIoc.Default.IsRegistered<LeaderboardVM>())
                             SimpleIoc.Default.Register<LeaderboardVM>();
                         PopUp.ShowPopUp(new LeaderboardView());
                     });
                return showLeaderBoards;
            }
        }
        public ICommand ShowUserTreasures
        {
            get
            {
                if (showUserTreasures == null)
                    showUserTreasures = new RelayCommand(() =>
                    {
                        if (!SimpleIoc.Default.IsRegistered<UserTreasuresVM>())
                            SimpleIoc.Default.Register<UserTreasuresVM>();
                        PopUp.ShowPopUp(new UserTreasuresView());
                    });
                return showUserTreasures;
            }
        }
        public ICommand ChangeUserRole
        {
            get
            {
                if (changeUserRole == null)
                    changeUserRole = new RelayCommand(() =>
                    {
                        if (!SimpleIoc.Default.IsRegistered<UsersRoleVM>())
                            SimpleIoc.Default.Register<UsersRoleVM>();
                        PopUp.ShowPopUp(new UsersRoleView());
                    });
                return changeUserRole;
            }
        }

        #endregion

    }
}
