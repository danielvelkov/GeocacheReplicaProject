using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Geocache.Enums;
using Geocache.Helper;
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
                return "Welcome " + UserData.GetUser().FirstName;
            }
        }



        #endregion
        private ICommand logOut;
        private ICommand goToUserPage;

        public ICommand LogOut
        {
            get
            {
                if (logOut == null)
                    logOut = new RelayCommand(() =>
                      {
                          SimpleIoc.Default.Unregister<UserDataService>();
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
                         MessengerInstance.Send<ViewModelBase>(ViewModelLocator.UserPageVM, "ChangePage");
                     });
                return goToUserPage;
            }
        }
        #region Commands

        #endregion

    }
}
