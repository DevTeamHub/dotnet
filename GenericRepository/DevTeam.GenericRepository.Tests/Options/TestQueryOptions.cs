using DevTeam.Permissions.Core;

namespace DevTeam.GenericRepository.Tests
{
    public class TestQueryOptions : QueryOptions, ISecurityOptions
    {
        public TestQueryOptions(): base()
        {
            ApplySecurity = true;
        }
        public bool ApplySecurity { get; set; }
    }
}
