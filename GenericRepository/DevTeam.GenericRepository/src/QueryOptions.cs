namespace DevTeam.GenericRepository
{
    public class QueryOptions
    {
        public QueryOptions() 
        {
            isDeleted = false;
            isReadOnly = true;
        }
        public bool isDeleted {  get; set; }
        public bool isReadOnly {  get; set; }
    }
}
