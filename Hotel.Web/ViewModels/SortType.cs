using System.ComponentModel.DataAnnotations;

namespace Hotel.Web.ViewModels
{
    public enum SortType
    {
        Distance = 0,
        Stars = 1,
        [Display(Name = "Minimum Cost")]
        MinCost = 2,
        [Display(Name = "Rating")]
        UserRating = 3
    }
}
