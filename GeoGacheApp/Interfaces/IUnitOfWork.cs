using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Interfaces
{
    interface IUnitOfWork : IDisposable
    {
        int Complete();
    }
}
