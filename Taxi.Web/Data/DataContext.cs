using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.Web.Data.Entities;

namespace Taxi.Web.Data
{
    //public class DataContext: DbContext  //Esta es la configuracion para generar una base de datos vacia
    public class DataContext : IdentityDbContext<UserEntity> //Esta es la configuracion para incluir todas la tablas necesarias  para manejar la seguridad integrada
    {                                                        //y personalizadas con UserEntity   
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
        }
        public DbSet<TaxiEntity> Taxis { get; set; }
        public DbSet<TripEntity> Trips { get; set; }
        public DbSet<TripDetailEntity> TripDetails { get; set; }
        public DbSet<UserGroupEntity> UserGroups { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaxiEntity>()
                .HasIndex(t => t.Plaque)
                .IsUnique();
        }
    }

}
