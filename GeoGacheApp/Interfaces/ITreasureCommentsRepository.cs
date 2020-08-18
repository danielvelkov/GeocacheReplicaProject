using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Interfaces
{
    public interface ITreasureCommentsRepository:IRepository<Treasures_Comments>
    {
        bool HasUserReportedTreasure(int UserId, int TreasureId);
        double GetTreasureRating(int TreasureId);
    }
}
