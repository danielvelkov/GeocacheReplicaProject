﻿using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache
{
    class RegisterValidation
    {

        private string username;
        private string password;
        private string confirmPass;
        private string firstName;
        private string lastName;
        private string country;
        private string city;
        private string address;
        const int PASSWORDMAXCHAR = 11;
        const int PASSWORDMINCHAR = 8;

        const int USERNAMEMAXCHAR = 14;
        const int USERNAMEMINCHAR = 8;


        public string errMsg
        {
            get;
            private set;

        }
        public delegate void ActionOnError(string errorMsg);
        private ActionOnError act;

        public RegisterValidation(
            User user,
            ActionOnError act,
            string Password,
            string ConfirmPassword)
        {
            this.username = user.Username;
            this.password = user.Password;
            this.firstName = user.FirstName;
            this.lastName = user.LastName;
            this.country = user.Country;
            this.city = user.City;
            this.address = user.Adress;
            this.act = act;
            this.password = Password;
            this.confirmPass = ConfirmPassword;

        }

        public bool ValidateRegisterData()
        {
            
            if (String.IsNullOrWhiteSpace(username))
            {
                errMsg = "*Username is empty.";
                act(errMsg);

                return false;

            }
            if (String.IsNullOrWhiteSpace(password))
            {
                errMsg = "*No password entered.";
                act(errMsg);

                return false;
            }
            if (username.Length < USERNAMEMINCHAR || password.Length < PASSWORDMINCHAR)
            {
                errMsg = "*Username or password is too short.";
                act(errMsg);

                return false;
            }
            if (username.Length > USERNAMEMAXCHAR || password.Length > PASSWORDMAXCHAR)
            {
                errMsg = "*Username or password is too long.";
                act(errMsg);

                return false;
            }
            Boolean matchingPassword;
            matchingPassword = password.Equals(confirmPass);
            if (!matchingPassword)
            {
                errMsg = "*Passwords don't match.";
                    act(errMsg);
                return false;
            }
            
            if (String.IsNullOrWhiteSpace(firstName) || String.IsNullOrWhiteSpace(lastName))
            {
                errMsg = "*First or last name empty.";
                act(errMsg);
                return false;
            }
            if(String.IsNullOrWhiteSpace(country) || String.IsNullOrWhiteSpace(city))
            {
                errMsg = "*City and country fields are required.";
                act(errMsg);
                return false;
            }
            return true;
        }

    }
}
