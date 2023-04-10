using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;

public class Address
{
    [Key]
    public int Id { get; set; }
    public int BuildingId { get; set; }
    public string BuildingNumber { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public int Country { get; set; }

    public Building Building { get; set; } = null!;
}
