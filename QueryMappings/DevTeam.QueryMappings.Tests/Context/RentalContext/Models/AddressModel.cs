namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Models
{
    public class AddressModel
    {
        public int Id { get; set; }
        public string BuildingNumber { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Countries Country { get; set; }
    }
}
