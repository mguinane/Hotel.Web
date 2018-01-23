using Hotel.Web.Filters;

using System.ComponentModel.DataAnnotations;

namespace Hotel.Web.ViewModels
{
    public class SearchCriteriaViewModel
    {
        public int PageIndex { get; set; }

        [ValidValuesFromEnum(typeof(SortType))]
        public int SortType { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        public int[] Stars { get; set; }
        public int MinUserRating { get; set; }
        public int MaxUserRating { get; set; }
        public int MinCost { get; set; }
    }
}
