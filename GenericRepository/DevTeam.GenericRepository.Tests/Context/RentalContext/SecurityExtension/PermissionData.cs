using DevTeam.Permissions.Core;

namespace DevTeam.GenericRepository.Tests.Context.RentalContext
{
    public class PermissionsData: RelatedData
    {
        public int? ApartmentId { get; set; }

        public override List<int> GetIdsByType(int entityType)
        {
            if (ApartmentId.HasValue)
            {
                return new() { ApartmentId.Value };
            }

            return new();
        }
    }
}
