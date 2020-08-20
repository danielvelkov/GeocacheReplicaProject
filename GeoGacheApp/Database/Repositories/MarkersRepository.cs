using Geocache.Interfaces;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Database.Repositories
{
    public class MarkersRepository:Repository<MarkerInfo>,IMarkerRepository
    {
        public MarkersRepository(GeocachingContext context) : base(context)
        {
        }

        public GeocachingContext MarkersContext
        {
            get { return Context as GeocachingContext; }
        }
    }
}
