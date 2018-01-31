using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotel.Web.Core.Models.Extensions
{
    public static class EstablishmentExtensions
    {
        public static IEnumerable<Establishment> Filter(this IEnumerable<Establishment> source, SearchCriteria criteria)
        {
            if (!string.IsNullOrWhiteSpace(criteria.Name))
                source = source.Where(e => e.Name.ToLower().Contains(criteria.Name.ToLower()));

            if (criteria.Stars != null && criteria.Stars.Length > 0)
                source = source.Where(e => criteria.Stars.Contains(e.Stars));

            if (criteria.MinUserRating > 0)
                source = source.Where(e => e.UserRating >= criteria.MinUserRating);

            if (criteria.MaxUserRating > 0)
                source = source.Where(e => e.UserRating <= criteria.MaxUserRating);

            if (criteria.MinCost > 0)
                source = source.Where(e => e.MinCost >= criteria.MinCost);

            return source;
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

        public static IEnumerable<Establishment> Page(this IEnumerable<Establishment> source, int pageIndex, int pageSize)
        {
            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public static int PageCount(this IEnumerable<Establishment> source, int pageSize)
        {
            return (int)Math.Ceiling(source.Count() / (double)pageSize);
        }
    }
}
