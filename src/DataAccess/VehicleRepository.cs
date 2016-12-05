using DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class VehicleRepository
    {
        private ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Vehicle> GetAll()
        {
            return _context.Vehicles.ToList();
        }

        public IEnumerable<Vehicle> GetAllAvailableVehicles()
        {
            return _context.Vehicles.Where(x => x.IsAvailable).ToList();
        }

        public Vehicle GetVehicleById(int id)
        {
            return _context.Vehicles.Where(x => x.Id == id).FirstOrDefault();
        }

        public void AddNewVehicle(Vehicle newVehicle)
        {
            _context.Vehicles.Add(newVehicle);
            _context.SaveChanges();
        }
    }
}
