﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Geocache.Database;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Geocache.ViewModel
{
    public class RegisterPageVM : ViewModelBase
    {
        public RegisterPageVM()
        {
            User = new User();
        }

        #region fields

        private string password;
        private string confirmPassword;

        
        private string errorMsg;
        private User user;
        #endregion

        #region Properties

        public User User
        {
            get
            {
                return user;
            }
            set
            {
                if (user != value)
                {
                    user = value;
                    
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                if (confirmPassword != value)
                {
                    confirmPassword = value;
                    RaisePropertyChanged("ConfirmPassword");
                }
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
        ICommand goToLoginPage;
        ICommand register;

        public ICommand Register
        {
            get
            {
                if (register == null)
                    register = new RelayCommand<Object>(x =>
                    {   // register user
                        var validation = new RegisterValidation(User, SetErrorMsg, Password, ConfirmPassword);
                        if (validation.ValidateRegisterData())
                        {
                            using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                            {
                                if (unitOfWork.Users.DoesUserExist(User.Username))
                                {
                                    SetErrorMsg("User already exists");
                                    return;
                                }

                                User.Password = Password;
                                User.createdAt = DateTime.Now;
                                User.isBanned = false;
                                User.Points = 0;
                                User.Role = Enums.Roles.USER;

                                unitOfWork.Users.Add(User);
                                MessageBox.Show("Registration complete. Congratulations!"
                                , "success", MessageBoxButton.OK);
                                unitOfWork.Complete();
                            }
                            GoToLoginPage.Execute(null);
                        }
                    });
                return register;
            }
        }

        public ICommand GoToLoginPage
        {
            get
            {
                if (goToLoginPage == null)
                    goToLoginPage = new RelayCommand(() =>
                     {
                         Password = "";
                         ConfirmPassword = "";
                         MessengerInstance.Send<Type>(typeof(LoginPageVM), "ChangePage");
                     });
                return goToLoginPage;
            }
        }
        #endregion

        private void SetErrorMsg(string msg)
        {

            ErrorMsg = msg;
        }
    }
}
