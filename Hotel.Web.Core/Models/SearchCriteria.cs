namespace Hotel.Web.Core.Models
{
    public class SearchCriteria
    {
        public int PageIndex { get; set; } = 1;
        public SortType SortType { get; set; } = SortType.Distance;
        public string Name { get; set; }
        public int[] Stars { get; set; }
        public int MinUserRating { get; set; }
        public int MaxUserRating { get; set; }
        public int MinCost { get; set; }
    }
}
