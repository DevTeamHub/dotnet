# QueryMappings

Query mappings provide simple strongly typed interface optimized for Entity Framework and IQueryable. 

[![TeamCity Build Status](http://teamcity.dev-team.com/app/rest/builds/buildType:(id:Packages_QueryMappings)/statusIcon.svg)](http://teamcity.dev-team.com/viewType.html?buildTypeId=Packages_QueryMappings) [![NuGet](https://img.shields.io/badge/nuget-v2.0.0-blue.svg)](https://www.nuget.org/packages/DevTeam.QueryMappings/)

## Motivation

As we use Entity Framework and IQueryable interface for a tons of different projects, we needed some naturally adaptable mapping interface to IQueryable interface. This interface should support possibility to pass arguments for more complicated mapping cases.
Also there are cases when we can't have Navigation Property between objects or when we want to write LINQ syntax for queries, so we needed possibility to inject EF Context inside of the mapping. 

## Features

1. Simple and strongly typed mapping interface. Mappings API should be natural - we just map one object to another, we don't need 10 different methods just to do it:

```C#

    _mappingsList.Add<Address, AddressModel>(x => new AddressModel
    {
        Id = x.Id,
        BuildingNumber = x.BuildingNumber,
        City = x.City,
        State = x.State,
        Country = (Countries)x.Country,
        Street = x.Street,
        ZipCode = x.ZipCode
    });

```

2. Oriented on Entity Framework. Best support of IQueryable and even injection of Entity Framework context inside of the mapping expression.

```C#

_mappingsList.Add<Building, BuildingStatisticsModel, BuildingArguments, IDbContext>(args =>
{
    return (query, context) => 
        from building in query
        join review in context.Set<Review>() on new { EntityId = building.Id, EntityTypeId = (int)EntityType.Building }
                                             equals new { EntityId = review.EntityId, EntityTypeId = review.EntityTypeId }
                                             into reviews
        let address = building.Address
        select new BuildingStatisticsModel
        {
            Id = building.Id,
            Address = address.BuildingNumber + ", " + address.Street + ", " + address.City,
            AppartmentsCount = building.Appartments.Count(),
            Size = building.Appartments.Sum(app => app.Size),
            ResidentsCount = building.Appartments.SelectMany(app => app.Residents).Where(r => r.Age > args.TargetResidentsAge).Count(),
            AverageBuildingRating = reviews.Average(r => r.Rating)
        };
});

```

3. Possibility to pass and use arguments inside of the mapping

```C#

_mappingsList.Add<Appartment, AppartmentModel, AppartmentsArguments>(args =>
{
    return x => new AppartmentModel
    {
        Id = x.Id,
        Badrooms = x.Badrooms,
        Bathrooms = x.Bathrooms,
        Floor = x.Floor,
        IsLodge = x.IsLodge,
        Number = x.Number,
        Size = x.Size.ToString() + args.UnitOfMeasure
    };
});

```

4. Strobgly typed interface. If types of your models have changed you will see compile-time errors. 

5. No any reflection is used in runtime. Only one place when reflection is used - start of application.

## Documentation

You can find documentation in the [wiki](https://github.com/DevTeamHub/QueryMappings/wiki) to this repository.

## Support

If you found any bug, please submit bug in our [issue tracker] (https://github.com/DevTeamHub/QueryMappings/issues). If you can submit [pull request] with fix (https://github.com/DevTeamHub/QueryMappings/pulls) - even better! And we will be happy to hear different suggestions to improve this project.

## Future

Main features in the near future is reusable mappings and object-to-object mappings. You can find roadmap to the project [here](https://github.com/DevTeamHub/QueryMappings/projects/1).  

## How to start

Please reference to [quick start](https://github.com/DevTeamHub/QueryMappings/wiki/Quick-Start) in our [wiki](https://github.com/DevTeamHub/QueryMappings/wiki).


