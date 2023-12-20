namespace DevTeam.GenericRepository
{
    public class QueryOptions
    {
        public QueryOptions() 
        {
            IncludeDeleted = false;
            IsReadOnly = false;
            ApplySecurity = false;
        }

        public bool IncludeDeleted {  get; set; }
        public bool IsReadOnly {  get; set; }
        public bool ApplySecurity {  get; set; }
    }
}
