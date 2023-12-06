using DevTeam.GenericService.Tests.Context.RentalContext.Entities;
using DevTeam.GenericService.Tests.Context.RentalContext.Models;
using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Helpers;

namespace DevTeam.GenericService.Tests.Context.RentalContext.Mappings
{
    public class PersonMappings : IMappingsStorage
    {
        public void Setup(IMappingsList mappings)
        {
            mappings.Add<Person, PersonModel>( x => new PersonModel
            {
                Id = x.Id,
                FullName = x.FirstName + ' ' + x.LastName,
                Phone = x.Phone,
                Gender = x.Gender,
                Email = x.Email,
                Age = x.Age,
                IsDeleted = x.IsDeleted
            });
        }
    }
}
