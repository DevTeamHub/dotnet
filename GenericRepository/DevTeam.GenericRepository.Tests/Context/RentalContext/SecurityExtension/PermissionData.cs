using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
