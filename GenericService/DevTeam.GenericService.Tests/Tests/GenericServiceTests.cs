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
    private static IGenericService _service = null!;
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
            .AddGenericServices();

        _serviceProvider = services.BuildServiceProvider();

        _rentalContext = (RentalContext)_serviceProvider.GetRequiredService<IRentalContext>();
        _service = _serviceProvider.GetRequiredService<IGenericService>();
        _repository = _serviceProvider.GetRequiredService<IRepository>();
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

        Assert.AreEqual(sourceModel.Street, result.Street);
        Assert.AreNotEqual(sourceModel.State, result.State);
        //Assert.AreEqual(returnResult, entityResult);
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

        Assert.AreEqual(sourceModel.Street, result.Street);
        Assert.AreNotEqual(sourceModel.State, result.State);

        Assert.AreEqual(result.State, returnResult.State);
        Assert.AreEqual(result.Street, returnResult.Street);
        Assert.AreEqual(result.City, returnResult.City);
        Assert.AreEqual(result.BuildingNumber, returnResult.BuildingNumber);
        Assert.AreEqual(result.Country, returnResult.Country);
        Assert.AreEqual(result.Id, returnResult.Id);
    }
}
