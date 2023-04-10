using System.Collections.Generic;

namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Models
{
    public class ApartmentReviewsModel
    {
        public int Id { get; set; }
        public string Number { get; set; }

        public List<ReviewModel> Reviews { get; set; }

        public ApartmentReviewsModel()
        {
            Reviews = new List<ReviewModel>();
        }
    }
}
