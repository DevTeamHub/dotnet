using System.Collections.Generic;

namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Models
{
    public class BuildingModel
    {
        public int Id { get; set; }
        public int Floors { get; set; }
        public int Year { get; set; }
        public bool IsParking { get; set; }
        public bool IsLaundry { get; set; }

        public AddressModel Address { get; set; }
        public List<ApartmentModel> Appartments { get; set; }
        public List<ReviewModel> Reviews { get; set; }

        public BuildingModel()
        {
            Appartments = new List<ApartmentModel>();
            Reviews = new List<ReviewModel>();
        }
    }
}
