using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Geocache.Enums;
using Geocache.Helper;
using Geocache.ViewModel.BrowserVM;
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
        public HomePageVM(UserDataService userData)
        {
            UserData = userData;
        }

        #region fields

        #endregion

        #region Parameters
        public UserDataService UserData { get; }

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
                        SimpleIoc.Default.Unregister<UserDataService>();
                        SimpleIoc.Default.Unregister<HomePageVM>();
                        SimpleIoc.Default.Unregister<UserPageVM>();
                        SimpleIoc.Default.Unregister<HomePageBrowserVM>();
                        // change to login page
                        MessengerInstance.Send<ViewModelBase>(ViewModelLocator.LoginPageVM, "ChangePage");
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
                        SimpleIoc.Default.Register<UserPageVM>();
                        MessengerInstance.Send<ViewModelBase>(ViewModelLocator.UserPageVM, "ChangePage");
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
                        SimpleIoc.Default.Register<HideTreasurePageVM>();
                        MessengerInstance.Send<ViewModelBase>(ViewModelLocator.HideTreasurePageVM, "ChangePage");
                    });
                return hideTreasure;
            }
        }

        public ICommand ShowLeaderBoards { get => showLeaderBoards; set => showLeaderBoards = value; }
        public ICommand ShowUserTreasures { get => showUserTreasures; set => showUserTreasures = value; }

        #endregion

    }
}
