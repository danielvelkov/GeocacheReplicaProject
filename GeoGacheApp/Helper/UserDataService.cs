using Geocache.Database;
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
        List<Treasure> userTreasures;
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
        public List<Treasure> GetUserTreasures()
        {
            userTreasures = new List<Treasure>();
            using(var UnitofWork= new UnitOfWork(new GeocachingContext()))
            {
               userTreasures= UnitofWork.Treasures.GetUserTreasures(currentUser.ID);
            }
            return userTreasures;
        }

        public List<Treasure> GetUnchainedUserTreasures()
        {
            userTreasures = new List<Treasure>();
            using (var UnitofWork = new UnitOfWork(new GeocachingContext()))
            {
                userTreasures = UnitofWork.Treasures.GetUserTreasuresNotChained(currentUser.ID);
            }
            return userTreasures;
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
