﻿using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.GenericService.Tests.Context.SecurityContext.Entities;
using DevTeam.GenericService.Tests.Tests;
using Microsoft.EntityFrameworkCore;

namespace DevTeam.GenericService.Tests.Context.SecurityContext;

public interface ISecurityContext : IDbContext
{
    DbSet<User> Users { get; set; }
}

public class SecurityContext : DbContext, ISecurityContext
{
    private static DbContextOptions<SecurityContext> GetOptions(string name)
    {
        return new DbContextOptionsBuilder<SecurityContext>()
            .UseInMemoryDatabase(databaseName: name)
            .Options;
    }

    public DbSet<User> Users { get; set; } = null!;

    public SecurityContext(string name)
        : base(GetOptions(name))
    {
        Users.AddRange(TestData.Users);
        SaveChanges();
    }

    public SecurityContext() 
        : this(Guid.NewGuid().ToString()) 
    { }
}
