using GeocacheV2.Interfaces;
using GeocacheV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeocacheV2.Database.Repositories
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
