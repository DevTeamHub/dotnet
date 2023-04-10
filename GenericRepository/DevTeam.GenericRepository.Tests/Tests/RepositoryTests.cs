using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.GenericRepository.Tests.Context.RentalContext;
using DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;
using DevTeam.GenericRepository.Tests.Context.SecurityContext;
using DevTeam.GenericRepository.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

namespace DevTeam.GenericRepository.Tests.Tests;

[Category("Repository")]
[TestOf(typeof(Repository))]
[TestFixture]
public class RepositoryTests
{
    private ServiceProvider _serviceProvider;
    private IRepository _repository;
    private RentalContext _rentalContext;
    private SecurityContext _securityContext;

    [OneTimeSetUp]
    public void Init()
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

    [OneTimeTearDown]
    public void Clear()
    {
        _rentalContext.Dispose();
        _securityContext.Dispose();
        _rentalContext = null;
        _securityContext = null;
        _repository = null;
    }

    [Test]
    public void Should_Correctly_Resolve_Different_Repositories()
    {
        var standard = _serviceProvider.GetRequiredService<IRepository>();
        var rental = _serviceProvider.GetRequiredService<IRepository<IRentalContext>>();
        var security = _serviceProvider.GetRequiredService<IRepository<ISecurityContext>>();

        Assert.IsInstanceOf<Repository>(standard);
        Assert.IsInstanceOf<Repository<IRentalContext>>(rental);
        Assert.IsInstanceOf<Repository<ISecurityContext>>(security);
    }

    [Test]
    public void Should_Correctly_Resolve_Different_Read_Only_Repositories()
    {
        var standard = _serviceProvider.GetRequiredService<IReadOnlyRepository>();
        var rental = _serviceProvider.GetRequiredService<IReadOnlyRepository<IRentalContext>>();
        var security = _serviceProvider.GetRequiredService<IReadOnlyRepository<ISecurityContext>>();

        Assert.IsInstanceOf<ReadOnlyRepository>(standard);
        Assert.IsInstanceOf<ReadOnlyRepository<IRentalContext>>(rental);
        Assert.IsInstanceOf<ReadOnlyRepository<ISecurityContext>>(security);
    }

    [Test]
    public void Should_Correctly_Resolve_Different_Soft_Delete_Repositories()
    {
        var standard = _serviceProvider.GetRequiredService<ISoftDeleteRepository>();
        var rental = _serviceProvider.GetRequiredService<ISoftDeleteRepository<IRentalContext>>();
        var security = _serviceProvider.GetRequiredService<ISoftDeleteRepository<ISecurityContext>>();

        Assert.IsInstanceOf<SoftDeleteRepository>(standard);
        Assert.IsInstanceOf<SoftDeleteRepository<IRentalContext>>(rental);
        Assert.IsInstanceOf<SoftDeleteRepository<ISecurityContext>>(security);
    }

    [Test]
    public void Should_Correctly_Resolve_Different_Read_Only_Delete_Repositories()
    {
        var standard = _serviceProvider.GetRequiredService<IReadOnlyDeleteRepository>();
        var rental = _serviceProvider.GetRequiredService<IReadOnlyDeleteRepository<IRentalContext>>();
        var security = _serviceProvider.GetRequiredService<IReadOnlyDeleteRepository<ISecurityContext>>();

        Assert.IsInstanceOf<ReadOnlyDeleteRepository>(standard);
        Assert.IsInstanceOf<ReadOnlyDeleteRepository<IRentalContext>>(rental);
        Assert.IsInstanceOf<ReadOnlyDeleteRepository<ISecurityContext>>(security);
    }

    [Test]
    public void Should_Return_Only_Not_Deleted_Items()
    {
        var entities = _rentalContext.People.ToList();
        var query = entities.AsQueryable();

        var modelsQuery = _repository.GetList<Person>();

        Assert.IsNotNull(modelsQuery);
        Assert.IsInstanceOf<IQueryable<Person>>(modelsQuery);

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
