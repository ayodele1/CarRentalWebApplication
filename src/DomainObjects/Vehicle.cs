using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    public enum VehicleType
    {
        Car,
        Suv,
        Truck,
        Luxury
    }


    public class Vehicle
    {
        public Vehicle()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }

        public int MakeYear { get; set; }

        public double PricePerDay { get; set; }

        public VehicleType ModelType { get; set; }

        public int PassengerCapacity { get; set; }

        public string ImageName { get; set; }

        public int isAvailable { get; set; }

        [NotMapped]
        public bool IsAvailable
        {
            get { return isAvailable != 0; }
            set { isAvailable = value ? 1 : 0; }
        }
    }
}
