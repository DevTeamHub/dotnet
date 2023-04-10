using System.Collections.Generic;

namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Entities
{
    public class Apartment
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string Number { get; set; }
        public int Size { get; set; }
        public int Badrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Floor { get; set; }
        public bool IsLodge { get; set; }

        public Building Building { get; set; }
        public List<Person> Residents { get; set; }

        public Apartment()
        {
            Residents = new List<Person>();
        }
    }
}
