using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Models
{
    public class MarkerInfo
    {
        public MarkerInfo(int treasureID, double latitude,
            double longtitude,
            string city,
            string country,
            string adress)
        {
            TreasureId = treasureID;
            Latitude = latitude;
            Longtitude = longtitude;
            City = city;
            Country = country;
            Adress = adress;
        }

        public MarkerInfo() { }

        [Key]
        [ForeignKey("Treasure")]
        public int TreasureId { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string Adress { get; set; }

        public virtual Treasure Treasure { get; set; }
    }
}
