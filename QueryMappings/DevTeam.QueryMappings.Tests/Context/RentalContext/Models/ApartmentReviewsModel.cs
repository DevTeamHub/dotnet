namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Models;

public class ApartmentReviewsModel
{
    public int Id { get; set; }
    public string Number { get; set; } = null!;

    public List<ReviewModel> Reviews { get; set; } = new();
}
