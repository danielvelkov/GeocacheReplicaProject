using Geocache.Enums;
using Geocache.Interfaces;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Database.Repositories
{
    public class TreasureCommentsRepository:Repository<Treasures_Comments>,ITreasureCommentsRepository
    {
        public TreasureCommentsRepository(GeocachingContext context) : base(context)
        {
        }

        public GeocachingContext TreasureCommentsContext
        {
            get { return Context as GeocachingContext; }
        }

        public bool HasUserReportedTreasure(int UserId, int TreasureId)
        {
            if (TreasureCommentsContext.Treasures_comments.Any(tc =>
             (tc.UserID == UserId && tc.TreasureID == TreasureId) &&
             tc.Type == CommentType.REPORT))
                return true;
            return false;
        }
    }
}
