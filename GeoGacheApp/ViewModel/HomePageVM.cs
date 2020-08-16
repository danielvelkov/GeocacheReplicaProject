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
                if (userRole == UserRoles.NONE)
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
        private ICommand findTreasure;
        private ICommand hideTreasure;
        private ICommand showUserTreasures;
        private ICommand showLeaderBoards;

        public ICommand LogOut
        {
            get
            {
                if (logOut == null)
                    logOut = new RelayCommand(() =>
                    {
                        //remove the user specific instances of pages
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

        public ICommand FindTreasure
        {
            get
            {
                if (findTreasure == null)
                    findTreasure = new RelayCommand(() =>
                    {
                        //MessengerInstance.Send<ViewModelBase>(ViewModelLocator.UserPageVM, "ChangePage");
                    });
                return findTreasure;
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
        public ICommand ShowUserTreasures { get => showUserTreasures; set => showUserTreasures = value; }

        #endregion

    }
}
