using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Database
{
    public class DbInitializer : DropCreateDatabaseAlways<GeocachingContext>
    {
        protected override void Seed(GeocachingContext context)
        {
            IList<User> defaultUsers = new List<User>();
            IList<Treasure> defaultTreasures = new List<Treasure>();

            defaultUsers.Add(new User() {
                Username = "geochacher2",
                FirstName = "Dan",
                LastName= "velkov",
                Password= "meatballs",
                Points=0,
                isBanned=false,
                Role=Enums.Roles.USER,
                createdAt=DateTime.Now,
                Country="Bulgaria",
                City="Sliven",
                Adress="centre"
                 });
            defaultUsers.Add(new User()
            {
                Username = "geochacher3",
                FirstName = "Petko",
                LastName = "Petkov",
                Password = "baloni12",
                Points = 0,
                isBanned = false,
                Role = Enums.Roles.MOD,
                createdAt = DateTime.Now,
                Country = "Bulgaria",
                City = "Sliven",
                Adress = "centre"
            });
            defaultUsers.Add(new User()
            {
                Username = "adminadmin",
                FirstName = "Admin",
                LastName = "",
                Password = "password",
                Points = 0,
                isBanned = false,
                Role = Enums.Roles.ADMIN,
                createdAt = DateTime.Now,
                Country = "Bulgaria",
                City = "Sliven",
                Adress = "centre"
            });

            
            context.Users.AddRange(defaultUsers);
            context.SaveChanges();

            defaultTreasures.Add(new Treasure()
            {
                Name = "Fountain Treasure",
                TreasureType = Enums.TreasureType.HIDDEN,
                TreasureSize = Enums.TreasureSizes.MEDIUM,
                Description = "During a full moon the treasure shows",
                Difficulty = 2.5,
                Rating = 2,
                Key = "BOBO",
                IsChained = true,
                UserId = 1,
                MarkerInfo = new MarkerInfo()
                {
                    Latitude = 42.6810581027792,
                    Longtitude = 26.3170980439331,
                    City = "Sliven",
                    Country = "Bulgaria",
                    Address = "bul. \"Hadzhi Dimitar\" 1"
                }
            });

            defaultTreasures.Add(new Treasure()
            {
                Name = "GreatTree Treasure",
                TreasureType = Enums.TreasureType.NORMAL,
                TreasureSize = Enums.TreasureSizes.SMALL,
                Description = "Seek and you shall find.",
                Difficulty = 4.5,
                Rating = 0,
                Key = "BOBO",
                IsChained = false,
                UserId = 1,
                MarkerInfo = new MarkerInfo()
                {
                    Latitude = 42.6804428994465,
                    Longtitude = 26.3164328560974,
                    City = "Sliven",
                    Country = "Bulgaria",
                    Address = "bul. \"Hadzhi Dimitar\" 5A"
                }
            });

            context.Treasures.AddRange(defaultTreasures);
            context.SaveChanges();
            context.Chained_Treasures.Add(new Chained_Treasures() { Treasure_1 = defaultTreasures[0], Treasure_2 = defaultTreasures[1] });

            base.Seed(context);
        }
    }
}
