using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Geocache.Database;
using Geocache.Helper;
using Geocache.Models;
using Geocache.ViewModel.BrowserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Geocache.ViewModel
{
    public class UserPageVM : ViewModelBase
    {
        public UserPageVM(UserDataService userData)
        {
            UserData = userData;
        }

        #region Parameters
        private User currentUser;
        private string errorMsg;
        public UserDataService UserData { get; }

        public const string CurrentUserPropertyName = "CurrentUser";

        /// <summary>
        /// Sets and gets the CurrentUser property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public User CurrentUser
        {
            get
            {
                if (currentUser == null)
                    currentUser = UserData.CurrentUser;
                return currentUser;
            }

            set
            {
                if (currentUser == value)
                {
                    return;
                }

                currentUser = value;
                RaisePropertyChanged(CurrentUserPropertyName);
            }
        }
        public string ErrorMsg
        {
            get
            {
                return errorMsg;
            }
            set
            {
                if (errorMsg != value)
                {
                    errorMsg = value;

                    RaisePropertyChanged("ErrorMsg");
                }
            }
        }
        #endregion

        #region commands
        private ICommand saveChanges;
        private ICommand goBack;

        public ICommand SaveChanges
        {
            get
            {
                return saveChanges ?? (saveChanges =
                  new RelayCommand((() =>
                  {
                      using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                      {
                          User user = unitOfWork.Users.Get(CurrentUser.ID);
                            user.FirstName = CurrentUser.FirstName;
                            user.LastName = CurrentUser.LastName;
                            user.Adress = CurrentUser.Adress;
                            user.City = CurrentUser.City;
                            user.Country = CurrentUser.Country;
                            //validate the data
                            RegisterValidation validation = new RegisterValidation(user, SetErrorMsg, user.Password, user.Password);
                            if (validation.ValidateRegisterData())
                            {
                                unitOfWork.Complete();

                                UserData.CurrentUser = (CurrentUser);
                                SimpleIoc.Default.GetInstance<HomePageBrowserVM>().CurrentLocation = new Location(
                                    UserData.GetUserHomeAddress());
                                MessageBoxResult result = MessageBox.Show("Changes to account made", "saved", MessageBoxButton.OK);
                                if (result == MessageBoxResult.OK)
                                    GoBack.Execute(null);
                            }
                      }

                  })
                ));

            }
        }

        public ICommand GoBack
        {
            get
            {
                return goBack ?? (goBack =
                  new RelayCommand((() =>
                  {
                      MessengerInstance.Send<Type>(typeof(HomePageVM), "ChangePage");
                  })
                ));

            }
        }
        #endregion

        private void SetErrorMsg(string msg)
        {

            ErrorMsg = msg;
        }
    }
}
