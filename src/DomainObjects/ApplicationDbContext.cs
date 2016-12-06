using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DomainObjects
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private IConfigurationRoot _config;

        public ApplicationDbContext(IConfigurationRoot config) : base()
        {
            _config = config;
        }

        //        public DbSet<User> Users { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerRepresentative> CustomerRepresentatives { get; set; }

        public DbSet<NonCustomer> NonCustomers { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            base.OnConfiguring(ob);
            var connectionString = _config.GetConnectionString("ConnectionString");
            ob.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
            mb.Entity<ApplicationUser>().ToTable("Users", "dbo");
            mb.Entity<IdentityRole>().ToTable("MyRoles", "dbo");
            mb.Entity<Reservation>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(y => y.Reservations)
                .HasForeignKey(z => z.ApplicationUserId);
            mb.Entity<Rental>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(y => y.Rentals)
                .HasForeignKey(z => z.ApplicationUserId);
                
            
        }

        /// <summary>
        /// Generate the Date Modified and Date Created values
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            foreach (var history in this.ChangeTracker.Entries()
                .Where(e => e.Entity is IModificationHistory && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .Select(e => e.Entity as IModificationHistory))
            {
                history.DateModified = DateTime.Now;
                if (history.DateCreated == DateTime.MinValue)
                    history.DateCreated = DateTime.Now;

            }
            return base.SaveChanges();
        }
    }
}
