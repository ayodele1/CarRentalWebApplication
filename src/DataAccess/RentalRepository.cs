using DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RentalRepository
    {
        private ApplicationDbContext _context;

        public RentalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddNewRental(Rental newRental)
        {
            _context.Rentals.Add(newRental);
            _context.SaveChanges();
        }

        public IEnumerable<Rental> GetAll()
        {
            return _context.Rentals.ToList();
        }
    }
}
