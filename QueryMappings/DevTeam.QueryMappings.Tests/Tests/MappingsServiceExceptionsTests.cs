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
using DevTeam.QueryMappings.Properties;
using DevTeam.QueryMappings.Base;

namespace DevTeam.QueryMappings.Tests.Tests
{
    [Category("MappingService")]
    [TestOf(typeof(MappingService))]
    [TestFixture]
    public class MappingServiceExceptionsTests
    {
        private IMappingService _service = null!;
        private RentalContext _context = null!;

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
            _context = null!;
            _service = null!;
        }

        /// <summary>
        /// Mapping from <see cref="Apartment"/> to <see cref="ApartmentShortModel"/> expects arguments. But user doesn't pass anything.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_If_Mapping_Requires_Arguments_But_Non_Has_Been_Passed()
        {
            var query = _context.Apartments.AsQueryable();
            var method = new TestDelegate(delegate { _service.Map<Apartment, ApartmentShortModel>(query); });
            var exceptionMessage = string.Format(Resources.InvalidCastMappingException, typeof(Apartment).Name, typeof(ApartmentShortModel).Name);
            exceptionMessage += Resources.ArgumentsHaventBeenPassed;

            var exception = Assert.Throws<MappingException>(method);
            Assert.IsNotNull(exception);
            Assert.AreEqual(exception!.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Apartment"/> to <see cref="ApartmentShortModel"/> expects arguments. But user doesn't pass anything.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_If_Object_Mapping_Requires_Arguments_But_Non_Has_Been_Passed()
        {
            var list = _context.Apartments.ToList();
            var method = new TestDelegate(delegate { _service.Map<Apartment, ApartmentShortModel>(list); });
            var exceptionMessage = string.Format(Resources.InvalidCastMappingException, typeof(Apartment).Name, typeof(ApartmentShortModel).Name);
            exceptionMessage += Resources.ArgumentsHaventBeenPassed;

            var exception = Assert.Throws<MappingException>(method);
            Assert.IsNotNull(exception);
            Assert.AreEqual(exception!.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Apartment"/> to <see cref="ApartmentShortModel"/> expects arguments of type <see cref="ApartmentsArguments"/>.
        /// User passes incorrect type as arguments.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_If_Mapping_Requires_Arguments_But_User_Passes_Arguments_Of_Another_Type()
        {
            var query = _context.Apartments.AsQueryable();
            var arguments = new BuildingArguments { TargetResidentsAge = 18 };
            var method = new TestDelegate(delegate { _service.Map<Apartment, ApartmentShortModel, BuildingArguments>(query, arguments); });
            var exceptionMessage = string.Format(Resources.ArgumentsOfIncorrectType, typeof(Apartment).Name, typeof(ApartmentShortModel).Name, typeof(ApartmentsArguments), typeof(BuildingArguments));

            var exception = Assert.Throws<MappingException>(method);
            Assert.IsNotNull(exception);
            Assert.AreEqual(exception!.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Apartment"/> to <see cref="ApartmentShortModel"/> expects arguments of type <see cref="ApartmentsArguments"/>.
        /// User passes incorrect type as arguments.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_If_Object_Mapping_Requires_Arguments_But_User_Passes_Arguments_Of_Another_Type()
        {
            var list = _context.Apartments.ToList();
            var arguments = new BuildingArguments { TargetResidentsAge = 18 };
            var method = new TestDelegate(delegate { _service.Map<Apartment, ApartmentShortModel, BuildingArguments>(list, arguments); });
            var exceptionMessage = string.Format(Resources.ArgumentsOfIncorrectType, typeof(Apartment).Name, typeof(ApartmentShortModel).Name, typeof(ApartmentsArguments), typeof(BuildingArguments));

            var exception = Assert.Throws<MappingException>(method);
            Assert.IsNotNull(exception);
            Assert.AreEqual(exception!.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Apartment"/> to <see cref="ApartmentReviewsModel"/> expects Database Context. 
        /// But user use non-generic version of <see cref="MappingService"/> and context isn't injected.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_When_Mapping_Expects_Database_Context_But_Its_Not_Injected()
        {
            var query = _context.Apartments.AsQueryable();
            var method = new TestDelegate(delegate { _service.Map<Apartment, ApartmentReviewsModel>(query); });
            var exceptionMessage = string.Format(Resources.InvalidCastMappingException, typeof(Apartment).Name, typeof(ApartmentReviewsModel).Name);
            exceptionMessage += Resources.ContextHaventBeenInjected;

            var exception = Assert.Throws<MappingException>(method);
            Assert.IsNotNull(exception);
            Assert.AreEqual(exception!.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Building"/> to <see cref="BuildingStatisticsModel"/> expects arguments and Database Context. 
        /// But user doesn't pass arguments and use non-generic version of <see cref="MappingService"/> and context isn't injected.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_When_Arguments_And_Context_Are_Required_But_Not_Provided()
        {
            var query = _context.Buildings.AsQueryable();
            var method = new TestDelegate(delegate { _service.Map<Building, BuildingStatisticsModel>(query); });
            var exceptionMessage = string.Format(Resources.InvalidCastMappingException, typeof(Building).Name, typeof(BuildingStatisticsModel).Name);
            exceptionMessage += Resources.ArgumentsHaventBeenPassed;
            exceptionMessage += Resources.ContextHaventBeenInjected;

            var exception = Assert.Throws<MappingException>(method);
            Assert.IsNotNull(exception);
            Assert.AreEqual(exception!.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Address"/> to <see cref="AddressModel"/> doesn't require arguments. 
        /// But user passes arguments anyway.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_When_Mapping_Doesnt_Expect_Arguments_But_They_Passed_Anyway()
        {
            var query = _context.Addresses.AsQueryable();
            var arguments = new BuildingArguments { TargetResidentsAge = 18 };
            var method = new TestDelegate(delegate { _service.Map<Address, AddressModel, BuildingArguments>(query, arguments); });
            var exceptionMessage = string.Format(Resources.InvalidCastMappingException, typeof(Address).Name, typeof(AddressModel).Name);
            exceptionMessage += Resources.ArgumentsAreNotNeeded;

            var exception = Assert.Throws<MappingException>(method);
            Assert.IsNotNull(exception);
            Assert.AreEqual(exception!.Message, exceptionMessage);
        }

        /// <summary>
        /// Object mapping from <see cref="Address"/> to <see cref="AddressModel"/> doesn't require arguments. 
        /// But user passes arguments anyway.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_When_Object_Mapping_Doesnt_Expect_Arguments_But_They_Passed_Anyway()
        {
            var list = _context.Addresses.ToList();
            var arguments = new BuildingArguments { TargetResidentsAge = 18 };
            var method = new TestDelegate(delegate { _service.Map<Address, AddressModel, BuildingArguments>(list, arguments); });
            var exceptionMessage = string.Format(Resources.InvalidCastMappingException, typeof(Address).Name, typeof(AddressModel).Name);
            exceptionMessage += Resources.ArgumentsAreNotNeeded;

            var exception = Assert.Throws<MappingException>(method);
            Assert.IsNotNull(exception);
            Assert.AreEqual(exception!.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Building"/> to <see cref="BuildingStatisticsModel"/> expects arguments and Database Context. 
        /// User passes arguments, but use non-generic version of <see cref="MappingService"/> and context isn't injected.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_When_Arguments_And_Context_Are_Required_But_Only_Arguments_Provided()
        {
            var query = _context.Buildings.AsQueryable();
            var arguments = new BuildingArguments { TargetResidentsAge = 18 };
            var method = new TestDelegate(delegate { _service.Map<Building, BuildingStatisticsModel, BuildingArguments>(query, arguments); });
            var exceptionMessage = string.Format(Resources.InvalidCastMappingException, typeof(Building).Name, typeof(BuildingStatisticsModel).Name);
            exceptionMessage += Resources.ContextHaventBeenInjected;

            var exception = Assert.Throws<MappingException>(method);
            Assert.IsNotNull(exception);
            Assert.AreEqual(exception!.Message, exceptionMessage);
        }
    }
}
