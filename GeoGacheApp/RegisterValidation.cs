using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoGacheApp
{
    class RegisterValidation
    {

        private string username;
        private string password;
        private string confirmPass;

        public string errMsg
        {
            get;
            private set;

        }
        public delegate void ActionOnError(string errorMsg);
        private ActionOnError act;

        public RegisterValidation(
            string username,
            string password,
            string confirmPassword,
            ActionOnError act
            )
        {
            this.username = username;
            this.password = password;
            this.confirmPass = confirmPassword;
            this.act = act;
        }

        public bool ValidateRegisterData()
        {
            Boolean emptyUsername;
            emptyUsername = username.Equals(String.Empty);
            if (emptyUsername)
            {
                errMsg = "username is empty";
                act(errMsg);

                return false;
            }
            if (username.Length < 5 || password.Length < 5)
            {
                errMsg = "username or password is too short";
                act(errMsg);

                return false;
            }
            Boolean emptyPassword;
            emptyPassword = password.Equals(String.Empty);
            if (emptyPassword)
            {
                errMsg = "no password";
                act(errMsg);

                return false;
            }
            Boolean matchingPassword;
            matchingPassword = password.Equals(confirmPass);
            if (!matchingPassword)
            {
                errMsg = "passwords dont match";
                    act(errMsg);
                return false;
            }
            UserContext context = new UserContext();
            User usr = (from user in context.Users where user.Username == username select user).FirstOrDefault();
            if (usr!=null)
            {
                errMsg = "username taken";
                act(errMsg);
                return false;
            }
            return true;
        }

    }
}
