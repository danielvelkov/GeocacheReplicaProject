using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoGacheApp
{
    public class UserData
    {
        static private List<User> _Users = new List<User>();

        static public List<User> TestUsers
        {
            get
            {
                UserContext contx = new UserContext();
                _Users = contx.Users.ToList();
                return _Users;
            }
            set { }
        }

        public static User isUserPassCorrect(string username, string password)
        {
            
                User testuser = (from user in TestUsers
                                 where user.Username.Equals(username) &&
     user.Password.Equals(password)
                                 select user).FirstOrDefault();

                if (testuser != null)
                {
                    return testuser;
                }
                return null;
            
            
           
        }
    }
}
