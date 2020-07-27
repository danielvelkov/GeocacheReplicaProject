﻿
using GeoGacheApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoGacheApp
{
    public enum TreasureSizes { SMALL = 1, MEDIUM = 2, LARGE = 3 }
    public enum TreasureType { NORMAL = 1, HIDDEN=2, SURPRISE=3 }

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
            this.isChained = isChained;
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
        public bool isChained { get; set; }

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
        public virtual Chained_Treasures Chained_Treasures { get; set; }
    }

    public class MarkerInfo
    {
        public MarkerInfo(int treasureID,double latitude,
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