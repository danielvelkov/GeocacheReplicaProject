using Geocache.Models;
using Geocache.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRoles Role { get; set; }
            
        public bool isBanned { get; set; }
        public int Points { get; set; }
        public DateTime createdAt { get; set; }
        // non-required data
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string Country { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }

        [Key]
        public int ID { get; set; }

        public virtual ICollection<Treasure> Treasures { get; set; }
        public virtual ICollection<Treasures_Comments> Treasures_Comments { get; set; }
        public virtual ICollection<Found_Treasures> Found_Treasures { get; set; }

        public User()
        {
            Treasures = new ObservableCollection<Treasure>();
            Treasures_Comments = new ObservableCollection<Treasures_Comments>();
            Found_Treasures = new ObservableCollection<Found_Treasures>();
        }

        public User(string username, string password, UserRoles role, bool isBanned, int points, DateTime createdAt, string firstName, string lastName, string country, string city, string adress)
        {
            Username = username;
            Password = password;
            Role = role;
            this.isBanned = isBanned;
            Points = points;
            this.createdAt = createdAt;
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            City = city;
            Adress = adress;
        }
    }
}
