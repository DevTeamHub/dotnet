using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.GenericRepository;
using DevTeam.GenericService.AspNetCore;
using DevTeam.GenericService.Tests.Context.RentalContext;
using DevTeam.GenericService.Tests.Context.RentalContext.Entities;
using DevTeam.GenericService.Tests.Context.RentalContext.Mappings;
using DevTeam.GenericService.Tests.Context.RentalContext.Models;
using DevTeam.GenericService.Tests.Context.SecurityContext;
using DevTeam.QueryMappings.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevTeam.GenericService.Tests.Tests;

[TestCategory("GenericService")]
[TestClass]
public class GenericServiceTests
{
    private static ServiceProvider _serviceProvider = null!;
    private static IGenericService<TestQueryOptions> _service = null!;
    private static IRepository<TestQueryOptions> _repository = null!;
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
            .AddGenericServices()
            .AddScoped(typeof(IGenericService<TestQueryOptions>), typeof(GenericService<TestQueryOptions>))
            .AddScoped(typeof(IRepository<TestQueryOptions>), typeof(Repository<TestQueryOptions>))
            .AddScoped(typeof(ISoftDeleteGenericService<TestQueryOptions>), typeof(SoftDeleteGenericService<TestQueryOptions>))
            .AddScoped(typeof(IReadOnlyRepository<TestQueryOptions>), typeof(ReadOnlyRepository<TestQueryOptions>))
            .AddScoped(typeof(IQueryExtension<,>), typeof(IncludeDeletedQueryExtension<,>))
            .AddScoped(typeof(IQueryExtension<,>), typeof(IsReadOnlyQueryExtension<,>));

        _serviceProvider = services.BuildServiceProvider();

        _rentalContext = (RentalContext)_serviceProvider.GetRequiredService<IRentalContext>();
        _service = _serviceProvider.GetRequiredService<IGenericService<TestQueryOptions>>();
        _repository = _serviceProvider.GetRequiredService<IRepository<TestQueryOptions>>();
        var mappings = _serviceProvider.GetRequiredService<IMappingsList>();

        MappingsConfiguration.Register(mappings, typeof(AddressMappings).Assembly);

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
        _service = null!;
    }

    [TestMethod]
    [DataRow(1)]
    public void Should_Update_By_Id_Return_Entity(int modelId)
    {
        var model = _service.Get<Address, AddressModel>(modelId)!;

        model.State = "Oklahoma";

        var returnResult = _service.Update<AddressModel, Address, int>(modelId + 1, model, (m, e) =>
        {
            e.Street = m.Street;
            e.State = m.State;
        });

        var result = _service.Get<Address, AddressModel>(modelId + 1);
        var sourceModel = _service.Get<Address, AddressModel>(modelId);

        var entityResult = _repository.Get<Address>(modelId + 1);

        Assert.IsNotNull(entityResult);
        Assert.IsNotNull(sourceModel);
        Assert.IsNotNull(result);
        Assert.IsNotNull(returnResult);

        Assert.AreEqual(sourceModel.Street, result.Street);
        Assert.AreNotEqual(sourceModel.State, result.State);
        
        Assert.AreEqual(returnResult.Building, entityResult.Building);
        Assert.AreEqual(returnResult.State, entityResult.State);
        Assert.AreEqual(returnResult.Street, entityResult.Street);
        Assert.AreEqual(returnResult.City, entityResult.City);
        Assert.AreEqual(returnResult.Country, entityResult.Country);
    }

    [TestMethod]
    [DataRow(1)]
    public void Should_Update_By_Id_Return_Result(int modelId)
    {
        var model = _service.Get<Address, AddressModel>(modelId)!;

        model.State = "Oklahoma";

        var returnResult = _service.Update<AddressModel, Address, AddressModel>(modelId + 1, model, (m, e) =>
        {
            e.Street = m.Street;
            e.State = m.State;
        });

        var result = _service.Get<Address, AddressModel>(modelId + 1);
        var sourceModel = _service.Get<Address, AddressModel>(modelId);

        Assert.IsNotNull(sourceModel);
        Assert.IsNotNull(result);
        Assert.IsNotNull(returnResult);

        Assert.AreEqual(sourceModel.Street, result.Street);
        Assert.AreNotEqual(sourceModel.State, result.State);

        Assert.AreEqual(result.State, returnResult.State);
        Assert.AreEqual(result.Street, returnResult.Street);
        Assert.AreEqual(result.City, returnResult.City);
        Assert.AreEqual(result.BuildingNumber, returnResult.BuildingNumber);
        Assert.AreEqual(result.Country, returnResult.Country);
        Assert.AreEqual(result.Id, returnResult.Id);
    }

    [TestMethod]
    public void Should_Return_Items_Included_Deleted()
    {
        var entities = _rentalContext.People.ToList();

        var options = new TestQueryOptions
        {
            IncludeDeleted = true,
            IsReadOnly = true,
        };
        var models = _service.GetList<Person, PersonModel>(null, null, options);

        Assert.IsNotNull(models);
        Assert.IsInstanceOfType(models, typeof(List<PersonModel>));

        Assert.AreEqual(entities.Count, models.Count);

        foreach (var entity in entities)
        {
            var model = models.FirstOrDefault(x => x.Id == entity.Id);

            if (model != null)
            {
                Assert.AreEqual(entity.Id, model.Id);
                Assert.AreEqual(entity.FirstName + ' ' + entity.LastName, model.FullName);
                Assert.AreEqual(entity.Age, model.Age);
                Assert.AreEqual(entity.Gender, model.Gender);
                Assert.AreEqual(entity.Email, model.Email);
                Assert.AreEqual(entity.Phone, model.Phone);
                Assert.AreEqual(entity.IsDeleted, model.IsDeleted);
            }
        }
    }

    [TestMethod]
    public void Should_Return_Only_Not_Deleted_Items()
    {
        var entities = _rentalContext.People.ToList();

        var models = _service.GetList<Person, PersonModel>();

        Assert.IsNotNull(models);
        Assert.IsInstanceOfType(models, typeof(List<PersonModel>));

        Assert.AreEqual(entities.Count(x => !x.IsDeleted), models.Count);

        foreach (var entity in entities)
        {
            var model = models.FirstOrDefault(x => x.Id == entity.Id);

            if (model != null)
            {
                Assert.AreEqual(entity.Id, model.Id);
                Assert.AreEqual(entity.FirstName + ' ' + entity.LastName, model.FullName);
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
