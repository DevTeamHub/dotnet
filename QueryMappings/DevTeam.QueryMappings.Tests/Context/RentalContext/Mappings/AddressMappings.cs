using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Helpers;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Entities;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Models;

namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Mappings
{
    public class AddressMappings: IMappingsStorage
    {
        public void Setup(IMappingsList mappings)
        {
            mappings.Add<Address, AddressModel>(x => new AddressModel
            {
                Id = x.Id,
                BuildingNumber = x.BuildingNumber,
                City = x.City,
                State = x.State,
                Country = (Countries) x.Country,
                Street = x.Street,
                ZipCode = x.ZipCode
            });

            mappings.Add<Address, AddressSummaryModel>(MappingsNames.ExtendedAddressFormat, x => new AddressSummaryModel
            {
                Id = x.Id,
                Address = x.BuildingNumber + " " + x.Street + ", " + x.City + ", " + x.State + ", " + x.Country + ", " + x.ZipCode
            });

            mappings.Add<Address, AddressSummaryModel>(MappingsNames.ShortAddressFormat, x => new AddressSummaryModel
            {
                Id = x.Id,
                Address = x.BuildingNumber + " " + x.Street + ", " + x.City 
            });

            mappings.Add<Address, InvalidAddressMapping>(x => new InvalidAddressMapping
            {
                Id = x.Id,
                BuildingNumber = x.BuildingNumber,
                City = x.City,
                State = x.State,
                Country = (Countries)x.Country,
                Street = x.Street,
                ZipCode = x.ZipCode
            });

            mappings.Add<Address, InvalidAddressMapping>(x => new InvalidAddressMapping
            {
                Id = x.Id,
                BuildingNumber = x.BuildingNumber,
                City = x.City,
                State = x.State,
                Country = (Countries)x.Country,
                Street = x.Street,
                ZipCode = x.ZipCode
            });
        }
    }
}
