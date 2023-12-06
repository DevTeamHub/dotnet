﻿using DevTeam.GenericRepository.Tests.Context;
using DevTeam.GenericRepository.Tests.Context.RentalContext;
using DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;
using DevTeam.GenericRepository.Tests.Context.SecurityContext.Entities;

namespace DevTeam.GenericRepository.Tests.Tests;

public static class TestData
{
    private static readonly List<Address> _addresses = new()
        {
            new Address
            {
                Id = 1,
                BuildingId = 1,
                BuildingNumber = "149",
                Street = "Sullivan Str",
                City = "New York",
                State = "NY",
                ZipCode = "10012",
                Country = (int)Countries.USA
            },
            new Address
            {
                Id = 2,
                BuildingId = 2,
                BuildingNumber = "618",
                Street = "Marguerita Ave",
                City = "Santa Monica",
                State = "CA",
                ZipCode = "90402",
                Country = (int)Countries.USA
            }
        };

    private static readonly List<Building> _buildings = new()
        {
            new Building
            {
                Id = 1,
                Floors = 2,
                IsLaundry = true,
                IsParking = false,
                Year = 1985
            },
            new Building
            {
                Id = 2,
                Floors = 1,
                IsLaundry = true,
                IsParking = true,
                Year = 1997
            }
        };

    private static readonly List<Apartment> _apartments = new()
        {
            new Apartment
            {
                Id = 1,
                BuildingId = 1,
                Number = "#1",
                Size = 75,
                Floor = 1,
                Badrooms = 1,
                Bathrooms = 1,
                IsLodge = false
            },
            new Apartment
            {
                Id = 2,
                BuildingId = 1,
                Number = "#2",
                Size = 150,
                Floor = 1,
                Badrooms = 2,
                Bathrooms = 2,
                IsLodge = false
            },
            new Apartment
            {
                Id = 3,
                BuildingId = 1,
                Number = "#3",
                Size = 75,
                Floor = 2,
                Badrooms = 1,
                Bathrooms = 1,
                IsLodge = true
            },
            new Apartment
            {
                Id = 4,
                BuildingId = 1,
                Number = "#4",
                Size = 150,
                Floor = 2,
                Badrooms = 1,
                Bathrooms = 2,
                IsLodge = true
            },
            new Apartment
            {
                Id = 5,
                BuildingId = 2,
                Number = "apt. 1",
                Size = 75,
                Floor = 1,
                Badrooms = 1,
                Bathrooms = 1,
                IsLodge = true
            },
            new Apartment
            {
                Id = 6,
                BuildingId = 2,
                Number = "apt. 2",
                Size = 250,
                Floor = 1,
                Badrooms = 3,
                Bathrooms = 2,
                IsLodge = true
            },
            new Apartment
            {
                Id = 7,
                BuildingId = 2,
                Number = "apt. 3",
                Size = 75,
                Floor = 1,
                Badrooms = 1,
                Bathrooms = 1,
                IsLodge = false
            }
        };

    private static readonly List<Person> _people = new()
        {
            new Person
            {
                Id = 1,
                ApartmentId = 1,
                FirstName = "Matthew",
                LastName = "Anderson",
                Gender = (int)Gender.Male,
                Age = 25,
                Email = "matt@gmail.com",
                Phone = "+14256542323",
                IsDeleted = true
            },
            new Person
            {
                Id = 2,
                ApartmentId = 4,
                FirstName = "Chris",
                LastName = "Jackson",
                Gender = (int)Gender.Male,
                Age = 32,
                Email = "chrisjackson@outlook.com",
                Phone = "+14257651212",
                IsDeleted = false
            },
            new Person
            {
                Id = 3,
                ApartmentId = 4,
                FirstName = "Lisa",
                LastName = "Jackson",
                Gender = (int)Gender.Female,
                Age = 27,
                Email = "lisajackson@outlook.com",
                Phone = "+14259875656",
                IsDeleted = false
            },
            new Person
            {
                Id = 4,
                ApartmentId = 5,
                FirstName = "John",
                LastName = "Doe",
                Gender = (int)Gender.Male,
                Age = 43,
                Email = "johndoe@yahoo.com",
                Phone = "+13235321416",
                IsDeleted = false
            },
            new Person
            {
                Id = 5,
                ApartmentId = 5,
                FirstName = "Anna",
                LastName = "Doe",
                Gender = (int)Gender.Female,
                Age = 36,
                Email = "annadoe@yahoo.com",
                Phone = "+13235321416",
                IsDeleted = false
            },
            new Person
            {
                Id = 6,
                ApartmentId = 5,
                FirstName = "Katty",
                LastName = "Doe",
                Gender = (int)Gender.Female,
                Age = 17,
                Email = "kattydoe@yahoo.com",
                Phone = "+13234453232",
                IsDeleted = false
            },
            new Person
            {
                Id = 7,
                ApartmentId = 5,
                FirstName = "Jack",
                LastName = "Doe",
                Gender = (int)Gender.Male,
                Age = 3,
                Email = null,
                Phone = null,
                IsDeleted = false
            },
            new Person
            {
                Id = 8,
                ApartmentId = 6,
                FirstName = "Albert",
                LastName = "Einstein",
                Gender = (int)Gender.Male,
                Age = 139,
                Email = null,
                Phone = null,
                IsDeleted = false
            }
        };

    private static readonly List<Review> _reviews = new()
        {
            new Review
            {
                Id = 1,
                EntityId = 1,
                EntityTypeId = (int)EntityType.Building,
                Rating = 4,
                Comments = "Some test comment"
            },
            new Review
            {
                Id = 2,
                EntityId = 1,
                EntityTypeId = (int)EntityType.Building,
                Rating = 5,
                Comments = "Some another test comment"
            },
            new Review
            {
                Id = 3,
                EntityId = 2,
                EntityTypeId = (int)EntityType.Building,
                Rating = 5,
                Comments = null
            },
            new Review
            {
                Id = 4,
                EntityId = 1,
                EntityTypeId = (int)EntityType.Apartment,
                Rating = 5,
                Comments = null
            },
            new Review
            {
                Id = 5,
                EntityId = 1,
                EntityTypeId = (int)EntityType.Apartment,
                Rating = 3,
                Comments = "some isn't really good opinion"
            },
            new Review
            {
                Id = 6,
                EntityId = 2,
                EntityTypeId = (int)EntityType.Apartment,
                Rating = 5,
                Comments = "Very good review of appartment"
            },
            new Review
            {
                Id = 7,
                EntityId = 3,
                EntityTypeId = (int)EntityType.Apartment,
                Rating = 1,
                Comments = "no comments..."
            },
            new Review
            {
                Id = 8,
                EntityId = 3,
                EntityTypeId = (int)EntityType.Apartment,
                Rating = 2,
                Comments = "aweful..."
            },
            new Review
            {
                Id = 9,
                EntityId = 4,
                EntityTypeId = (int)EntityType.Apartment,
                Rating = 3,
                Comments = null
            },
            new Review
            {
                Id = 10,
                EntityId = 4,
                EntityTypeId = (int)EntityType.Apartment,
                Rating = 3,
                Comments = null
            },
            new Review
            {
                Id = 11,
                EntityId = 4,
                EntityTypeId = (int)EntityType.Apartment,
                Rating = 4,
                Comments = null
            },
            new Review
            {
                Id = 12,
                EntityId = 5,
                EntityTypeId = (int)EntityType.Apartment,
                Rating = 4,
                Comments = "good review"
            },
            new Review
            {
                Id = 13,
                EntityId = 6,
                EntityTypeId = (int)EntityType.Apartment,
                Rating = 5,
                Comments = "amazing review"
            },
            new Review
            {
                Id = 14,
                EntityId = 6,
                EntityTypeId = (int)EntityType.Apartment,
                Rating = 3,
                Comments = "average review"
            }
        };

    private static readonly List<User> _users = new()
        {
            new User
            {
                Id = 1,
                UserName = "User1",
                Password = "asdasdasdas",
                IsAdmin = false
            },
            new User
            {
                Id = 2,
                UserName = "User2",
                Password = "erterterte",
                IsAdmin = true
            },
            new User
            {
                Id = 3,
                UserName = "User3",
                Password = "ghjghjgjhjgh",
                IsAdmin = false
            }
        };

    public static List<Address> Addresses => _addresses;
    public static List<Apartment> Apartments => _apartments;
    public static List<Building> Buildings => _buildings;
    public static List<Person> People => _people;
    public static List<Review> Reviews => _reviews;
    public static List<User> Users => _users;
}