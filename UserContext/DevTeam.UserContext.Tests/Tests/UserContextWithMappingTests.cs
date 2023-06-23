using DevTeam.UserContext.Tests.Context;
using DevTeam.GenericService.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using DevTeam.UserContext.Tests.Context.Entities;
using DevTeam.UserContext.Tests.Context.Models;
using DevTeam.UserContext.Tests.Context.Mappings;
using DevTeam.UserContext.Tests.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using DevTeam.Extensions.EntityFrameworkCore;
using System.Security.Claims;
using IdentityModel;
using DevTeam.UserContext.Asbtractions;

namespace DevTeam.UserContext.Tests.Tests;

[TestCategory("UserContextServiceFilter")]
[TestClass]
public class UserContextWithMappingTests
{
    private static IServiceProvider _serviceProvider = null!;
    private static HomeController _controller = null!;
    private static ActionExecutingContext _executingContext = null!;
    private static ActionExecutionDelegate _next = null!;

    [ClassInitialize]
    public static void Init(TestContext testContext)
    {
        var builder = WebApplication.CreateBuilder();

        var services = builder.Services;

        services
            .AddDbContext<IDbContext, SecurityContext>()
            .AddGenericServices();

        services.AddScoped<HomeController>();

        var mvcBuilder = services.AddControllers();

        mvcBuilder
            .AddUserContext()
            .AddUserMapping<User, UserModel>(new UserContextOptions { UseSession = false });

        var app = builder.Build();

        app.UseGenericServices(typeof(UserMappings).Assembly);

        app.MapControllers();

        _serviceProvider = app.Services;
        _controller = _serviceProvider.GetRequiredService<HomeController>();

        var user = TestData.Users.Single(x => x.Id == 1);

        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Email, user.UserName),
            new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
            new Claim(JwtClaimTypes.Role, "User")
        };

        var identity = new ClaimsIdentity(claims, "authorization_code", JwtClaimTypes.Subject, JwtClaimTypes.Role);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext
        {
            User = principal
        };

        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor(),
        };
        var metadata = new List<IFilterMetadata>();

        _executingContext = new ActionExecutingContext(actionContext, metadata, new Dictionary<string, object?>(), _controller);

        _next = () => {
            var ctx = new ActionExecutedContext(actionContext, metadata, _controller);
            return Task.FromResult(ctx);
        };
    }

    [ClassCleanup]
    public static void Clear()
    {
        _serviceProvider = null!;
        _controller = null!;
    }

    [TestMethod]
    public async Task Shound_Find_Address_Expression_Mapping_In_Storage()
    {
        var filter = _serviceProvider.GetRequiredService<UserContextServiceFilter<int>>();

        await filter.OnActionExecutionAsync(_executingContext, _next);

        var userContext = _controller.UserContext;

        Assert.IsNotNull(userContext);
        Assert.IsInstanceOfType<IUserContext<int>>(userContext);
        Assert.IsInstanceOfType<IUserContext<UserModel, int>>(userContext);
        Assert.IsNotNull(userContext.User);
    }
}