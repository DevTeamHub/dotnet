using DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;

namespace DevTeam.GenericRepository.Tests.Context.RentalContext
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IUserContext<Person> _userContext;

        public PermissionsService(IUserContext<Person> userContext)
        {
            _userContext = userContext;
            _userContext.User ??= new Person()
            {
                Id = 2,
                ApartmentId = 4,
                FirstName = "Chris",
                LastName = "Jackson",
                Gender = (int)Gender.Male,
                Age = 32,
                Email = "chrisjackson@outlook.com",
                Phone = "+14257651212",
                IsDeleted = false,
                Permissions = new List<PermissionModel>
                {
                    new PermissionModel { Id = 1, Scopes = (int)PermissionScopes.Apartments }
                }
            };
        }

        public List<PermissionModel> GetCurrentAccountPermissions()
        {
            
            return _userContext.User!.Permissions;
        }

        public string GetCurrentAccountId()
        {
            return _userContext.Id!.Value.ToString();
        }
    }
}
