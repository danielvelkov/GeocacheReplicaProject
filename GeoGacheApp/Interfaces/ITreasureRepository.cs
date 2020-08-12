using Geocache;
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
        List<Treasure> GetOthersTreasures(int UserID);
        List<Treasure> GetUserTreasuresNotChained(int UserID);
    }
}
