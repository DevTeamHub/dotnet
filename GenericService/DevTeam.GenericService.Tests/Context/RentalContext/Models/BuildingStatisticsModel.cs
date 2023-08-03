namespace DevTeam.GenericService.Tests.Context.RentalContext.Models;

public class BuildingStatisticsModel
{
    public int Id { get; set; }
    public string Address { get; set; } = null!;
    public int AppartmentsCount { get; set; }
    public int ResidentsCount { get; set; }
    public int Size { get; set; }
    public double AverageBuildingRating { get; set; }
}
