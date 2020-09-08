using Geocache;
using Geocache.Interfaces;
using Geocache.Models;
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

        public List<Treasure> GetTreasuresNotFoundByUser(int UserID)
        {
            //this gets treasures that the user hasnt found and that arent his
            return TreasureContext.Treasures.Where(
                t => t.UserId != UserID &&
                !TreasureContext.Found_Treasures.Any(ft=>(ft.TreasureID==t.ID) && ft.UserID==UserID )).ToList();
        }
        public List<Treasure> GetTreasuresAndThoseFoundByUser(int UserID)
        {
            return TreasureContext.Treasures.Where(
                t => t.UserId != UserID ).ToList();
        }
        public List<Treasure> GetUserTreasures(int UserID)
        {
            //include ensures we dont dispose lazyloaded property after entity leaves "using" scope 
            return TreasureContext.Treasures.Include(x=>x.MarkerInfo).Include(y=>y.Treasures_Comments)
                .Include(y=>y.Chained_Treasure1).Include(y=>y.Chained_Treasure2).Where(t => t.UserId == UserID).ToList();
        }
        public IEnumerable<Treasure> GetUserTreasuresNotChained(int UserID)
        {
            return TreasureContext.Treasures.Where(t => (t.UserId == UserID) && t.IsChained == false);
        }
        public MarkerInfo GetTreasureInfo(int TreasureId)
        {
            return TreasureContext.MarkerInfos.SingleOrDefault(t => t.TreasureId == TreasureId);
        }

        public int GetUserHiddenTreasuresCount(int UserId)
        {
            int count = 0;
            IQueryable<Treasure> treasures;
            if ((treasures=TreasureContext.Treasures.Where(t => t.UserId == UserId)) != null)
                count = treasures.Count();
            return count;
        }
        public IEnumerable<Treasure> GetTreasuresAndComments()
        {
            return TreasureContext.Treasures.Include(x => x.Treasures_Comments).Include(x=>x.Chained_Treasure1)
                .Include(x=>x.Chained_Treasure2).Where(x => x.UserId!=0);
        }
    }
}
