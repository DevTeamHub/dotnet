using System.ComponentModel.DataAnnotations;

namespace DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;

public class Apartment
{
    [Key]
    public int Id { get; set; }
    public int BuildingId { get; set; }
    public string Number { get; set; } = null!;
    public int Size { get; set; }
    public int Badrooms { get; set; }
    public int Bathrooms { get; set; }
    public int Floor { get; set; }
    public bool IsLodge { get; set; }

    public Building Building { get; set; } = null!;
    public List<Person> Residents { get; set; } = new();
}
