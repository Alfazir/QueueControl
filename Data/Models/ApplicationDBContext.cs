using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QueueControlServer.Models;

namespace QueueControlServer.Models
{
    public class ApplicationDBContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
             Database.EnsureCreated();  
        }

        /*  protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
              base.OnModelCreating(modelBuilder);

              modelBuilder.Entity<User>(b =>
              {
                  // Each User can have many UserClaims
                  b.HasMany(e => e.FactoryName)
                      .WithOne()
                      .HasForeignKey(uc => uc.UserId)
                      .IsRequired();
              });
          }
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<QueueItems>()
                .Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<QueueItems>()
                .Property(u => u.CarrierName)
                     .HasDefaultValue("———");

            modelBuilder.Entity<Queue>().Navigation(u => u.Factory).AutoInclude();




                                  
          base.OnModelCreating(modelBuilder);


        }



        public virtual DbSet<QueueItems> QueueItems { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Factory> Factories { get; set; }
        public virtual DbSet<Carrier> Carriers { get; set; }
        public virtual DbSet<Terminal> Terminals { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Supervisor> Supervisors { get; set; }
        public virtual DbSet<LocalAdmin> LocalAdmins { get; set; }
        public virtual DbSet<GoodsParameter> GoodsParameters { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public DbSet<Queue> Queue { get; set; }
    }
}
