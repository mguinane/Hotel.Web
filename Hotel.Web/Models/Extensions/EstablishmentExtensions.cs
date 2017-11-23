using System.Collections.Generic;
using System.Linq;

namespace Hotel.Web.Models.Extensions
{
    public static class EstablishmentExtensions
    {
        public static IEnumerable<Establishment> Filter(this IEnumerable<Establishment> source, SearchResultsCriteria criteria)
        {
            return source.Where(e =>
                (string.IsNullOrWhiteSpace(criteria.Name) || e.Name.ToLower().Contains(criteria.Name.ToLower())) &&
                ((criteria.Stars == null || criteria.Stars.Length == 0) || criteria.Stars.Contains(e.Stars)) &&
                (criteria.MinUserRating == 0 || e.UserRating >= criteria.MinUserRating) && (criteria.MaxUserRating == 0 || e.UserRating <= criteria.MinUserRating) &&
                (criteria.MinCost == 0 || e.MinCost >= criteria.MinCost));
        }

        public static IEnumerable<Establishment> Sort(this IEnumerable<Establishment> source, SortType sortType)
        {
            switch (sortType)
            {
                case SortType.Stars:
                    return source.OrderByDescending(e => e.Stars);
                case SortType.MinCost:
                    return source.OrderBy(e => e.MinCost);
                case SortType.UserRating:
                    return source.OrderByDescending(e => e.UserRating).ThenByDescending(e => e.UserRatingCount);
                default:
                    return source.OrderBy(e => e.Distance);
            }
        }
    }
}
