using DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Users.OfType<Customer>().ToList();
        }

        public void AddNewCustomer(Customer newCustomer)
        {
            _context.Users.Add(newCustomer);
            _context.SaveChanges();
        }
    }
}
