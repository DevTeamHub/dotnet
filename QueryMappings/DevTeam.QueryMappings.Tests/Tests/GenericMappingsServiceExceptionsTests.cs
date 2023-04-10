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
    [Category("GenericMappingService")]
    [TestOf(typeof(MappingService<>))]
    [TestFixture]
    public class GenericMappingServiceExceptionsTests
    {
        private IMappingService<IRentalContext> _service;
        private IMappingService<ISecurityContext> _securityService;
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
            _service = serviceProvider.GetRequiredService<IMappingService<IRentalContext>>();
            _securityService = serviceProvider.GetRequiredService<IMappingService<ISecurityContext>>();

            MappingsConfiguration.Register(mappings, typeof(AddressMappings).Assembly);
        }

        [OneTimeTearDown]
        public void Clear()
        {
            _context = null;
            _service = null;
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
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        /// <summary>
        /// Object mapping from <see cref="Apartment"/> to <see cref="ApartmentShortModel"/> expects arguments. But user doesn't pass anything.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_If_Object_Mapping_Requires_Arguments_But_Non_Has_Been_Passed()
        {
            var list = _context.Apartments.ToList();
            var method = new TestDelegate(delegate { _service.Map<Apartment, ApartmentShortModel>(list); });
            var exceptionMessage = string.Format(Resources.InvalidCastMappingException, typeof(Apartment).Name, typeof(ApartmentShortModel).Name);
            exceptionMessage += Resources.ArgumentsHaventBeenPassed;

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
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
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        /// <summary>
        /// Object mapping from <see cref="Apartment"/> to <see cref="ApartmentShortModel"/> expects arguments of type <see cref="ApartmentsArguments"/>.
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
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Apartment"/> to <see cref="ApartmentReviewsModel"/> expects Database Context. 
        /// User uses generic version of <see cref="MappingService{TContext}"/>, but with incorrect interface of Database Context.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_When_Mapping_Expects_Database_Context_Of_One_Type_But_Another_Type_Provided()
        {
            var query = _context.Apartments.AsQueryable();
            var method = new TestDelegate(delegate { _securityService.Map<Apartment, ApartmentReviewsModel>(query); });
            var exceptionMessage = string.Format(Resources.ContextOfIncorrectType, typeof(Apartment).Name, typeof(ApartmentReviewsModel).Name, typeof(IRentalContext), typeof(ISecurityContext));

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Building"/> to <see cref="BuildingStatisticsModel"/> expects arguments and Database Context. 
        /// But user doesn't pass arguments.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_When_Arguments_And_Context_Are_Required_But_Not_Provided()
        {
            var query = _context.Buildings.AsQueryable();
            var method = new TestDelegate(delegate { _service.Map<Building, BuildingStatisticsModel>(query); });
            var exceptionMessage = string.Format(Resources.InvalidCastMappingException, typeof(Building).Name, typeof(BuildingStatisticsModel).Name);
            exceptionMessage += Resources.ArgumentsHaventBeenPassed;

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
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
            Assert.AreEqual(exception.Message, exceptionMessage);
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
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Building"/> to <see cref="BuildingStatisticsModel"/> expects arguments and Database Context. 
        /// User uses correct version of <see cref="MappingService{TContext}"/>, but doesn't provide arguments.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_When_Arguments_And_Context_Are_Required_But_Only_Context_Provided()
        {
            var query = _context.Buildings.AsQueryable();
            var method = new TestDelegate(delegate { _service.Map<Building, BuildingStatisticsModel>(query); });
            var exceptionMessage = string.Format(Resources.InvalidCastMappingException, typeof(Building).Name, typeof(BuildingStatisticsModel).Name);
            exceptionMessage += Resources.ArgumentsHaventBeenPassed;

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Building"/> to <see cref="BuildingStatisticsModel"/> expects arguments and Database Context. 
        /// User passes arguments, but use incorrect implementation of <see cref="MappingService{TContext}"/>.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_When_Arguments_And_Context_Are_Required_But_Context_Is_Incorrect_Type()
        {
            var query = _context.Buildings.AsQueryable();
            var arguments = new BuildingArguments { TargetResidentsAge = 18 };
            var method = new TestDelegate(delegate { _securityService.Map<Building, BuildingStatisticsModel, BuildingArguments>(query, arguments); });
            var exceptionMessage = string.Format(Resources.ContextOfIncorrectType, typeof(Building).Name, typeof(BuildingStatisticsModel).Name, typeof(IRentalContext), typeof(ISecurityContext));

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Building"/> to <see cref="BuildingStatisticsModel"/> expects arguments and Database Context. 
        /// User passes correct implementation of <see cref="MappingService{TContext}"/>, but incorrect argument type.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_When_Arguments_And_Context_Are_Required_But_Arguments_Are_Incorrect_Type()
        {
            var query = _context.Buildings.AsQueryable();
            var arguments = new ApartmentsArguments { UnitOfMeasure = "asda" };
            var method = new TestDelegate(delegate { _service.Map<Building, BuildingStatisticsModel, ApartmentsArguments>(query, arguments); });
            var exceptionMessage = string.Format(Resources.ArgumentsOfIncorrectType, typeof(Building).Name, typeof(BuildingStatisticsModel).Name, typeof(BuildingArguments), typeof(ApartmentsArguments));

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        /// <summary>
        /// Mapping from <see cref="Building"/> to <see cref="BuildingStatisticsModel"/> expects arguments and Database Context. 
        /// User passes incorrect implementation of <see cref="MappingService{TContext}"/> and incorrect argument type.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_When_Arguments_And_Context_Are_Required_But_Arguments_And_Context_Are_Incorrect_Type()
        {
            var query = _context.Buildings.AsQueryable();
            var arguments = new ApartmentsArguments { UnitOfMeasure = "asda" };
            var method = new TestDelegate(delegate { _securityService.Map<Building, BuildingStatisticsModel, ApartmentsArguments>(query, arguments); });
            var exceptionMessage = string.Format(Resources.ArgumentsOfIncorrectType, typeof(Building).Name, typeof(BuildingStatisticsModel).Name, typeof(BuildingArguments), typeof(ApartmentsArguments));

            var exception = Assert.Throws<MappingException>(method);
            Assert.AreEqual(exception.Message, exceptionMessage);
        }

        /// <summary>
        /// User passes nullable value of arguments.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_If_Args_Arent_Passed_Into_Method()
        {
            var query = _context.Apartments.AsQueryable();

            var methodWithoutContext = new TestDelegate(delegate { _service.Map<Apartment, ApartmentReviewsModel, ApartmentsArguments>(query, null); });
            var methodWithContext = new TestDelegate(delegate { _service.Map<Apartment, ApartmentReviewsModel, ApartmentsArguments>(query, null); });

            var exception1 = Assert.Throws<MappingException>(methodWithContext);
            Assert.AreEqual(exception1.Message, Resources.ArgumentsAreRequiredException);

            var exception2 = Assert.Throws<MappingException>(methodWithoutContext);
            Assert.AreEqual(exception2.Message, Resources.ArgumentsAreRequiredException);
        }

        /// <summary>
        /// User passes nullable value of arguments.
        /// </summary>
        [Test]
        public void Should_Throw_Exception_If_Args_Arent_Passed_Into_Object_Mapping()
        {
            var list = _context.Apartments.ToList();

            var methodWithoutContext = new TestDelegate(delegate { _service.Map<Apartment, ApartmentReviewsModel, ApartmentsArguments>(list, null); });
            var methodWithContext = new TestDelegate(delegate { _service.Map<Apartment, ApartmentReviewsModel, ApartmentsArguments>(list, null); });

            var exception1 = Assert.Throws<MappingException>(methodWithContext);
            Assert.AreEqual(exception1.Message, Resources.ArgumentsAreRequiredException);

            var exception2 = Assert.Throws<MappingException>(methodWithoutContext);
            Assert.AreEqual(exception2.Message, Resources.ArgumentsAreRequiredException);
        }
    }
}
