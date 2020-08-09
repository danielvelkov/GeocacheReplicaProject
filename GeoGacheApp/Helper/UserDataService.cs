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
            if(!string.IsNullOrEmpty(currentUser.Adress))
            address.Append(currentUser.Adress + ",");
            if (!string.IsNullOrEmpty(currentUser.City))
                address.Append(currentUser.City + ",");
            if (!string.IsNullOrEmpty(currentUser.Country))
                address.Append(currentUser.Country);
            else address.Length--;
            return address.ToString();
        }
    }
}
