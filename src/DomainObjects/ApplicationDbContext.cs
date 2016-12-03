using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace DomainObjects
{
    public class ApplicationDbContext : DbContext
    {
        private IConfigurationRoot _config;

        public ApplicationDbContext(IConfigurationRoot config) : base()
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerRepresentative> CustomerRepresentatives { get; set; }

        public DbSet<NonCustomer> NonCustomers { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            var connectionString =   _config.GetConnectionString("ConnectionString");
            base.OnConfiguring(ob);
            ob.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {

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
