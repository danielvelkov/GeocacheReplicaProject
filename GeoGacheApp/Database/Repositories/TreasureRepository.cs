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

        public List<Treasure> GetOthersTreasures(int UserID)
        {
            //this gets treasures that the user hasnt found and that arent his
            // add a way to get the first one in the chained treasure
            return TreasureContext.Treasures.Where(
                t => t.UserId != UserID &&
                !TreasureContext.Found_Treasures.Any(ft=>(ft.TreasureID==t.ID) && ft.UserID==UserID )).ToList();
        }
        public List<Treasure> GetUserTreasures(int UserID)
        {
            //include ensures we dont make it a lazy loaded proprety which is disposed if its not inside using unitOfWork
            return TreasureContext.Treasures.Include(x=>x.MarkerInfo).Include(y=>y.Treasures_Comments).Where(t => t.UserId == UserID).ToList();
        }
        public List<Treasure> GetUserTreasuresNotChained(int UserID)
        {
            return TreasureContext.Treasures.Where(t => (t.UserId == UserID) && t.IsChained == false).ToList();
        }
        public MarkerInfo GetTreasureInfo(int TreasureId)
        {
            return TreasureContext.MarkerInfos.SingleOrDefault(t => t.TreasureId == TreasureId);
        }
    }
}
