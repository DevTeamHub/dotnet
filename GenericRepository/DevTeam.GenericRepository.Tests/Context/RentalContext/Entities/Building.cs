using System.ComponentModel.DataAnnotations;

namespace DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;

public class Building
{
    [Key]
    public int Id { get; set; }
    public int Floors { get; set; }
    public int Year { get; set; }
    public bool IsParking { get; set; }
    public bool IsLaundry { get; set; }

    public Address Address { get; set; } = null!;
    public List<Apartment> Appartments { get; set; } = new();
}
