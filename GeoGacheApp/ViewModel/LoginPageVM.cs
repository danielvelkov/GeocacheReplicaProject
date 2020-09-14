using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Geocache;
using Geocache.Database;
using Geocache.Helper;
using Geocache.Interfaces;
using Geocache.Models;
using Geocache.ViewModel.BrowserVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private bool isLoading = false;
        private bool isButtonEnabled = true;
        public const string UsernamePropertyName = "Username";

        private string username = "";

        public const string ErrorMsgPropertyName = "ErrorMsg";

        private string errorMsg = "";
        #endregion

        #region Parameters


        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        public bool IsButtonEnabled
        {
            get
            {
                return isButtonEnabled;
            }
            set
            {
                isButtonEnabled = value;
                RaisePropertyChanged("IsButtonEnabled");
            }
        }

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

        IAsyncCommand loginCommand;
        ICommand registerCommand;

        public IAsyncCommand Login
        {
            get
            {
                return loginCommand ?? (loginCommand =
                  new AsyncCommand(LoginAsync, CanExecuteLogin));
            }
        }

        private bool CanExecuteLogin()
        {
            return !IsLoading;
        }

        public ICommand Register
        {
            get
            {
                return registerCommand ?? (registerCommand =
                 new RelayCommand<Object>(x =>
                 {
                     
                     MessengerInstance.Send<Type>(typeof(RegisterPageVM), "ChangePage");
                 }

                ));
            }
        }

        private async Task LoginAsync()
        {
            IsButtonEnabled = false;
            IsLoading = true;
            await Task.Run(() => 
            {
                using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                {
                    if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                    {
                        
                        User user = unitOfWork.Users.ValidateLogin(Username, Password);
                        
                        if (user != null)
                        {
                            if (user.isBanned)
                            {
                                ErrorMsg = "*BANNED USER";
                            }
                            //login the user
                            if (!SimpleIoc.Default.IsRegistered<UserDataService>())
                                SimpleIoc.Default.Register<UserDataService>(() => { return new UserDataService { CurrentUser = user }; });
                            else
                                SimpleIoc.Default.GetInstance<UserDataService>().CurrentUser = user;
                            // if we've logged out we need to create the instances again
                            
                            if (!SimpleIoc.Default.IsRegistered<UserPageVM>())
                                ViewModelLocator.ReRegisterInstances();
                            Password = ""; //clear password so they cant enter :p
                            MessengerInstance.Send<Type>(typeof(HomePageVM), "ChangePage"); //change to homepage 
                        }
                        else
                            ErrorMsg = "*Password is wrong or no such user exists.";       
                    }
                    else
                    {
                        ErrorMsg = "*Password or Username is empty.";
                    }
                }
                IsLoading = false;
                IsButtonEnabled = true;
            });
            
        }

        #endregion
    }
}
