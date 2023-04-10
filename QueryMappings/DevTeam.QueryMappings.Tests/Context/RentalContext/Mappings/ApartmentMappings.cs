using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Helpers;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Entities;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Mappings.Arguments;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Models;
using System.Linq;

namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Mappings
{
    public class ApartmentMappings: IMappingsStorage
    {
        public void Setup(IMappingsList mappings)
        {
            mappings.Add<Apartment, ApartmentModel, ApartmentsArguments>(MappingsNames.AppartmentsWithBuilding, args => 
            {
                return x => new ApartmentModel
                {
                    Id = x.Id,
                    Badrooms = x.Badrooms,
                    Bathrooms = x.Bathrooms,
                    Floor = x.Floor,
                    IsLodge = x.IsLodge,
                    Number = x.Number,
                    Size = x.Size.ToString() + args.UnitOfMeasure,
                    Building = new BuildingModel
                    {
                        Id = x.Building.Id,
                        Year = x.Building.Year,
                        Floors = x.Building.Floors,
                        IsLaundry = x.Building.IsLaundry,
                        IsParking = x.Building.IsParking
                    }
                };
            });

            mappings.Add<Apartment, ApartmentModel, ApartmentsArguments>(MappingsNames.AppartmentsWithoutBuilding, args =>
            {
                return x => new ApartmentModel
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

            mappings.Add<Apartment, ApartmentShortModel, ApartmentsArguments>(args => 
            {
                return appartment => new ApartmentShortModel
                {
                    Id = appartment.Id,
                    Floor = appartment.Floor,
                    IsLodge = appartment.IsLodge,
                    Number = appartment.Number,
                    Size = appartment.Size.ToString() + args.UnitOfMeasure
                };
            });

            mappings.Add<Apartment, ApartmentReviewsModel, IRentalContext>((query, context) =>
                from appartment in query
                join review in context.Set<Review>() on new { EntityId = appartment.Id, EntityTypeId = (int)EntityType.Apartment }
                                                     equals new { EntityId = review.EntityId, EntityTypeId = review.EntityTypeId }
                                                     into reviews
                select new ApartmentReviewsModel
                {
                    Id = appartment.Id,
                    Number = appartment.Number,
                    Reviews = reviews.Select(review => new ReviewModel
                    {
                        Id = review.Id,
                        EntityId = review.EntityId,
                        EntityType = (EntityType)review.EntityTypeId,
                        Rating = review.Rating,
                        Comments = review.Comments
                    }).ToList()
                });
        }
    }
}
