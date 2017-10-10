using System.Collections.Generic;

namespace Hotel.Web.Models
{
    // Class created using Edit -> Paste JSON As Classes (Web Essentials extension)

    public class AvailabilitySearch
    {
        public string AvailabilitySearchId { get; set; }
        public IEnumerable<Establishment> Establishments { get; set; }

        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int TotalResults { get; set; }
    }
}
