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
        List<Treasure> GetTreasures(int UserID);
    }
}
