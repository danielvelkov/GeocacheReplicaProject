using GeocacheV2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeocacheV2
{
    public class GeocachingContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Treasure> Treasures { get; set; }
        public DbSet<Found_Treasures> Found_Treasures { get; set; }
        public DbSet<Treasures_Comments> Treasures_comments { get; set; }
        public DbSet<Chained_Treasures> Chained_Treasures { get; set; }
        public DbSet<MarkerInfo> MarkerInfos { get; set; }

        // DbTest1 is the connection string
        public GeocachingContext() : base(Properties.Settings.Default.DbTest1)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
