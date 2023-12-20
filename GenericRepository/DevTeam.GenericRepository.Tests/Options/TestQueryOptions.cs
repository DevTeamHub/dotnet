namespace DevTeam.GenericRepository.Tests
{
    public class TestQueryOptions : QueryOptions
    {
        public TestQueryOptions(): base()
        {
            ApplySecurity = true;
        }
    }
}
