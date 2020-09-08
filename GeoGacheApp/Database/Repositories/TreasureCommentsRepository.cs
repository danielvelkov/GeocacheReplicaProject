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

        public double GetTreasureRating(int TreasureId)
        {
            int count = 0;
            double rating = 0;
            foreach (var tc in TreasureCommentsContext.Treasures_comments.Where(tc => tc.TreasureID == TreasureId))
            {
                count++;
                rating += tc.Rated;
            }
            if (rating == 0)
                return 0;
            return rating / (double)count;
        }

        public int GetRatingOfTreasureByUser(int UserId, int TreasureId)
        {
            return (int)TreasureCommentsContext.Treasures_comments.SingleOrDefault
                (t => t.UserID == UserId && t.TreasureID == TreasureId && t.Type==CommentType.COMMENT).Rated;
        }

        public bool HasUserCommented(int UserId, int TreasureId)
        {
            if (TreasureCommentsContext.Treasures_comments.Any(tc =>
             (tc.UserID == UserId && tc.TreasureID == TreasureId) &&
             tc.Type == CommentType.COMMENT))
                return true;
            return false;
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
