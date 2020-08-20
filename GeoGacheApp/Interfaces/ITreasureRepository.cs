using Geocache;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Interfaces
{
    public interface ITreasureRepository:IRepository<Treasure>
    {
        //methods concerning treasures

        //get all treasures that user hasnt found
        List<Treasure> GetUserTreasures(int UserID);
        List<Treasure> GetTreasuresNotFoundByUser(int UserID);
        List<Treasure> GetTreasuresAndFoundByUser(int UserID);
        IEnumerable<Treasure> GetUserTreasuresNotChained(int UserID);
        IEnumerable<Treasure> GetTreasuresAndComments();
        MarkerInfo GetTreasureInfo(int TreasureId);
        int GetUserHiddenTreasuresCount(int UserId);
    }
}
