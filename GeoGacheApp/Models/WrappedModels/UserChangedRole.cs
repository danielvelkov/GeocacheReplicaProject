using Geocache.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Models.WrappedModels
{
    public class UserChangedRole
    {
        public User User { get; set; }
        public UserRoles UserRole { get; set; }


        public override string ToString()
        {
            return User.FirstName + " " + User.LastName + " from " + User.City + " " + User.Country;
        }
    }
}
