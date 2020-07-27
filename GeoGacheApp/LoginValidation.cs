using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoGacheApp
{
    class LoginValidation
    {
        private string username;
        private string password;
        private int currentRole;
        
        public string Username
        {
            get
            {
                return username;
            }
            private set
            {
                username = value;
            }
        }

        public int CurrentRole
        {
            get { return currentRole; }
            set { currentRole = value; }
        }

        //so you can check the error message
        public string errMsg
        {
            get;
            private set;
        }

        public delegate void ActionOnError(string errorMsg);
        private ActionOnError act;


        public LoginValidation(string user, string pass, ActionOnError action)
        {
            username = user;
            password = pass;
            act = action;
        }

        public bool ValidateUserInput(ref User user)
        {

            //test if its empty
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
            user = UserData.isUserPassCorrect(username, password);
            if (user != null)
            {
                Predicate<User> userFinder = (User usr) => { return usr.Username == username; };
                
                //Logger.LogActivity("Login succesful", index);
                currentRole = (int)user.Role;
                return true;
            }
            errMsg = "User not found";
            act(errMsg);

            return false;

        }
    }
}
