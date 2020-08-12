using Geocache;
using Geocache.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Database.Repositories
{
    public class TreasureRepository : Repository<Treasure>, ITreasureRepository
    {
        public TreasureRepository(GeocachingContext context) : base(context)
        {
        }

        public GeocachingContext TreasureContext
        {
            get { return Context as GeocachingContext; }
        }

        public List<Treasure> GetOthersTreasures(int UserID)
        {
            return TreasureContext.Treasures.Where(t => t.UserId != UserID).ToList();
        }
        public List<Treasure> GetUserTreasures(int UserID)
        {
            return TreasureContext.Treasures.Where(t => t.UserId == UserID).ToList();
        }
        public List<Treasure> GetUserTreasuresNotChained(int UserID)
        {
            return TreasureContext.Treasures.Where(t => (t.UserId == UserID) && t.IsChained == false).ToList();
        }
    }
}
