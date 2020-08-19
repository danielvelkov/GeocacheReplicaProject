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

        public Treasure GetNextChainedTreasure(int TreasureId)
        {
            Treasure treasure;
            if(ChainedTreasureContext.Chained_Treasures.Include("Treasure_2").Where
                (tc => tc.Treasure1_ID == TreasureId).Count()!= 0){
                return treasure = ChainedTreasureContext.Chained_Treasures.Include("Treasure_2").First
                (tc => tc.Treasure1_ID == TreasureId).Treasure_2;
            }
            return null;
        }

        public void UnchainTreasure(int CtId)
        {
            Treasure treasure;
            if ((treasure = ChainedTreasureContext.Treasures.SingleOrDefault(t => t.ID == CtId)) != null)
            {
                treasure.IsChained = false;
                ChainedTreasureContext.SaveChanges();
            }

        }
    }
}
