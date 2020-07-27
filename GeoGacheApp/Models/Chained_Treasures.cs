using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoGacheApp.Models
{
    public class Chained_Treasures
    {
        public Chained_Treasures(Treasure treasure_1, Treasure treasure_2)
        {
            Treasure_1 = treasure_1;
            Treasure_2 = treasure_2;
        }

        public Chained_Treasures() { }

        [Key]
        public int Id { get; set; }

        public virtual Treasure Treasure_1{ get; set; }
        public virtual Treasure Treasure_2 { get; set; }
    }
}
