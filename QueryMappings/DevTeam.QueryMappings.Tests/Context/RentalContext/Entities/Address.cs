namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string BuildingNumber { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Country { get; set; }

        public Building Building { get; set; }
    }
}
