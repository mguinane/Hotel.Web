using Hotel.Web.Core.Models;
using Hotel.Web.Core.Models.Extensions;
using Hotel.Web.Core.Repositories;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Text;

namespace Hotel.Web.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AvailabilitySearch _searchResults;

        public HotelRepository()
        {
            var assembly = typeof(HotelRepository).GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName().Name;
            var stream = assembly.GetManifestResourceStream($"{assemblyName}.Repositories.hotels.json");
            var text = string.Empty;

            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                text = reader.ReadToEnd();
            }

            _searchResults = JsonConvert.DeserializeObject<AvailabilitySearch>(text);
        }

        public AvailabilitySearch GetHotels(SearchCriteria criteria, int pageSize)
        {
            // Filter & Sort results
            var establishments = _searchResults.Establishments.Filter(criteria).Sort(criteria.SortType);

            // Page results
            var pageCount = establishments.PageCount(pageSize);
            establishments = establishments.Page(criteria.PageIndex, pageSize);

            return new AvailabilitySearch(_searchResults.AvailabilitySearchId, establishments, criteria.PageIndex, pageCount);
        }
    }   
}
