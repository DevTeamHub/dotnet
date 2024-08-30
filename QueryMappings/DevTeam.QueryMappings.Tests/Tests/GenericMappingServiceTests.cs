using DevTeam.QueryMappings.Helpers;
using DevTeam.QueryMappings.Tests.Mappings;
using DevTeam.QueryMappings.Tests.Context.RentalContext;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Mappings;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Entities;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Models;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Mappings.Arguments;
using Microsoft.Extensions.DependencyInjection;
using DevTeam.QueryMappings.AspNetCore;
using DevTeam.QueryMappings.Services.Interfaces;

namespace DevTeam.QueryMappings.Tests;

[TestCategory("GenericMappingService")]
[TestClass]
public class GenericMappingServiceTests
{
    private static IMappingService<IRentalContext> _service = null!;
    private static RentalContext _context = null!;

    [ClassInitialize]
    public static void Init(TestContext testContext)
    {
        var services = new ServiceCollection();

        services
            .AddDbContext<ISecurityContext, SecurityContext>()
            .AddDbContext<IRentalContext, RentalContext>()
            .AddQueryMappings();

        var serviceProvider = services.BuildServiceProvider();
        var mappings = serviceProvider.GetRequiredService<IMappingsList>();
        _context = (RentalContext)serviceProvider.GetRequiredService<IRentalContext>();
        _service = serviceProvider.GetRequiredService<IMappingService<IRentalContext>>();

        MappingsConfiguration.Register(mappings, typeof(AddressMappings).Assembly);
    }

    [ClassCleanup]
    public static void Clear()
    {
        _context = null!;
        _service = null!;
    }

    [TestMethod]
    public void Should_Convert_Address_Query_Into_Models()
    {
        var entities = _context.Addresses.ToList();
        var query = entities.AsQueryable();

        var modelsQuery = _service.Map<Address, AddressModel>(query);

        Assert.IsNotNull(modelsQuery);
        Assert.IsInstanceOfType(modelsQuery, typeof(IQueryable<AddressModel>));

        var models = modelsQuery.ToList();

        Assert.AreEqual(entities.Count, models.Count);

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            var model = models[i];

            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.State, entity.State);
            Assert.AreEqual(model.Street, entity.Street);
            Assert.AreEqual(model.ZipCode, entity.ZipCode);
            Assert.AreEqual(model.Country, (Countries)entity.Country);
            Assert.AreEqual(model.BuildingNumber, entity.BuildingNumber);
            Assert.AreEqual(model.City, entity.City);
        }
    }

    [TestMethod]
    public void Should_Convert_Address_Item_Into_Model_Object()
    {
        var entity = _context.Addresses.First();

        var model = _service.Map<Address, AddressModel>(entity);

        Assert.IsNotNull(model);
        Assert.IsInstanceOfType(model, typeof(AddressModel));

        Assert.AreEqual(model.Id, entity.Id);
        Assert.AreEqual(model.State, entity.State);
        Assert.AreEqual(model.Street, entity.Street);
        Assert.AreEqual(model.ZipCode, entity.ZipCode);
        Assert.AreEqual(model.Country, (Countries)entity.Country);
        Assert.AreEqual(model.BuildingNumber, entity.BuildingNumber);
        Assert.AreEqual(model.City, entity.City);
    }

    [TestMethod]
    public void Should_Convert_Addresses_List_Into_Model_Objects()
    {
        var entities = _context.Addresses.ToList();

        var models = _service.Map<Address, AddressModel>(entities).ToList();

        Assert.IsNotNull(models);
        Assert.IsInstanceOfType(models, typeof(List<AddressModel>));

        Assert.AreEqual(entities.Count, models.Count);

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            var model = models[i];

            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.State, entity.State);
            Assert.AreEqual(model.Street, entity.Street);
            Assert.AreEqual(model.ZipCode, entity.ZipCode);
            Assert.AreEqual(model.Country, (Countries)entity.Country);
            Assert.AreEqual(model.BuildingNumber, entity.BuildingNumber);
            Assert.AreEqual(model.City, entity.City);
        }
    }

    [TestMethod]
    public void Should_Convert_Address_Query_Into_Short_Model_Using_Named_Mappings()
    {
        var entities = _context.Addresses.ToList();
        var query = entities.AsQueryable();

        var shortModelsQuery = _service.Map<Address, AddressSummaryModel>(query, MappingsNames.ShortAddressFormat);

        Assert.IsNotNull(shortModelsQuery);
        Assert.IsInstanceOfType(shortModelsQuery, typeof(IQueryable<AddressSummaryModel>));

        var extendedModelsQuery = _service.Map<Address, AddressSummaryModel>(query, MappingsNames.ExtendedAddressFormat);

        Assert.IsNotNull(extendedModelsQuery);
        Assert.IsInstanceOfType(extendedModelsQuery, typeof(IQueryable<AddressSummaryModel>));

        var shortModels = shortModelsQuery.ToList();

        Assert.AreEqual(entities.Count, shortModels.Count);

        var extendedModels = extendedModelsQuery.ToList();

        Assert.AreEqual(entities.Count, extendedModels.Count);

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            var shortModel = shortModels[i];
            var extendedModel = extendedModels[i];

            Assert.AreEqual(shortModel.Id, entity.Id);
            Assert.AreEqual(shortModel.Address, entity.BuildingNumber + " " + entity.Street + ", " + entity.City);

            Assert.AreEqual(extendedModel.Id, entity.Id);
            Assert.AreEqual(extendedModel.Address, entity.BuildingNumber + " " + entity.Street + ", " + entity.City + ", " + entity.State + ", " + entity.Country + ", " + entity.ZipCode);
        }
    }

    [TestMethod]
    public void Should_Convert_Address_Item_Into_Short_Model_Using_Named_Mappings()
    {
        var entity = _context.Addresses.First();

        var shortModel = _service.Map<Address, AddressSummaryModel>(entity, MappingsNames.ShortAddressFormat);

        Assert.IsNotNull(shortModel);
        Assert.IsInstanceOfType(shortModel, typeof(AddressSummaryModel));

        var extendedModel = _service.Map<Address, AddressSummaryModel>(entity, MappingsNames.ExtendedAddressFormat);

        Assert.IsNotNull(extendedModel);
        Assert.IsInstanceOfType(extendedModel, typeof(AddressSummaryModel));

        Assert.AreEqual(shortModel.Id, entity.Id);
        Assert.AreEqual(shortModel.Address, entity.BuildingNumber + " " + entity.Street + ", " + entity.City);

        Assert.AreEqual(extendedModel.Id, entity.Id);
        Assert.AreEqual(extendedModel.Address, entity.BuildingNumber + " " + entity.Street + ", " + entity.City + ", " + entity.State + ", " + entity.Country + ", " + entity.ZipCode);
    }

    [TestMethod]
    public void Should_Convert_Addresses_List_Into_Short_Models_List_Using_Named_Mappings()
    {
        var entities = _context.Addresses.ToList();

        var shortModelsQuery = _service.Map<Address, AddressSummaryModel>(entities, MappingsNames.ShortAddressFormat).ToList();

        Assert.IsNotNull(shortModelsQuery);
        Assert.IsInstanceOfType(shortModelsQuery, typeof(List<AddressSummaryModel>));

        var extendedModelsQuery = _service.Map<Address, AddressSummaryModel>(entities, MappingsNames.ExtendedAddressFormat).ToList();

        Assert.IsNotNull(extendedModelsQuery);
        Assert.IsInstanceOfType(extendedModelsQuery, typeof(List<AddressSummaryModel>));

        var shortModels = shortModelsQuery.ToList();

        Assert.AreEqual(entities.Count, shortModels.Count);

        var extendedModels = extendedModelsQuery.ToList();

        Assert.AreEqual(entities.Count, extendedModels.Count);

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            var shortModel = shortModels[i];
            var extendedModel = extendedModels[i];

            Assert.AreEqual(shortModel.Id, entity.Id);
            Assert.AreEqual(shortModel.Address, entity.BuildingNumber + " " + entity.Street + ", " + entity.City);

            Assert.AreEqual(extendedModel.Id, entity.Id);
            Assert.AreEqual(extendedModel.Address, entity.BuildingNumber + " " + entity.Street + ", " + entity.City + ", " + entity.State + ", " + entity.Country + ", " + entity.ZipCode);
        }
    }

    [TestMethod]
    public void Should_Convert_Appartments_Query_Into_Model_And_Apply_Arguments()
    {
        var entities = _context.Apartments.ToList();
        var query = entities.AsQueryable();

        var arguments = new ApartmentsArguments { UnitOfMeasure = "sq ft" };
        var modelsQuery = _service.Map<Apartment, ApartmentShortModel, ApartmentsArguments>(query, arguments);

        Assert.IsNotNull(modelsQuery);
        Assert.IsInstanceOfType(modelsQuery, typeof(IQueryable<ApartmentShortModel>));

        var models = modelsQuery.ToList();

        Assert.AreEqual(entities.Count, models.Count);

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            var model = models[i];

            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Floor, entity.Floor);
            Assert.AreEqual(model.IsLodge, entity.IsLodge);
            Assert.AreEqual(model.Size, entity.Size.ToString() + arguments.UnitOfMeasure);
            Assert.AreEqual(model.Number, entity.Number);
        }
    }

    [TestMethod]
    public void Should_Convert_Appartments_List_Into_Model_And_Apply_Arguments()
    {
        var entities = _context.Apartments.ToList();

        var arguments = new ApartmentsArguments { UnitOfMeasure = "sq ft" };
        var models = _service.Map<Apartment, ApartmentShortModel, ApartmentsArguments>(entities, arguments).ToList();

        Assert.IsNotNull(models);
        Assert.IsInstanceOfType(models, typeof(List<ApartmentShortModel>));

        Assert.AreEqual(entities.Count, models.Count);

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            var model = models[i];

            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Floor, entity.Floor);
            Assert.AreEqual(model.IsLodge, entity.IsLodge);
            Assert.AreEqual(model.Size, entity.Size.ToString() + arguments.UnitOfMeasure);
            Assert.AreEqual(model.Number, entity.Number);
        }
    }

    [TestMethod]
    public void Should_Convert_Appartments_Query_Into_Model_And_Apply_Arguments_Using_Named_Mapping()
    {
        var entities = _context.Apartments.ToList();
        var query = entities.AsQueryable();

        var arguments = new ApartmentsArguments { UnitOfMeasure = "sq meters" };
        var modelsQuery = _service.Map<Apartment, ApartmentModel, ApartmentsArguments>(query, arguments, MappingsNames.AppartmentsWithBuilding);

        Assert.IsNotNull(modelsQuery);
        Assert.IsInstanceOfType(modelsQuery, typeof(IQueryable<ApartmentModel>));

        var models = modelsQuery.ToList();

        Assert.AreEqual(entities.Count, models.Count);

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            var model = models[i];

            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Floor, entity.Floor);
            Assert.AreEqual(model.IsLodge, entity.IsLodge);
            Assert.AreEqual(model.Size, entity.Size.ToString() + arguments.UnitOfMeasure);
            Assert.AreEqual(model.Number, entity.Number);
            Assert.AreEqual(model.Badrooms, entity.Badrooms);
            Assert.AreEqual(model.Bathrooms, entity.Bathrooms);

            Assert.IsNotNull(model.Building);

            var building = _context.Buildings.FirstOrDefault(x => x.Id == model.Building.Id);

            Assert.IsNotNull(building);
            Assert.AreEqual(model.Building.Id, building!.Id);
            Assert.AreEqual(model.Building.Year, building.Year);
            Assert.AreEqual(model.Building.Floors, building.Floors);
            Assert.AreEqual(model.Building.IsLaundry, building.IsLaundry);
            Assert.AreEqual(model.Building.IsParking, building.IsParking);
        }
    }

    [TestMethod]
    public void Should_Convert_Appartments_List_Into_Model_And_Apply_Arguments_Using_Named_Mapping()
    {
        var entities = _context.Apartments.ToList();

        var arguments = new ApartmentsArguments { UnitOfMeasure = "sq meters" };
        var models = _service.Map<Apartment, ApartmentModel, ApartmentsArguments>(entities, arguments, MappingsNames.AppartmentsWithBuilding).ToList();

        Assert.IsNotNull(models);
        Assert.IsInstanceOfType(models, typeof(List<ApartmentModel>));

        Assert.AreEqual(entities.Count, models.Count);

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            var model = models[i];

            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Floor, entity.Floor);
            Assert.AreEqual(model.IsLodge, entity.IsLodge);
            Assert.AreEqual(model.Size, entity.Size.ToString() + arguments.UnitOfMeasure);
            Assert.AreEqual(model.Number, entity.Number);
            Assert.AreEqual(model.Badrooms, entity.Badrooms);
            Assert.AreEqual(model.Bathrooms, entity.Bathrooms);

            Assert.IsNotNull(model.Building);

            var building = _context.Buildings.FirstOrDefault(x => x.Id == model.Building.Id);

            Assert.IsNotNull(building);
            Assert.AreEqual(model.Building.Id, building!.Id);
            Assert.AreEqual(model.Building.Year, building.Year);
            Assert.AreEqual(model.Building.Floors, building.Floors);
            Assert.AreEqual(model.Building.IsLaundry, building.IsLaundry);
            Assert.AreEqual(model.Building.IsParking, building.IsParking);
        }
    }

    [TestMethod]
    public void Should_Convert_Appartments_Into_Model_And_Attach_Reviews_Info_Using_EF_Context()
    {
        var entities = _context.Apartments.ToList();
        var query = entities.AsQueryable();

        var modelsQuery = _service.Map<Apartment, ApartmentReviewsModel>(query);

        Assert.IsNotNull(modelsQuery);
        Assert.IsInstanceOfType(modelsQuery, typeof(IQueryable<ApartmentReviewsModel>));

        var models = modelsQuery.ToList();

        Assert.AreEqual(entities.Count, models.Count);

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            var model = models[i];

            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Number, entity.Number);

            Assert.IsNotNull(model.Reviews);

            var reviews = _context.Reviews.Where(x => x.EntityId == entity.Id && x.EntityTypeId == (int)EntityType.Apartment).ToList();

            Assert.AreEqual(model.Reviews.Count, reviews.Count);

            for (var j = 0; j < model.Reviews.Count; j++)
            {
                var review = reviews[j];
                var reviewModel = model.Reviews[j];

                Assert.AreEqual(reviewModel.Id, review.Id);
                Assert.AreEqual(reviewModel.EntityId, review.EntityId);
                Assert.AreEqual(reviewModel.EntityType, (EntityType)review.EntityTypeId);
                Assert.AreEqual(reviewModel.Rating, review.Rating);
                Assert.AreEqual(reviewModel.Comments, review.Comments);
            }
        }
    }

    [TestMethod]
    public void Should_Convert_Building_Into_Model_Using_Named_Mapping_And_Attach_Reviews_Info_Using_EF_Context()
    {
        var entities = _context.Buildings.ToList();
        var query = entities.AsQueryable();

        var modelsQuery = _service.Map<Building, BuildingModel>(query, MappingsNames.BuildingWithReviews);

        Assert.IsNotNull(modelsQuery);
        Assert.IsInstanceOfType(modelsQuery, typeof(IQueryable<BuildingModel>));

        var models = modelsQuery.ToList();

        Assert.AreEqual(entities.Count, models.Count);

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            var model = models[i];

            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Year, entity.Year);
            Assert.AreEqual(model.Floors, entity.Floors);
            Assert.AreEqual(model.IsLaundry, entity.IsLaundry);
            Assert.AreEqual(model.IsParking, entity.IsParking);

            Assert.IsNotNull(model.Reviews);
            Assert.IsNotNull(model.Address);
            Assert.IsNotNull(model.Appartments);

            var address = _context.Addresses.FirstOrDefault(x => x.Id == model.Address.Id);

            Assert.IsNotNull(address);

            Assert.AreEqual(model.Address.Id, address!.Id);
            Assert.AreEqual(model.Address.State, address.State);
            Assert.AreEqual(model.Address.Street, address.Street);
            Assert.AreEqual(model.Address.ZipCode, address.ZipCode);
            Assert.AreEqual(model.Address.Country, (Countries)address.Country);
            Assert.AreEqual(model.Address.BuildingNumber, address.BuildingNumber);
            Assert.AreEqual(model.Address.City, address.City);

            var apartments = _context.Apartments.Where(x => x.BuildingId == model.Id).ToList();

            Assert.IsNotNull(apartments);
            Assert.AreEqual(apartments.Count, model.Appartments.Count);

            for (var k = 0; k < apartments.Count; k++)
            {
                var apartment = apartments[k];
                var apartmentModel = model.Appartments[k];

                Assert.AreEqual(apartmentModel.Id, apartment.Id);
                Assert.AreEqual(apartmentModel.Floor, apartment.Floor);
                Assert.AreEqual(apartmentModel.IsLodge, apartment.IsLodge);
                Assert.AreEqual(apartmentModel.Size, apartment.Size.ToString());
                Assert.AreEqual(apartmentModel.Number, apartment.Number);
            }

            var reviews = _context.Reviews.Where(x => x.EntityId == entity.Id && x.EntityTypeId == (int)EntityType.Building).ToList();

            Assert.AreEqual(model.Reviews.Count, reviews.Count);

            for (var j = 0; j < model.Reviews.Count; j++)
            {
                var review = reviews[j];
                var reviewModel = model.Reviews[j];

                Assert.AreEqual(reviewModel.Id, review.Id);
                Assert.AreEqual(reviewModel.EntityId, review.EntityId);
                Assert.AreEqual(reviewModel.EntityType, (EntityType)review.EntityTypeId);
                Assert.AreEqual(reviewModel.Rating, review.Rating);
                Assert.AreEqual(reviewModel.Comments, review.Comments);
            }
        }
    }

    [TestMethod]
    public void Should_Get_Budilding_Statistics_Using_Arguments_And_EF_Context()
    {
        var entities = _context.Buildings.ToList();
        var query = entities.AsQueryable();

        var arguments = new BuildingArguments { TargetResidentsAge = 18 };
        var modelsQuery = _service.Map<Building, BuildingStatisticsModel, BuildingArguments>(query, arguments);

        Assert.IsNotNull(modelsQuery);
        Assert.IsInstanceOfType(modelsQuery, typeof(IQueryable<BuildingStatisticsModel>));

        var models = modelsQuery.ToList();

        Assert.AreEqual(entities.Count, models.Count);

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            var model = models[i];

            var reviews = _context.Reviews.Where(x => x.EntityId == entity.Id && x.EntityTypeId == (int)EntityType.Building).ToList();

            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Address, entity.Address.BuildingNumber + ", " + entity.Address.Street + ", " + entity.Address.City);
            Assert.AreEqual(model.AppartmentsCount, entity.Appartments.Count);
            Assert.AreEqual(model.Size, entity.Appartments.Sum(app => app.Size));
            Assert.AreEqual(model.ResidentsCount, entity.Appartments.SelectMany(app => app.Residents).Where(r => r.Age > arguments.TargetResidentsAge).Count());
            Assert.AreEqual(model.AverageBuildingRating, reviews.Average(r => r.Rating));
        }
    }
}
