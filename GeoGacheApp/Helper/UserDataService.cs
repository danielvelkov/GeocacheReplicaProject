using Geocache.Interfaces;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Helper
{
    public class UserDataService:IDataService
    {
        User currentUser;
        //ICollection<Treasure> userTreasures;
        //ICollection<Treasures_Comments> userComments;

        public UserDataService(User LoggedUser)
        {
            SetUser(LoggedUser);
        }

        public User GetUser()
        {
            return currentUser;
        }

        public void SetUser(User user)
        {
            currentUser = user;
        }
        public string GetUserAddress()
        {
            StringBuilder address = new StringBuilder();
            address.Append(currentUser.Adress + ", ");
            address.Append(currentUser.City + ", ");
            address.Append(currentUser.Country);
            return address.ToString();
        }
    }
}
