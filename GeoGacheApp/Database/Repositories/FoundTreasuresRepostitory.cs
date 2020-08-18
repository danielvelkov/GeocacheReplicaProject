using Geocache.Interfaces;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Database.Repositories
{
    public class FoundTreasuresRepostitory:Repository<Found_Treasures>,IFoundTreasuresRepository
    {
        public FoundTreasuresRepostitory(GeocachingContext context) : base(context)
        {
        }

        public GeocachingContext FoundTreasuresContext
        {
            get { return Context as GeocachingContext; }
        }

        public Treasure GetNextTreasure(int TreasureId)
        {
            Chained_Treasures treasure = FoundTreasuresContext.Chained_Treasures.SingleOrDefault(t => t.Treasure1_ID == TreasureId);
            if (treasure.Treasure_2 != null)
                return treasure.Treasure_2;
            else return null;
        }

        public int GetUserFoundTreasuresCount(int UserId)
        {
            int count = 0;
            if (FoundTreasuresContext.Found_Treasures.Where(ft => ft.UserID == UserId) != null)
                count = FoundTreasuresContext.Found_Treasures.Where(ft => ft.UserID == UserId).Count();
            return count;
        }

        public int GetUserPoints(int UserId)
        {
            int userPoints = 0;
            foreach(Found_Treasures treas in FoundTreasuresContext.Found_Treasures.Where(x => x.UserID == UserId)){
                userPoints += treas.Points;
            }
            return userPoints;
        }

        public bool HasUserFoundTreasure(int UserId, int TreasureId)
        {
            return FoundTreasuresContext.Found_Treasures.Any(t => t.TreasureID == TreasureId && t.UserID == UserId);
        }
    }
}
