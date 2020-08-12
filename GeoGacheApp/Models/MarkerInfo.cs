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

        // this gets the distance between this marker and the lat n lon and checks if its lower than the range
        public bool IsInRadius(double lat,double lon,double RangeInKm)
        {
            double R = 6371000; //earths radius in m
            double F1 = lat * Math.PI / 180; // φ, λ in radians
            double F2 = Latitude * Math.PI / 180;
            double Δφ = (Latitude - lat) * Math.PI / 180;
            double Δλ = (Longtitude - lon) * Math.PI / 180;

            double a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                      Math.Cos(F1) * Math.Cos(F2) *
                      Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = R * c; // in metres

            if ((d / 1000) < RangeInKm)
                return true;
            return false;
        }

        public string GetMarkerAddress()
        {
            StringBuilder address = new StringBuilder();
            if (!string.IsNullOrEmpty(this.Adress))
                address.Append(this.Adress + ",");
            if (!string.IsNullOrEmpty(this.City))
                address.Append(this.City + ",");
            if (!string.IsNullOrEmpty(this.Country))
                address.Append(this.Country);
            else address.Length--;
            return address.ToString();
        }
    }
}
