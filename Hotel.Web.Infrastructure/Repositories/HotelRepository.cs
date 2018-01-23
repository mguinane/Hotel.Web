using Hotel.Web.Core.Models;
using Hotel.Web.Core.Models.Extensions;
using Hotel.Web.Core.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var totalResults = _searchResults.Establishments.Count();

            // Filter & Sort results
            var establishments = _searchResults.Establishments.Filter(criteria).Sort(criteria.SortType);

            // Page results
            var establishmentList = establishments as IList<Establishment> ?? establishments.ToList();

            var availability = new AvailabilitySearch()
            {
                AvailabilitySearchId = _searchResults.AvailabilitySearchId,
                Establishments = establishmentList.Skip((criteria.PageIndex - 1) * pageSize).Take(pageSize),
                PageIndex = criteria.PageIndex,
                PageCount = (int)Math.Ceiling(establishmentList.Count() / (double)pageSize),
                TotalResults = totalResults
            };

            return availability;
        }
    }   
}
