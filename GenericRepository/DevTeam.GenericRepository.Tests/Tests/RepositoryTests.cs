using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.GenericRepository.Tests.Context.RentalContext;
using DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;
using DevTeam.GenericRepository.Tests.Context.SecurityContext;
using DevTeam.GenericRepository.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DevTeam.Extensions.Abstractions;
using DevTeam.GenericRepository;
using DevTeam.GenericRepository.Tests.Context;
using System.Security;

namespace DevTeam.GenericRepository.Tests;

[TestCategory("Repository")]
[TestClass]
public class RepositoryTests
{
    private static ServiceProvider _serviceProvider = null!;
    private static IRepository<IRentalContext, TestQueryOptions> _repository = null!;
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
            .AddScoped<IUserContext<Person>, UserContext<Person>>()
            .AddScoped<IUserContext, UserContext>()
            .AddScoped(typeof(IQueryExtension<,>), typeof(IncludeDeletedQueryExtension<,>))
            .AddScoped(typeof(IQueryExtension<,>), typeof(IsReadOnlyQueryExtension<,>))
            .AddScoped(typeof(ISecurityQueryExtension<Person, TestQueryOptions>), typeof(PersonQueryExtension))
            .AddScoped<IRelatedDataService<PermissionsData>, RelatedDataService>()
            .AddScoped<IPermissionsService, PermissionsService>()
            .AddGenericRepository();

        _serviceProvider = services.BuildServiceProvider();

        _rentalContext = (RentalContext)_serviceProvider.GetRequiredService<IRentalContext>();
        _repository = _serviceProvider.GetRequiredService<IRepository<IRentalContext, TestQueryOptions>>();


        //_rentalContext = new RentalContext("OriginalRental");
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
        var rental = _serviceProvider.GetRequiredService<IRepository<IRentalContext, TestQueryOptions>>();
        var security = _serviceProvider.GetRequiredService<IRepository<ISecurityContext, TestQueryOptions>>();

        Assert.IsInstanceOfType(standard, typeof(Repository));
        Assert.IsInstanceOfType(rental, typeof(Repository<IRentalContext, TestQueryOptions>));
        Assert.IsInstanceOfType(security, typeof(Repository<ISecurityContext, TestQueryOptions>));
    }

    [TestMethod]
    public void Should_Correctly_Resolve_Different_Read_Only_Repositories()
    {
        var standard = _serviceProvider.GetRequiredService<IReadOnlyRepository>();
        var rental = _serviceProvider.GetRequiredService<IReadOnlyRepository<IRentalContext, TestQueryOptions>>();
        var security = _serviceProvider.GetRequiredService<IReadOnlyRepository<ISecurityContext, TestQueryOptions>>();

        Assert.IsInstanceOfType(standard, typeof(ReadOnlyRepository));
        Assert.IsInstanceOfType(rental, typeof(ReadOnlyRepository<IRentalContext, TestQueryOptions>));
        Assert.IsInstanceOfType(security, typeof(ReadOnlyRepository<ISecurityContext, TestQueryOptions>));
    }

    [TestMethod]
    public void Should_Correctly_Resolve_Different_Soft_Delete_Repositories()
    {
        var standard = _serviceProvider.GetRequiredService<ISoftDeleteRepository>();
        var rental = _serviceProvider.GetRequiredService<ISoftDeleteRepository<IRentalContext, TestQueryOptions>>();
        var security = _serviceProvider.GetRequiredService<ISoftDeleteRepository<ISecurityContext, TestQueryOptions>>();

        Assert.IsInstanceOfType(standard, typeof(SoftDeleteRepository));
        Assert.IsInstanceOfType(rental, typeof(SoftDeleteRepository<IRentalContext, TestQueryOptions>));
        Assert.IsInstanceOfType(security, typeof(SoftDeleteRepository<ISecurityContext, TestQueryOptions>));
    }

    [TestMethod]
    public void Should_Correctly_Resolve_Different_Read_Only_Delete_Repositories()
    {
        var standard = _serviceProvider.GetRequiredService<IReadOnlyDeleteRepository>();
        var rental = _serviceProvider.GetRequiredService<IReadOnlyDeleteRepository<IRentalContext, TestQueryOptions>>();
        var security = _serviceProvider.GetRequiredService<IReadOnlyDeleteRepository<ISecurityContext, TestQueryOptions>>();

        Assert.IsInstanceOfType(standard, typeof(ReadOnlyDeleteRepository));
        Assert.IsInstanceOfType(rental, typeof(ReadOnlyDeleteRepository<IRentalContext, TestQueryOptions>));
        Assert.IsInstanceOfType(security, typeof(ReadOnlyDeleteRepository<ISecurityContext, TestQueryOptions>));
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
                Assert.AreEqual(entity.ApartmentId, model.ApartmentId);
                Assert.AreEqual(entity.FirstName, model.FirstName);
                Assert.AreEqual(entity.LastName, model.LastName);
                Assert.AreEqual(entity.Age, model.Age);
                Assert.AreEqual(entity.Gender, model.Gender);
                Assert.AreEqual(entity.Email, model.Email);
                Assert.AreEqual(entity.Phone, model.Phone);
                Assert.AreEqual(entity.IsDeleted, false);
            } else
            {
                Assert.AreEqual(entity.IsDeleted, true);
            }
        }
    }

    [TestMethod]
    public void Should_Update_Propery_With_Id_And_Property_Selector()
    {
        const string newName = "TestName";
        var entity = _rentalContext.People.Where(x => x.IsDeleted == false).First();
        var entityFirstName = entity.FirstName;

        _repository.UpdateProperty<Person, string>(entity.Id, x => x.FirstName, newName);
        _repository.Save();

        var updatedEntity = _rentalContext.People.Where(x => x.Id == entity.Id).First();
        Assert.AreNotEqual(entityFirstName, updatedEntity.FirstName);
        Assert.AreEqual(updatedEntity.FirstName, newName);
    }

    [TestMethod]
    public void Should_Return_List_Of_Items_Included_Deleted()
    {
        var entities = _rentalContext.People.ToList();

        var options = new TestQueryOptions
        {
            IncludeDeleted = true,
        };
        var modelsQuery = _repository.GetList<Person>(null, options);

        Assert.IsNotNull(modelsQuery);
        Assert.IsInstanceOfType(modelsQuery, typeof(IQueryable<Person>));

        var models = modelsQuery.ToList();

        Assert.AreEqual(entities.Count, models.Count);

        foreach (var entity in entities)
        {
            var model = models.FirstOrDefault(x => x.Id == entity.Id);

            if (model != null)
            {
                Assert.AreEqual(entity.Id, model.Id);
                Assert.AreEqual(entity.ApartmentId, model.ApartmentId);
                Assert.AreEqual(entity.FirstName, model.FirstName);
                Assert.AreEqual(entity.LastName, model.LastName);
                Assert.AreEqual(entity.Age, model.Age);
                Assert.AreEqual(entity.Gender, model.Gender);
                Assert.AreEqual(entity.Email, model.Email);
                Assert.AreEqual(entity.Phone, model.Phone);
                Assert.AreEqual(entity.IsDeleted, model.IsDeleted);
            }
        }
    }

    [TestMethod]
    public void Should_Return_Item_By_Id_Of_Deleted_Items()
    {
        var entities = _rentalContext.People.ToList();
        var searchEntity = entities.FirstOrDefault(entity => entity.IsDeleted);
        if (searchEntity != null)
        {
            var options = new TestQueryOptions
            {
                IncludeDeleted = true,
            };
            var model = _repository.Get<Person>(searchEntity.Id, options);

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(model, typeof(Person));
            Assert.AreEqual(searchEntity.Id, model.Id);
            Assert.AreEqual(searchEntity.ApartmentId, model.ApartmentId);
            Assert.AreEqual(searchEntity.FirstName, model.FirstName);
            Assert.AreEqual(searchEntity.LastName, model.LastName);
            Assert.AreEqual(searchEntity.Age, model.Age);
            Assert.AreEqual(searchEntity.Gender, model.Gender);
            Assert.AreEqual(searchEntity.Email, model.Email);
            Assert.AreEqual(searchEntity.Phone, model.Phone);
            Assert.AreEqual(searchEntity.IsDeleted, model.IsDeleted);
        }
    }

    [TestMethod]
    public void Should_Return_Item_By_Id_Of_Items()
    {
        var entities = _rentalContext.People.ToList();
        var searchEntity = entities.FirstOrDefault(entity => !entity.IsDeleted);
        if (searchEntity != null)
        {
            var model = _repository.Get<Person>(searchEntity.Id);

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(model, typeof(Person));
            Assert.AreEqual(searchEntity.Id, model.Id);
            Assert.AreEqual(searchEntity.ApartmentId, model.ApartmentId);
            Assert.AreEqual(searchEntity.FirstName, model.FirstName);
            Assert.AreEqual(searchEntity.LastName, model.LastName);
            Assert.AreEqual(searchEntity.Age, model.Age);
            Assert.AreEqual(searchEntity.Gender, model.Gender);
            Assert.AreEqual(searchEntity.Email, model.Email);
            Assert.AreEqual(searchEntity.Phone, model.Phone);
            Assert.AreEqual(searchEntity.IsDeleted, model.IsDeleted);
        }
    }

    [TestMethod]
    public void Get_Not_Deleted_Items_With_Security_Filter()
    {
        var entities = _rentalContext.People.ToList();
        var permissions = new PermissionsArgs
        {
            AccessPermission = (int)Permissions.ViewPeople,
            OtherPermissions = new int[] { }
        };
        var modelsQuery = _repository.Query<Person, PermissionsArgs>(permissions);

        Assert.IsNotNull(modelsQuery);
        Assert.IsInstanceOfType(modelsQuery, typeof(IQueryable<Person>));

        var models = modelsQuery.ToList();

        Assert.AreEqual(entities.Count(x => !x.IsDeleted && x.ApartmentId == 4), models.Count);
    }
}
