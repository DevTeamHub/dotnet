namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}
