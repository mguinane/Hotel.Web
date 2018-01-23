namespace Hotel.Web.Core.Models
{
    public class SearchCriteria
    {
        public int PageIndex { get; private set; }
        public SortType SortType { get; private set; }
        public string Name { get; private set; }
        public int[] Stars { get; private set; }
        public int MinUserRating { get; private set; }
        public int MaxUserRating { get; private set; }
        public int MinCost { get; private set; }

        public SearchCriteria()
        {
            PageIndex = 1;
            SortType = SortType.Distance;
        }
    }
}
