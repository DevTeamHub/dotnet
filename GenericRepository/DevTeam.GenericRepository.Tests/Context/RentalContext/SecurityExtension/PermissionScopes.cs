using System.ComponentModel;

namespace DevTeam.GenericRepository.Tests.Context.RentalContext
{
    [Flags]
    public enum PermissionScopes : byte
    {
        [Description("Apartments")]
        Apartments = 1
    }
}
