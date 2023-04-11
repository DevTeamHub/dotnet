namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public int AppartmentId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }
        public int Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public Apartment Appartment { get; set; } = null!;
    }
}
