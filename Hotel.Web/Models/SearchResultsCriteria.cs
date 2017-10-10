namespace Hotel.Web.Models
{
    public class SearchResultsCriteria
    {
        public int PageIndex { get; set; }
        public int SortType { get; set; }
        public string Name { get; set; }
        public int[] Stars { get; set; }
        public int MinUserRating { get; set; }
        public int MaxUserRating { get; set; }
        public int MinCost { get; set; }
    }
}
