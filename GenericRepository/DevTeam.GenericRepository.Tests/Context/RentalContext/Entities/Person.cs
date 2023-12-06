using DevTeam.Extensions.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;

public class Person : IEntity, IDeleted
{
    [Key]
    public int Id { get; set; }
    public int ApartmentId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public int Gender { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsDeleted { get; set; }
    public List<PermissionModel> Permissions { get; set; } = null!;

    public Apartment Appartment { get; set; } = null!;
}