using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Helpers;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Entities;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Mappings.Arguments;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Models;

namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Mappings
{
    public class BuildingMappings : IMappingsStorage
    {
        public void Setup(IMappingsList mappings)
        {
            mappings.Add<Building, BuildingModel>(MappingsNames.BuildingWithoutReviews, x => new BuildingModel
            {
                Id = x.Id,
                Year = x.Year,
                Floors = x.Floors,
                IsLaundry = x.IsLaundry,
                IsParking = x.IsParking,
                Address = new AddressModel
                {
                    Id = x.Address.Id,
                    BuildingNumber = x.Address.BuildingNumber,
                    City = x.Address.City,
                    Country = (Countries)x.Address.Country,
                    State = x.Address.State,
                    Street = x.Address.Street,
                    ZipCode = x.Address.ZipCode
                },
                Appartments = x.Appartments.Select(a => new ApartmentModel
                {
                    Id = a.Id,
                    Badrooms = a.Badrooms,
                    Bathrooms = a.Bathrooms,
                    Floor = a.Floor,
                    IsLodge = a.IsLodge,
                    Number = a.Number,
                    Size = a.Size.ToString()
                }).ToList()
            });

            mappings.Add<Building, BuildingModel, IRentalContext>(MappingsNames.BuildingWithReviews, (query, context) => 
                from building in query
                let reviews = context.Set<Review>().Where(x => x.EntityId == building.Id && x.EntityTypeId == (int) EntityType.Building)
                select new BuildingModel
                {
                    Id = building.Id,
                    Year = building.Year,
                    Floors = building.Floors,
                    IsLaundry = building.IsLaundry,
                    IsParking = building.IsParking,
                    Address = new AddressModel
                    {
                        Id = building.Address.Id,
                        BuildingNumber = building.Address.BuildingNumber,
                        City = building.Address.City,
                        Country = (Countries)building.Address.Country,
                        State = building.Address.State,
                        Street = building.Address.Street,
                        ZipCode = building.Address.ZipCode
                    },
                    Appartments = building.Appartments.Select(a => new ApartmentModel
                    {
                        Id = a.Id,
                        Badrooms = a.Badrooms,
                        Bathrooms = a.Bathrooms,
                        Floor = a.Floor,
                        IsLodge = a.IsLodge,
                        Number = a.Number,
                        Size = a.Size.ToString()
                    }).ToList(),
                    Reviews = reviews.Select(review => new ReviewModel
                    {
                        Id = review.Id,
                        EntityId = review.EntityId,
                        EntityType = (EntityType) review.EntityTypeId,
                        Rating = review.Rating,
                        Comments = review.Comments
                    }).ToList()
                });

            mappings.Add<Building, BuildingStatisticsModel, BuildingArguments, IRentalContext>(args =>
            {
                return (query, context) =>
                    from building in query
                    let reviews = context.Set<Review>().Where(x => x.EntityId == building.Id && x.EntityTypeId == (int)EntityType.Building)
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
        }
    }
}
