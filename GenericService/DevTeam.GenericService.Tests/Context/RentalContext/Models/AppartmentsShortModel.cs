namespace DevTeam.GenericService.Tests.Context.RentalContext.Models;

public class ApartmentShortModel
{
    public int Id { get; set; }
    public string Number { get; set; } = null!;
    public string Size { get; set; } = null!;
    public int Floor { get; set; }
    public bool IsLodge { get; set; }
}
