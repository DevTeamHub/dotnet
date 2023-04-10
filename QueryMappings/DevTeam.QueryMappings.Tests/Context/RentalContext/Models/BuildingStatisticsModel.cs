namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Models
{
    public class BuildingStatisticsModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int AppartmentsCount { get; set; }
        public int ResidentsCount { get; set; }
        public int Size { get; set; }
        public double AverageBuildingRating { get; set; }
    }
}
