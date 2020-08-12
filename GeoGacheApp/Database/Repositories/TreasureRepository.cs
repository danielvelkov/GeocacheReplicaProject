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

        public List<Treasure> GetTreasures(int UserID)
        {
            return TreasureContext.Treasures.Where(t => t.UserId != UserID).ToList();
        }
    }
}
