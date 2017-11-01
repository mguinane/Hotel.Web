namespace Hotel.Web.ViewModels
{
    public class AvailabilitySearchViewModel
    {
        public string AvailabilitySearchId { get; set; }
        public EstablishmentViewModel[] Establishments { get; set; }

        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int TotalResults { get; set; }

        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < PageCount);
    }
}
