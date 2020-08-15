
using Geocache.Enums;
using Geocache.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Models
{
    public class Treasure
    {
        public Treasure(string name,
            TreasureType treasureType,
            TreasureSizes treasureSize,
            string description,
            double difficulty,
            double rating,
            int userId,
            bool isChained)
        {
            Name = name;
            TreasureType = treasureType;
            TreasureSize = treasureSize;
            Description = description;
            Difficulty = difficulty;
            Rating = rating;
            Key = GenerateKey();
            UserId = userId;
            this.IsChained = isChained;
        }

        public Treasure()
        {
            Found_Treasures = new HashSet<Found_Treasures>();
            Treasures_Comments = new HashSet<Treasures_Comments>();
        }

        public string Name { get; set; }
        public TreasureType TreasureType { get; set; }

        public TreasureSizes TreasureSize { get; set; }
        public string Description { get; set; }
        public double Difficulty { get; set; }
        public double Rating { get; set; }
        public string Key { get; set; }
        public bool IsChained { get; set; }

        /* a way to get a random key with a defined lenght */
        // this case its 8 
        public static string GenerateKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private static Random random = new Random();


        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual MarkerInfo MarkerInfo { get; set; }
        public virtual ICollection<Treasures_Comments> Treasures_Comments { get; set; }
        public virtual ICollection<Found_Treasures> Found_Treasures { get; set; }

        [InverseProperty("Treasure_1")]
        public virtual ICollection<Chained_Treasures> Chained_Treasure1 { get; set; }
        [InverseProperty("Treasure_2")]
        public virtual ICollection<Chained_Treasures> Chained_Treasure2 { get; set; }

        public Location GetLatLng()
        {
            return new Location(MarkerInfo.Latitude, MarkerInfo.Longtitude);
        }
    }
}
