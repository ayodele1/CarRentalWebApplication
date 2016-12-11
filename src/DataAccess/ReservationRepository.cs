using DomainObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ReservationRepository
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public ReservationRepository(ApplicationDbContext context, UserManager<ApplicationUser> userMgr)
        {
            _context = context;
            _userManager = userMgr;
        }

        public IEnumerable<Reservation> GetAll()
        {
            return _context.Reservations.ToList();
        }

        public Reservation FindReservationByConfirmationNumber(long confirmationNumber)
        {
            return _context.Reservations.Find(confirmationNumber);
        }

        public bool AddNewReservation(Reservation newReservation, string userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {                
                user.Reservations.Add(newReservation);
                _context.SaveChanges();
                
                return true;
            }
            return false;
        }

        public Reservation FindReservationByUser(long confirmationNumber, ApplicationUser user)
        {
            return _context.Reservations.Where(x => x.ConfirmationNumber == confirmationNumber && string.Compare(x.ApplicationUserId, user.Id) == 0).FirstOrDefault();
        }

    }
}
