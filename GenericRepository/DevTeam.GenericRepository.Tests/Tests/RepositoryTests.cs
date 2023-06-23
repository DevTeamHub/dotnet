using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.GenericRepository.Tests.Context.RentalContext;
using DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;
using DevTeam.GenericRepository.Tests.Context.SecurityContext;
using DevTeam.GenericRepository.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevTeam.GenericRepository.Tests;

[TestCategory("Repository")]
[TestClass]
public class RepositoryTests
{
    private static ServiceProvider _serviceProvider = null!;
    private static IRepository _repository = null!;
    private static RentalContext _rentalContext = null!;
    private static SecurityContext _securityContext = null!;

    [ClassInitialize]
    public static void Init(TestContext testContext)
    {
        var services = new ServiceCollection();

        services
            .AddDbContext<IDbContext, RentalContext>()
            .AddDbContext<IRentalContext, RentalContext>()
            .AddDbContext<ISecurityContext, SecurityContext>()
            .AddGenericRepository();

        _serviceProvider = services.BuildServiceProvider();

        _rentalContext = (RentalContext)_serviceProvider.GetRequiredService<IRentalContext>();
        _repository = _serviceProvider.GetRequiredService<IRepository>();


        _rentalContext = new RentalContext("OriginalRental");
        _securityContext = new SecurityContext("OriginalSecurity");
    }

    [ClassCleanup]
    public static void Clear()
    {
        _rentalContext.Dispose();
        _securityContext.Dispose();
        _rentalContext = null!;
        _securityContext = null!;
        _repository = null!;
    }

    [TestMethod]
    public void Should_Correctly_Resolve_Different_Repositories()
    {
        var standard = _serviceProvider.GetRequiredService<IRepository>();
        var rental = _serviceProvider.GetRequiredService<IRepository<IRentalContext>>();
        var security = _serviceProvider.GetRequiredService<IRepository<ISecurityContext>>();

        Assert.IsInstanceOfType(standard, typeof(Repository));
        Assert.IsInstanceOfType(rental, typeof(Repository<IRentalContext>));
        Assert.IsInstanceOfType(security, typeof(Repository<ISecurityContext>));
    }

    [TestMethod]
    public void Should_Correctly_Resolve_Different_Read_Only_Repositories()
    {
        var standard = _serviceProvider.GetRequiredService<IReadOnlyRepository>();
        var rental = _serviceProvider.GetRequiredService<IReadOnlyRepository<IRentalContext>>();
        var security = _serviceProvider.GetRequiredService<IReadOnlyRepository<ISecurityContext>>();

        Assert.IsInstanceOfType(standard, typeof(ReadOnlyRepository));
        Assert.IsInstanceOfType(rental, typeof(ReadOnlyRepository<IRentalContext>));
        Assert.IsInstanceOfType(security, typeof(ReadOnlyRepository<ISecurityContext>));
    }

    [TestMethod]
    public void Should_Correctly_Resolve_Different_Soft_Delete_Repositories()
    {
        var standard = _serviceProvider.GetRequiredService<ISoftDeleteRepository>();
        var rental = _serviceProvider.GetRequiredService<ISoftDeleteRepository<IRentalContext>>();
        var security = _serviceProvider.GetRequiredService<ISoftDeleteRepository<ISecurityContext>>();

        Assert.IsInstanceOfType(standard, typeof(SoftDeleteRepository));
        Assert.IsInstanceOfType(rental, typeof(SoftDeleteRepository<IRentalContext>));
        Assert.IsInstanceOfType(security, typeof(SoftDeleteRepository<ISecurityContext>));
    }

    [TestMethod]
    public void Should_Correctly_Resolve_Different_Read_Only_Delete_Repositories()
    {
        var standard = _serviceProvider.GetRequiredService<IReadOnlyDeleteRepository>();
        var rental = _serviceProvider.GetRequiredService<IReadOnlyDeleteRepository<IRentalContext>>();
        var security = _serviceProvider.GetRequiredService<IReadOnlyDeleteRepository<ISecurityContext>>();

        Assert.IsInstanceOfType(standard, typeof(ReadOnlyDeleteRepository));
        Assert.IsInstanceOfType(rental, typeof(ReadOnlyDeleteRepository<IRentalContext>));
        Assert.IsInstanceOfType(security, typeof(ReadOnlyDeleteRepository<ISecurityContext>));
    }

    [TestMethod]
    public void Should_Return_Only_Not_Deleted_Items()
    {
        var entities = _rentalContext.People.ToList();
        var query = entities.AsQueryable();

        var modelsQuery = _repository.GetList<Person>();

        Assert.IsNotNull(modelsQuery);
        Assert.IsInstanceOfType(modelsQuery, typeof(IQueryable<Person>));

        var models = modelsQuery.ToList();

        Assert.AreEqual(entities.Count(x => !x.IsDeleted), models.Count);

        foreach (var entity in entities)
        {
            var model = models.FirstOrDefault(x => x.Id == entity.Id);

            if (model != null)
            {
                Assert.AreEqual(entity.Id, model.Id);
                Assert.AreEqual(entity.AppartmentId, model.AppartmentId);
                Assert.AreEqual(entity.FirstName, model.FirstName);
                Assert.AreEqual(entity.LastName, model.LastName);
                Assert.AreEqual(entity.Age, model.Age);
                Assert.AreEqual(entity.Gender, model.Gender);
                Assert.AreEqual(entity.Email, model.Email);
                Assert.AreEqual(entity.Phone, model.Phone);
                Assert.AreEqual(entity.IsDeleted, false);
            }
            else
            {
                Assert.AreEqual(entity.IsDeleted, true);
            }
        }
    }
}
