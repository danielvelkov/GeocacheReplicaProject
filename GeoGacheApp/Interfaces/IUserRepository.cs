using Geocache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        // methods concerning users
        bool DoesUserExist(string Username);
        User ValidateLogin(string Username, string Password);
        void UpdatePassword(int id, string Password);
    }
}
