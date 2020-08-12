using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GeocacheV2.Database;
using GeocacheV2.Helper;
using GeocacheV2.Models;
using GeocacheV2.ViewModel.BrowserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GeocacheV2.ViewModel
{
    public class LoginPageVM : ViewModelBase
    {
        public LoginPageVM()
        {

        }

        #region fields
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

        ICommand loginCommand;
        ICommand registerCommand;

        public ICommand Login
        {
            get
            {
                return loginCommand ?? (loginCommand =
                  new RelayCommand<Object>((x =>
                  {
                      using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                      {
                          //if(!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                          //{
                          //    User user = unitOfWork.Users.ValidateLogin(Username, Password);

                          //    if (user != null)
                          //    {
                          //        //create the instances of the pages connected to the user
                          //        SimpleIoc.Default.Register<HomePageVM>();
                          //        SimpleIoc.Default.Register<HomePageBrowserVM>();
                          //        SimpleIoc.Default.Register<UserPageVM>();

                          //        //login the user
                          //        SimpleIoc.Default.Register<UserDataService>(() => { return new UserDataService(user); });

                          //        //change to homepage
                          //        MessengerInstance.Send<ViewModelBase>(ViewModelLocator.HomePageVM, "ChangePage");

                          //    }
                          //    else
                          //    ErrorMsg = "Password is wrong or no such user exists";
                          //    return;
                          //}
                          //else
                          //{
                          //    ErrorMsg = " Password or Username is empty";
                          //}

                          User user = unitOfWork.Users.ValidateLogin("geochacher2", "meatballs");
                          //        //create the instances of the pages connected to the user
                          SimpleIoc.Default.Register<HomePageVM>();
                          SimpleIoc.Default.Register<HomePageBrowserVM>();
                          //SimpleIoc.Default.Register<UserPageVM>();

                          //login the user
                          SimpleIoc.Default.Register<UserDataService>(() => { return new UserDataService(user); });

                          //change to homepage
                          MessengerInstance.Send<ViewModelBase>(ViewModelLocator.HomePageVM, "ChangePage");

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
                     SimpleIoc.Default.Register<RegisterPageVM>();
                     MessengerInstance.Send<ViewModelBase>(ViewModelLocator.RegisterPageVM, "ChangePage");
                 }

                ));
            }
        }

        #endregion
    }
}
