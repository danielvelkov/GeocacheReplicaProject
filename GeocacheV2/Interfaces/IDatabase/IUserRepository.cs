using GeocacheV2;
using GeocacheV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeocacheV2.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        // methods concerning users
        bool DoesUserExist(string Username);
        User ValidateLogin(string Username, string Password);
        void UpdatePassword(int id, string Password);
    }
}
