using DevTeam.QueryMappings.Helpers;
using NUnit.Framework;
using DevTeam.QueryMappings.Tests.Mappings;
using DevTeam.QueryMappings.Tests.Context.RentalContext;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Mappings;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Entities;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Models;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Mappings.Arguments;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using DevTeam.QueryMappings.AspNetCore;
using DevTeam.QueryMappings.Services.Interfaces;
using DevTeam.QueryMappings.Services.Implementations;
using System.Collections.Generic;

namespace DevTeam.QueryMappings.Tests.Tests
{
    [Category("MappingService")]
    [TestOf(typeof(MappingService))]
    [TestFixture]
    public class MappingServiceTests
    {
        private IMappingService _service;
        private RentalContext _context;

        [OneTimeSetUp]
        public void Init()
        {
            var services = new ServiceCollection();

            services
                .AddDbContext<ISecurityContext, SecurityContext>()
                .AddDbContext<IRentalContext, RentalContext>()
                .AddQueryMappings();

            var serviceProvider = services.BuildServiceProvider();
            var mappings = serviceProvider.GetRequiredService<IMappingsList>();
            _context = (RentalContext)serviceProvider.GetRequiredService<IRentalContext>();
            _service = serviceProvider.GetRequiredService<IMappingService>();

            MappingsConfiguration.Register(mappings, typeof(AddressMappings).Assembly);
        }

        [OneTimeTearDown]
        public void Clear()
        {
            _context = null;
            _service = null;
        }

        [Test]
        public void Should_Convert_Address_Query_Into_Models()
        {
            var entities = _context.Addresses.ToList();
            var query = entities.AsQueryable();

            var modelsQuery = _service.Map<Address, AddressModel>(query);

            Assert.IsNotNull(modelsQuery);
            Assert.IsInstanceOf<IQueryable<AddressModel>>(modelsQuery);

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

        [Test]
        public void Should_Convert_Address_Item_Into_Model_Object()
        {
            var entity = _context.Addresses.First();

            var model = _service.Map<Address, AddressModel>(entity);

            Assert.IsNotNull(model);
            Assert.IsInstanceOf<AddressModel>(model);

            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.State, entity.State);
            Assert.AreEqual(model.Street, entity.Street);
            Assert.AreEqual(model.ZipCode, entity.ZipCode);
            Assert.AreEqual(model.Country, (Countries)entity.Country);
            Assert.AreEqual(model.BuildingNumber, entity.BuildingNumber);
            Assert.AreEqual(model.City, entity.City);
        }

        [Test]
        public void Should_Convert_Addresses_List_Into_Model_Objects()
        {
            var entities = _context.Addresses.ToList();

            var models = _service.Map<Address, AddressModel>(entities);

            Assert.IsNotNull(models);
            Assert.IsInstanceOf<List<AddressModel>>(models);

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

        [Test]
        public void Should_Convert_Address_Query_Into_Short_Model_Using_Named_Mappings()
        {
            var entities = _context.Addresses.ToList();
            var query = entities.AsQueryable();

            var shortModelsQuery = _service.Map<Address, AddressSummaryModel>(query, MappingsNames.ShortAddressFormat);

            Assert.IsNotNull(shortModelsQuery);
            Assert.IsInstanceOf<IQueryable<AddressSummaryModel>>(shortModelsQuery);

            var extendedModelsQuery = _service.Map<Address, AddressSummaryModel>(query, MappingsNames.ExtendedAddressFormat);

            Assert.IsNotNull(extendedModelsQuery);
            Assert.IsInstanceOf<IQueryable<AddressSummaryModel>>(extendedModelsQuery);

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

        [Test]
        public void Should_Convert_Address_Item_Into_Short_Model_Using_Named_Mappings()
        {
            var entity = _context.Addresses.First();

            var shortModel = _service.Map<Address, AddressSummaryModel>(entity, MappingsNames.ShortAddressFormat);

            Assert.IsNotNull(shortModel);
            Assert.IsInstanceOf<AddressSummaryModel>(shortModel);

            var extendedModel = _service.Map<Address, AddressSummaryModel>(entity, MappingsNames.ExtendedAddressFormat);

            Assert.IsNotNull(extendedModel);
            Assert.IsInstanceOf<AddressSummaryModel>(extendedModel);

            Assert.AreEqual(shortModel.Id, entity.Id);
            Assert.AreEqual(shortModel.Address, entity.BuildingNumber + " " + entity.Street + ", " + entity.City);

            Assert.AreEqual(extendedModel.Id, entity.Id);
            Assert.AreEqual(extendedModel.Address, entity.BuildingNumber + " " + entity.Street + ", " + entity.City + ", " + entity.State + ", " + entity.Country + ", " + entity.ZipCode);
        }

        [Test]
        public void Should_Convert_Addresses_List_Into_Short_Models_List_Using_Named_Mappings()
        {
            var entities = _context.Addresses.ToList();

            var shortModelsQuery = _service.Map<Address, AddressSummaryModel>(entities, MappingsNames.ShortAddressFormat);

            Assert.IsNotNull(shortModelsQuery);
            Assert.IsInstanceOf<List<AddressSummaryModel>>(shortModelsQuery);

            var extendedModelsQuery = _service.Map<Address, AddressSummaryModel>(entities, MappingsNames.ExtendedAddressFormat);

            Assert.IsNotNull(extendedModelsQuery);
            Assert.IsInstanceOf<List<AddressSummaryModel>>(extendedModelsQuery);

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

        [Test]
        public void Should_Convert_Appartments_Query_Into_Model_And_Apply_Arguments()
        {
            var entities = _context.Apartments.ToList();
            var query = entities.AsQueryable();

            var arguments = new ApartmentsArguments { UnitOfMeasure = "sq ft" };
            var modelsQuery = _service.Map<Apartment, ApartmentShortModel, ApartmentsArguments>(query, arguments);

            Assert.IsNotNull(modelsQuery);
            Assert.IsInstanceOf<IQueryable<ApartmentShortModel>>(modelsQuery);

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

        [Test]
        public void Should_Convert_Appartments_List_Into_Model_And_Apply_Arguments()
        {
            var entities = _context.Apartments.ToList();

            var arguments = new ApartmentsArguments { UnitOfMeasure = "sq ft" };
            var models = _service.Map<Apartment, ApartmentShortModel, ApartmentsArguments>(entities, arguments);

            Assert.IsNotNull(models);
            Assert.IsInstanceOf<List<ApartmentShortModel>>(models);

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

        [Test]
        public void Should_Convert_Appartments_Query_Into_Model_And_Apply_Arguments_Using_Named_Mapping()
        {
            var entities = _context.Apartments.ToList();
            var query = entities.AsQueryable();

            var arguments = new ApartmentsArguments { UnitOfMeasure = "sq meters" };
            var modelsQuery = _service.Map<Apartment, ApartmentModel, ApartmentsArguments>(query, arguments, MappingsNames.AppartmentsWithBuilding);

            Assert.IsNotNull(modelsQuery);
            Assert.IsInstanceOf<IQueryable<ApartmentModel>>(modelsQuery);

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
                Assert.AreEqual(model.Building.Id, building.Id);
                Assert.AreEqual(model.Building.Year, building.Year);
                Assert.AreEqual(model.Building.Floors, building.Floors);
                Assert.AreEqual(model.Building.IsLaundry, building.IsLaundry);
                Assert.AreEqual(model.Building.IsParking, building.IsParking);
            }
        }

        [Test]
        public void Should_Convert_Appartments_List_Into_Model_And_Apply_Arguments_Using_Named_Mapping()
        {
            var entities = _context.Apartments.ToList();

            var arguments = new ApartmentsArguments { UnitOfMeasure = "sq meters" };
            var models = _service.Map<Apartment, ApartmentModel, ApartmentsArguments>(entities, arguments, MappingsNames.AppartmentsWithBuilding);

            Assert.IsNotNull(models);
            Assert.IsInstanceOf<List<ApartmentModel>>(models);

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
                Assert.AreEqual(model.Building.Id, building.Id);
                Assert.AreEqual(model.Building.Year, building.Year);
                Assert.AreEqual(model.Building.Floors, building.Floors);
                Assert.AreEqual(model.Building.IsLaundry, building.IsLaundry);
                Assert.AreEqual(model.Building.IsParking, building.IsParking);
            }
        }
    }
}
