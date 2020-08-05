using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Geocache;
using Geocache.Database;
using Geocache.Helper;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Geocache.ViewModel
{
    public class LoginPageVM : ViewModelBase
    {
        public LoginPageVM()
        {

        }

        #region fields
        ICommand loginCommand;
        ICommand registerCommand;
        #endregion

        #region Parameters

        
        public const string UsernamePropertyName = "Username";

        private string username = "";

        /// <summary>
        /// Sets and gets the Username property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                if (username == value)
                {
                    return;
                }

                username = value;
                RaisePropertyChanged(UsernamePropertyName);
            }
        }
       
        public const string ErrorMsgPropertyName = "ErrorMsg";

        private string errorMsg = "";

        /// <summary>
        /// Sets and gets the ErrorMsg property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                return errorMsg;
            }

            set
            {
                if (errorMsg == value)
                {
                    return;
                }

                errorMsg = value;
                RaisePropertyChanged(ErrorMsgPropertyName);
            }
        }

        // you get the password at the password changed event. SEE  the codebehind-> LoginPageView.xaml.cs 
        public string Password { private get; set; }
        #endregion

        #region commands
        public ICommand Login
        {
            get
            {
                return loginCommand ?? (loginCommand =
                  new RelayCommand<Object>((x =>
                  {
                      using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                      {
                          if(!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                          {
                              User user = unitOfWork.Users.ValidateLogin(Username, Password);

                              if (user != null)
                              {
                                  //login the user
                                  SimpleIoc.Default.Register<UserDataService>(() => { return new UserDataService(user); });
                                  //change to homepage
                                  MessengerInstance.Send<ViewModelBase>(ViewModelLocator.HomePageVM, "ChangePage");

                              }
                              else
                              ErrorMsg = "Password is wrong or no such user exists";
                              return;
                          }
                          else
                          {
                              ErrorMsg = " Password or Username is empty";
                          }
                          
                      }
                  })
                ));
            }
        }

        public ICommand Register
        {
            get
            {
                return registerCommand ?? (registerCommand =
                 new RelayCommand<Object>(x =>
                 {
                     MessengerInstance.Send<ViewModelBase>(ViewModelLocator.RegisterPageVM, "ChangePage");
                 }

                ));
            }
        }

        #endregion
    }
}
