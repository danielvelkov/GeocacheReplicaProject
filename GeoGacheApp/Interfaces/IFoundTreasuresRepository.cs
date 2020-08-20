using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Interfaces
{
    public interface IFoundTreasuresRepository:IRepository<Found_Treasures>
    {
        //methods concerning found treasures
        int GetUserPoints(int UserId);
        bool HasUserFoundTreasure(int UserId, int TreasureId);
        Treasure GetNextTreasure(int TreasureId);
        int GetUserFoundTreasuresCount(int UserId);
    }
}
