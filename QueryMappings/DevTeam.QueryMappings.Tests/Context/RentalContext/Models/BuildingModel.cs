namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Models;

public class BuildingModel
{
    public int Id { get; set; }
    public int Floors { get; set; }
    public int Year { get; set; }
    public bool IsParking { get; set; }
    public bool IsLaundry { get; set; }

    public AddressModel Address { get; set; } = null!;
    public List<ApartmentModel> Appartments { get; set; } = new();
    public List<ReviewModel> Reviews { get; set; } = new();
}
