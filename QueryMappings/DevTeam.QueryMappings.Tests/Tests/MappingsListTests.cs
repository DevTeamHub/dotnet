using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.QueryMappings.AspNetCore;
using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Helpers;
using DevTeam.QueryMappings.Mappings;
using DevTeam.QueryMappings.Properties;
using DevTeam.QueryMappings.Tests.Context.RentalContext;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Entities;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Mappings;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Mappings.Arguments;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Models;
using DevTeam.QueryMappings.Tests.Mappings;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DevTeam.QueryMappings.Tests.Tests
{
    [Category("MappingsList")]
    [TestOf(typeof(MappingsList))]
    [TestFixture]
    public class MappingsListTests
    {
        private ServiceProvider _serviceProvider;
        private IMappingsList _mappings;

        [OneTimeSetUp]
        public void Init()
        {
            var services = new ServiceCollection();
            
            services
                .AddDbContext<ISecurityContext, SecurityContext>()
                .AddDbContext<IRentalContext, RentalContext>()
                .AddQueryMappings();

            _serviceProvider = services.BuildServiceProvider();
            _mappings = _serviceProvider.GetRequiredService<IMappingsList>();

            MappingsConfiguration.Register(_mappings, typeof(AddressMappings).Assembly);
        }

        [OneTimeTearDown]
        public void Clear()
        {
            _serviceProvider = null;
            _mappings.Clear();
        }

        [Test]
        public void Entity_Framework_Context_Should_Be_Available()
        {
            var firstContext = _serviceProvider.GetRequiredService<IRentalContext>();
            var secondContext = _serviceProvider.GetRequiredService<ISecurityContext>();

            Assert.IsNotNull(firstContext);
            Assert.IsInstanceOf<RentalContext>(firstContext);

            Assert.IsNotNull(secondContext);
            Assert.IsInstanceOf<SecurityContext>(secondContext);
        }

        [Test]
        public void Shound_Find_Address_Expression_Mapping_In_Storage()
        {
            var mapping = _mappings.Get<Address, AddressModel>();

            Assert.IsNotNull(mapping);
            Assert.IsInstanceOf<ExpressionMapping<Address, AddressModel>>(mapping);
            Assert.AreEqual(mapping.From, typeof(Address));
            Assert.AreEqual(mapping.To, typeof(AddressModel));
            Assert.IsNull(mapping.Name);
        }

        [Test]
        public void Shound_Throw_Exception_If_Mapping_Doesnt_Exist()
        {
            var method = new TestDelegate(delegate { _mappings.Get<Address, BuildingModel>(); });
            var exceptionMessage = string.Format(Resources.MappingNotFoundException, typeof(Address).Name, typeof(BuildingModel).Name);

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        [Test]
        public void Shound_Throw_Exception_If_We_Try_To_Find_Named_Mapping_Without_Explicit_Name_Argument()
        {
            var method = new TestDelegate(delegate { _mappings.Get<Address, AddressSummaryModel>(); });
            var exceptionMessage = string.Format(Resources.NameIsNullWhenSearchForNamedMappingException, typeof(Address).Name, typeof(AddressSummaryModel).Name);

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        [Test]
        public void Shound_Throw_Exception_If_The_Same_Not_Named_Mapping_Registered_Twice()
        {
            var method = new TestDelegate(delegate { _mappings.Get<Address, InvalidAddressMapping>(); });
            var exceptionMessage = string.Format(Resources.MoreThanOneMappingFoundException, typeof(Address).Name, typeof(InvalidAddressMapping).Name);

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        [Test]
        public void Shound_Throw_Exception_If_Name_For_Named_Mapping_Is_Doesnt_Exist()
        {
            var method = new TestDelegate(delegate { _mappings.Get<Address, AddressSummaryModel>("SomeInvalidName"); });
            var exceptionMessage = string.Format(Resources.MappingNotFoundException, typeof(Address).Name, typeof(AddressSummaryModel).Name);

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        [Test]
        public void Shound_Find_Expression_Named_Mapping_By_Name()
        {
            var mappingName = MappingsNames.ExtendedAddressFormat;

            var namedMapping = _mappings.Get<Address, AddressSummaryModel>(mappingName);

            Assert.IsNotNull(namedMapping);
            Assert.IsInstanceOf<ExpressionMapping<Address, AddressSummaryModel>>(namedMapping);
            Assert.AreEqual(namedMapping.From, typeof(Address));
            Assert.AreEqual(namedMapping.To, typeof(AddressSummaryModel));
            Assert.AreEqual(namedMapping.Name, mappingName);
        }

        [Test]
        public void Shound_Find_Parameterized_Mapping_Without_Name()
        {
            var parameterizedMapping = _mappings.Get<Apartment, ApartmentShortModel>();

            Assert.IsNotNull(parameterizedMapping);
            Assert.IsInstanceOf<ParameterizedMapping<Apartment, ApartmentShortModel, ApartmentsArguments>>(parameterizedMapping);
            Assert.AreEqual(parameterizedMapping.From, typeof(Apartment));
            Assert.AreEqual(parameterizedMapping.To, typeof(ApartmentShortModel));
            Assert.IsNull(parameterizedMapping.Name);
        }

        [Test]
        public void Shound_Find_Parameterized_Mapping_By_Name()
        {
            var mappingName = MappingsNames.AppartmentsWithoutBuilding;

            var parameterizedMapping = _mappings.Get<Apartment, ApartmentModel>(mappingName);

            Assert.IsNotNull(parameterizedMapping);
            Assert.IsInstanceOf<ParameterizedMapping<Apartment, ApartmentModel, ApartmentsArguments>>(parameterizedMapping);
            Assert.AreEqual(parameterizedMapping.From, typeof(Apartment));
            Assert.AreEqual(parameterizedMapping.To, typeof(ApartmentModel));
            Assert.AreEqual(parameterizedMapping.Name, mappingName);
        }

        [Test]
        public void Shound_Find_Query_Mapping_Without_Name()
        {
            var queryMapping = _mappings.Get<Apartment, ApartmentReviewsModel>();

            Assert.IsNotNull(queryMapping);
            Assert.IsInstanceOf<QueryMapping<Apartment, ApartmentReviewsModel, IRentalContext>>(queryMapping);
            Assert.AreEqual(queryMapping.From, typeof(Apartment));
            Assert.AreEqual(queryMapping.To, typeof(ApartmentReviewsModel));
            Assert.IsNull(queryMapping.Name);
        }

        [Test]
        public void Shound_Find_Query_Mapping_By_Name()
        {
            var mappingName = MappingsNames.BuildingWithReviews;

            var queryMapping = _mappings.Get<Building, BuildingModel>(mappingName);

            Assert.IsNotNull(queryMapping);
            Assert.IsInstanceOf<QueryMapping<Building, BuildingModel, IRentalContext>>(queryMapping);
            Assert.AreEqual(queryMapping.From, typeof(Building));
            Assert.AreEqual(queryMapping.To, typeof(BuildingModel));
            Assert.AreEqual(queryMapping.Name, mappingName);
        }

        [Test]
        public void Shound_Find_Parameterized_Query_Mapping()
        {
            var mapping = _mappings.Get<Building, BuildingStatisticsModel>();

            Assert.IsNotNull(mapping);
            Assert.IsInstanceOf<ParameterizedQueryMapping<Building, BuildingStatisticsModel, BuildingArguments, IRentalContext>>(mapping);
            Assert.AreEqual(mapping.From, typeof(Building));
            Assert.AreEqual(mapping.To, typeof(BuildingStatisticsModel));
            Assert.IsNull(mapping.Name);
        }

        [Test]
        public void Shound_Exist_Address_Expression_Mapping_In_Storage()
        {
            var mappingExist = _mappings.Exist<Address, AddressModel>();

            Assert.IsTrue(mappingExist);
        }

        [Test]
        public void Shound_Return_False_If_Mapping_Doesnt_Exist()
        {
            var mappingExist = _mappings.Exist<Address, BuildingModel>();

            Assert.IsFalse(mappingExist);
        }

        [Test]
        public void Shound_Throw_Exception_If_We_Try_To_Check_Existence_Named_Mapping_Without_Explicit_Name_Argument()
        {
            var method = new TestDelegate(delegate { _mappings.Exist<Address, AddressSummaryModel>(); });
            var exceptionMessage = string.Format(Resources.NameIsNullWhenSearchForNamedMappingException, typeof(Address).Name, typeof(AddressSummaryModel).Name);

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        [Test]
        public void Shound_Throw_Exception_If_We_Try_To_Check_Existence_And_The_Same_Not_Named_Mapping_Registered_Twice()
        {
            var method = new TestDelegate(delegate { _mappings.Exist<Address, InvalidAddressMapping>(); });
            var exceptionMessage = string.Format(Resources.MoreThanOneMappingFoundException, typeof(Address).Name, typeof(InvalidAddressMapping).Name);

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        [Test]
        public void Shound_Throw_Exception_When_Check_Existence_If_Name_For_Named_Mapping_Is_Doesnt_Exist()
        {
            var mappingExist = _mappings.Exist<Address, AddressSummaryModel>("SomeInvalidName");

            Assert.False(mappingExist);
        }

        [Test]
        public void Shound_Exist_Expression_Named_Mapping_By_Name()
        {
            var mappingName = MappingsNames.ExtendedAddressFormat;

            var namedMappingExist = _mappings.Exist<Address, AddressSummaryModel>(mappingName);

            Assert.IsTrue(namedMappingExist);
        }

        [Test]
        public void Shound_Exist_Parameterized_Mapping_Without_Name()
        {
            var parameterizedMappingExist = _mappings.Exist<Apartment, ApartmentShortModel>();

            Assert.IsTrue(parameterizedMappingExist);
        }

        [Test]
        public void Shound_Exist_Parameterized_Mapping_By_Name()
        {
            var mappingName = MappingsNames.AppartmentsWithoutBuilding;

            var parameterizedMappingExist = _mappings.Exist<Apartment, ApartmentModel>(mappingName);

            Assert.IsTrue(parameterizedMappingExist);
        }

        [Test]
        public void Shound_Exist_Query_Mapping_Without_Name()
        {
            var queryMappingExist = _mappings.Exist<Apartment, ApartmentReviewsModel>();

            Assert.IsTrue(queryMappingExist);
        }

        [Test]
        public void Shound_Exist_Query_Mapping_By_Name()
        {
            var mappingName = MappingsNames.BuildingWithReviews;

            var queryMappingExist = _mappings.Exist<Building, BuildingModel>(mappingName);

            Assert.IsTrue(queryMappingExist);
        }

        [Test]
        public void Shound_Exist_Parameterized_Query_Mapping()
        {
            var mappingExist = _mappings.Exist<Building, BuildingStatisticsModel>();

            Assert.IsTrue(mappingExist);
        }
    }
}
