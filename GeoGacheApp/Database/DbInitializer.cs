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

            base.Seed(context);
        }
    }
}
