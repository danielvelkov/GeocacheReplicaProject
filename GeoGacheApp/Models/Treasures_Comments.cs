using Geocache.Database;
using Geocache.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Models
{
    public class Treasures_Comments
    {
        public Treasures_Comments(
            int treasureID,
            int userID, 
            string content,
            DateTime createdAt,
            CommentType type, 
            double rated)
        {
            TreasureID = treasureID;
            UserID = userID;
            Content = content;
            CreatedAt = createdAt;
            Type = type;
            Rated = rated;
        }

        public Treasures_Comments() { }

        [Key, Column(Order = 0)]
        public int ID { get; set; }

        public virtual Treasure Treasure { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Treasure"), Column(Order=1)]
        public int? TreasureID { get; set; }
        [ForeignKey("User"), Column(Order =2)]
        public int? UserID { get; set; }
        
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public CommentType Type { get; set; }
        public double Rated { get; set; }

        [NotMapped]
        public string Username
        {
            get
            {
                using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                    return unitOfWork.Users.Get((int)UserID).Username;
            }

        }
    }
}
