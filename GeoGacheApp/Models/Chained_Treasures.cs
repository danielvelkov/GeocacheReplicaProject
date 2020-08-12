using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Models
{

    public class Chained_Treasures
    {
        public Chained_Treasures(int treasure1Id, int treasure2Id)
        {
            Treasure1_ID = treasure1Id;
            Treasure2_ID = treasure2Id;
        }

        public Chained_Treasures() { }

        [Key]
        public int Id { get; set; }

        [ForeignKey("Treasure_1")]
        public int Treasure1_ID { get; set; }

        [ForeignKey("Treasure_2")]
        public int Treasure2_ID { get; set; }

        public virtual Treasure Treasure_1 { get; set; }
        public virtual Treasure Treasure_2 { get; set; }
    }
}
