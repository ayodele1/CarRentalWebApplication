using DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CustomerRepresentativeRepository
    {
        private ApplicationDbContext _context;

        public CustomerRepresentativeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CustomerRepresentative> GetAll()
        {
            return _context.Users.OfType<CustomerRepresentative>().ToList();
        }

        public void AddNewCustomerRep(CustomerRepresentative newCustomerRep)
        {
            _context.Users.Add(newCustomerRep);
            _context.SaveChanges();
        }
    }
}
