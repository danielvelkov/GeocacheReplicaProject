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
        Location userLocation;

        public Location UserLocation { get => userLocation; set => userLocation = value; }
        public User CurrentUser { get => currentUser; set => currentUser = value; }
        
        public UserDataService(){}

        public string GetUserHomeAddress()
        {
            StringBuilder address = new StringBuilder();
            if(!string.IsNullOrEmpty(CurrentUser.Adress))
            address.Append(CurrentUser.Adress + ",");
            if (!string.IsNullOrEmpty(CurrentUser.City))
                address.Append(CurrentUser.City + ",");
            if (!string.IsNullOrEmpty(CurrentUser.Country))
                address.Append(CurrentUser.Country);
            else address.Length--;
            return address.ToString();
        }
        public List<Treasure> GetUserTreasures()
        {
            userTreasures = new List<Treasure>();
            using (var UnitofWork = new UnitOfWork(new GeocachingContext()))
            {
                userTreasures = UnitofWork.Treasures.GetUserTreasures(CurrentUser.ID);
            }
            return userTreasures;
        }

        public List<Treasure> GetUnchainedUserTreasures()
        {
            userTreasures = new List<Treasure>();
            using (var UnitofWork = new UnitOfWork(new GeocachingContext()))
            {
                userTreasures = UnitofWork.Treasures.GetUserTreasuresNotChained(CurrentUser.ID);
            }
            return userTreasures;
        }
        /// <summary>
        /// Get all the unchained treasures except the ones the param is connected to
        /// </summary>
        /// <param name="Tresure to change"></param>
        /// <returns></returns>
        public List<Treasure> GetUnchainedUserTreasures(int TreasureId)
        {
            userTreasures = new List<Treasure>();
            using (var UnitofWork = new UnitOfWork(new GeocachingContext()))
            {
                userTreasures = UnitofWork.Treasures.GetUserTreasuresNotChained(CurrentUser.ID);

            }
            return userTreasures.Where(t=>t.ID!=TreasureId).ToList();
        }

    }
}
