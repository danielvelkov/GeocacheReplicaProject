using Geocache.Interfaces;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Database.Repositories
{
    public class ChainedTreasureRepository: Repository<Chained_Treasures>, IChainedTreasureRepository
    {
        public ChainedTreasureRepository(GeocachingContext context) : base(context)
        {
        }

        public GeocachingContext ChainedTreasureContext
        {
            get { return Context as GeocachingContext; }
        }
    }
}
