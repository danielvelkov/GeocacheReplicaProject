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

        // DbTest1 is the connection string
        public GeocachingContext() : base(Properties.Settings.Default.DbTest1)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<User>()
        //    //    .HasMany<Treasure>(u => u.Treasures)
        //    //    .WithRequired(t => t.User)
        //    //   //.HasForeignKey(t=>t.UserId)
        //    //   ;

        //    //modelBuilder.Entity<User>()
        //    //    .HasMany<Found_Treasures>(u => u.Found_Treasures)
        //    //    .WithRequired(t => t.User);
        //    //// .HasForeignKey(t => t.UserID);

        //    //modelBuilder.Entity<User>()
        //    //    .HasMany<Treasures_Comments>(u => u.Treasures_Comments)
        //    //    .WithRequired(t => t.User);
        //    //// .HasForeignKey(t => t.UserID);



        //    modelBuilder.Entity<Treasure>()
        //        .HasOptional(t => t.MarkerInfo)
        //        .WithOptionalPrincipal(mi => mi.Treasure);

        //    //modelBuilder.Entity<Treasure>()
        //    //    .HasMany<Treasures_Comments>(u => u.Treasures_Comments)
        //    //    .WithRequired(t => t.Treasure)
        //    //    .HasForeignKey(t => t.TreasureID);

        //    //modelBuilder.Entity<Treasure>()
        //    //    .HasMany<Found_Treasures>(u => u.Found_Treasures)
        //    //    .WithRequired(t => t.Treasure)
        //    //    .HasForeignKey(t => t.TreasureID);

        //    //modelBuilder.Entity<Treasure>()
        //    //    .HasOptional(t => t.Chained_Treasures)
        //    //    .WithRequired(ct => ct.Treasure_1);


        //    //modelBuilder.Entity<Treasure>()
        //    //    .HasOptional(t => t.Chained_Treasures)
        //    //    .WithRequired(ct => ct.Treasure_2);

        //    modelBuilder.Entity<Chained_Treasures>()
        //        .HasIndex(ct => ct.Treasure_1)
        //        .IsUnique();
        
        //}
    }
}
