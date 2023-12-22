namespace DevTeam.GenericRepository
{
    public class QueryOptions
    {
        public QueryOptions() 
        {
            IncludeDeleted = false;
            IsReadOnly = false;
        }

        public bool IncludeDeleted {  get; set; }
        public bool IsReadOnly {  get; set; }
    }
}
