using Geocache;
using Geocache.Interfaces;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Geocache.Database.Repositories
{
    class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(GeocachingContext context) : base(context) { }

        public GeocachingContext UserContext
        {
            get { return Context as GeocachingContext; }
        }

        // implement the methods from the used interface
        public bool DoesUserExist(string Username)
        {
            User user = UserContext.Users.First(u=>u.Username==Username);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public void UpdatePassword(int id, string newPassword)
        {
            User user = UserContext.Users.SingleOrDefault(a => a.ID == id);
            user.Password = newPassword;
            UserContext.SaveChanges();
        }

        public User ValidateLogin(string Username, string Password)
        {
            Password = Regex.Replace(Password, @"\s+", "");
            User user = UserContext.Users.FirstOrDefault(a => a.Username == Username && a.Password==Password);
            return user;
        }
    }
}
