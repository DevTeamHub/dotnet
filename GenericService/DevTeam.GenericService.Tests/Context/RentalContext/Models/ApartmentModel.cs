namespace DevTeam.GenericService.Tests.Context.RentalContext.Models;

public class ApartmentModel
{
    public int Id { get; set; }
    public int BuildingId { get; set; }
    public string Number { get; set; } = null!;
    public string Size { get; set; } = null!;
    public int Badrooms { get; set; }
    public int Bathrooms { get; set; }
    public int Floor { get; set; }
    public bool IsLodge { get; set; }

    public BuildingModel Building { get; set; } = null!;
    public List<PersonModel> Residents { get; set; } = new();
    public List<ReviewModel> Reviews { get; set; } = new();
}
