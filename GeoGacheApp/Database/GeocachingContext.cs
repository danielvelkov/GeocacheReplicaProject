using Geocache.Database;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache
{
    public class GeocachingContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Treasure> Treasures { get; set; }
        public DbSet<Found_Treasures> Found_Treasures { get; set; }
        public DbSet<Treasures_Comments> Treasures_comments { get; set; }
        public DbSet<Chained_Treasures> Chained_Treasures { get; set; }
        public DbSet<MarkerInfo> MarkerInfos { get; set; }
        
        //for testing (creates a db in localDB)
        public GeocachingContext():base()
        {
            
            System.Data.Entity.Database.SetInitializer<GeocachingContext>(new DbInitializer());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Treasure>().
                HasOptional(p => p.MarkerInfo).
                WithRequired(s=>s.Treasure).
                WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}