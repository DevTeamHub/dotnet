namespace DevTeam.GenericService.Tests.Context.RentalContext.Models;

public class AddressModel
{
    public int Id { get; set; }
    public string BuildingNumber { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public Countries Country { get; set; }
}
