using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Geocache.Database;
using Geocache.Helper;
using Geocache.Models;
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

        private User currentUser;
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
                    currentUser = UserData.GetUser();
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

                          if (user != null)
                          {
                              user.FirstName = CurrentUser.FirstName;
                              user.LastName = CurrentUser.LastName;
                              user.Adress = CurrentUser.Adress;
                              user.City = CurrentUser.City;
                              user.Country = CurrentUser.Country;
                              unitOfWork.Complete();
                              MessageBox.Show("Changes to account made", "saved", MessageBoxButton.OK);

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
                      MessengerInstance.Send<ViewModelBase>(ViewModelLocator.HomePageVM, "ChangePage");
                  })
                ));

            }
        }

        #endregion
    }
}
