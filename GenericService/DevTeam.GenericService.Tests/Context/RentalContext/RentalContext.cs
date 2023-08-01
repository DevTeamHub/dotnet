using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.GenericService.Tests.Context.RentalContext.Entities;
using DevTeam.GenericService.Tests.Tests;
using Microsoft.EntityFrameworkCore;

namespace DevTeam.GenericService.Tests.Context.RentalContext;

public interface IRentalContext : IDbContext 
{
    DbSet<Building> Buildings { get; set; } 
    DbSet<Apartment> Apartments { get; set; }
    DbSet<Address> Addresses { get; set; }
    DbSet<Person> People { get; set; } 
    DbSet<Review> Reviews { get; set; }
}

public class RentalContext : DbContext, IRentalContext
{
    private static DbContextOptions<RentalContext> GetOptions(string name)
    {
        return new DbContextOptionsBuilder<RentalContext>()
            .UseInMemoryDatabase(databaseName: name)
            .Options;
    }

    public DbSet<Building> Buildings { get; set; } = null!;
    public DbSet<Apartment> Apartments { get; set; } = null!;
    public DbSet<Address> Addresses { get; set; } = null!;
    public DbSet<Person> People { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;

    public RentalContext(string name)
        : base(GetOptions(name))
    {
        Buildings.AddRange(TestData.Buildings);
        Apartments.AddRange(TestData.Apartments);
        Addresses.AddRange(TestData.Addresses);
        People.AddRange(TestData.People);
        Reviews.AddRange(TestData.Reviews);
        SaveChanges();
    }

    public RentalContext()
        : this(Guid.NewGuid().ToString())
    { }
}
