using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Models
{
    public class Found_Treasures
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }


        [ForeignKey("User"), Column(Order = 1)]
        public int? UserID { get; set; }
        [ForeignKey("Treasure"), Column(Order = 2)]
        public int? TreasureID { get; set; }


        public int Points { get; set; }
        public DateTime Found_at { get; set; }

        public virtual Treasure Treasure { get; set; }
        public virtual User User { get; set; }


        public Found_Treasures() { }

        public Found_Treasures(
            int userID,
            int treasureID,
            int points,
            DateTime found_at)
        {
            UserID = userID;
            TreasureID = treasureID;
            Points = points;
            Found_at = found_at;
        }
    }
}
