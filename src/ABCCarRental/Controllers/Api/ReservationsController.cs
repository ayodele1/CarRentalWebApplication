using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCCarRental.Controllers.Api
{
    public class ReservationsController : Controller
    {
        private ReservationRepository _res;

        public ReservationsController(ReservationRepository repo)
        {
            _res = repo;
        }

        [HttpGet("/api/Reservations/")]
        public IActionResult Get([FromQuery]string user_id)
        {
            try
            {
                var reservations = _res.GetReservationsForUser(user_id);
                return Ok(JsonConvert.SerializeObject(reservations, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            }
            catch (Exception ex)
            {

            }
            return BadRequest("Failed to get Reservations");
        }
    }
}
