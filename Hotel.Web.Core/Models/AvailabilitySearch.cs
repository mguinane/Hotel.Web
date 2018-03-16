using System.Collections.Generic;

namespace Hotel.Web.Core.Models
{
    // Class created using Edit -> Paste JSON As Classes (Web Essentials extension)

    public class AvailabilitySearch
    {
        public string AvailabilitySearchId { get; private set; }
        public IEnumerable<Establishment> Establishments { get; private set; } = new List<Establishment>();

        public int PageIndex { get; private set; }
        public int PageCount { get; private set; }

        public AvailabilitySearch() { }

        public AvailabilitySearch(string availabilitySearchId, IEnumerable<Establishment> establishments, int pageIndex, int pageCount)
        {
            AvailabilitySearchId = availabilitySearchId;
            Establishments = establishments;
            PageIndex = pageIndex;
            PageCount = pageCount;
        }
    }
}
