namespace Hotel.Web.Core.Models
{
    public class SearchCriteria
    {
        public int PageIndex { get; set; }
        public SortType SortType { get; set; }
        public string Name { get; set; }
        public int[] Stars { get; set; }
        public int MinUserRating { get; set; }
        public int MaxUserRating { get; set; }
        public int MinCost { get; set; }

        public SearchCriteria()
        {
            PageIndex = 1;
            SortType = SortType.Distance;
        }
    }
}
