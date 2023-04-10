using System.Collections.Generic;

namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Entities
{
    public class Building
    {
        public int Id { get; set; }
        public int Floors { get; set; }
        public int Year { get; set; }
        public bool IsParking { get; set; }
        public bool IsLaundry { get; set; }

        public Address Address { get; set; }
        public List<Apartment> Appartments { get; set; }

        public Building()
        {
            Appartments = new List<Apartment>();
        }
    }
}
