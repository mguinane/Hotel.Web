using FluentAssertions;
using Hotel.Web.Core.Models;
using Hotel.Web.Core.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Hotel.Web.Tests.Core.Models.Extensions
{
    public class EstablishmentExtensionsTests
    {
        private List<Establishment> _establishments;
        private SearchCriteria _searchCriteria;
        const SortType DefaultSearchType = SortType.Distance;
        const int DefaultPageSize = 5;

        public EstablishmentExtensionsTests()
        {
            _establishments = GetFakeAvailabilitySearch();
            _searchCriteria = new SearchCriteria();
        }

        [Fact]
        public void Filter_FilterEstablishmentsByAnySearchCriteria_ReturnEstablishments()
        {
            // Arrange
            // Act
            var result = _establishments.Filter(_searchCriteria).ToList();

            // Assert
            result.Should().BeOfType<List<Establishment>>();
        }

        [Fact]
        public void Filter_FilterEstablishmentsByName_ReturnEstablishmentsMatchingName()
        {
            // Arrange
            var name = "London";
            _searchCriteria.Name = name;

            // Act
            var result = _establishments.Filter(_searchCriteria).ToList();

            // Assert
            result.Should().OnlyContain(e => e.Name.Contains(name));
        }

        [Fact]
        public void Filter_FilterEstablishmentsByStars_ReturnEstablishmentsMatchingStars()
        {
            // Arrange
            var stars = new int[] { 4, 5 };
            _searchCriteria.Stars = stars;

            // Act
            var result = _establishments.Filter(_searchCriteria).ToList();

            // Assert
            result.Should().OnlyContain(e => stars.Contains(e.Stars));
        }

        [Fact]
        public void Filter_FilterEstablishmentsByUserRating_ReturnEstablishmentsMatchingUserRating()
        {
            // Arrange
            var minUserRating = 7;
            var maxUserRating = 9;
            _searchCriteria.MinUserRating = minUserRating;
            _searchCriteria.MaxUserRating = maxUserRating;

            // Act
            var result = _establishments.Filter(_searchCriteria).ToList();

            // Assert
            result.Should().OnlyContain(e => e.UserRating >= minUserRating && e.UserRating <= maxUserRating);
        }

        [Fact]
        public void Filter_FilterEstablishmentsByMinCost_ReturnEstablishmentsMatchingMinCost()
        {
            // Arrange
            var minCost = 150;
            _searchCriteria.MinCost = minCost;

            // Act
            var result = _establishments.Filter(_searchCriteria).ToList();

            // Assert
            result.Should().OnlyContain(e => e.MinCost >= minCost);
        }

        [Fact]
        public void Sort_SortEstablishmentsByAnySortType_ReturnEstablishments()
        {
            // Arrange
            // Act
            var result = _establishments.Sort(DefaultSearchType).ToList();

            // Assert
            result.Should().BeOfType<List<Establishment>>();
        }

        [Fact]
        public void Sort_SortEstablishmentsByMinCost_ReturnEstablishmentsSortedByMinCost()
        {
            // Arrange
            // Act
            var result = _establishments.Sort(SortType.MinCost).ToList();

            // Assert
            result.Should().BeInAscendingOrder(e => e.MinCost);
        }

        [Fact]
        public void Sort_SortEstablishmentsByUserRating_ReturnEstablishmentsSortedByUserRating()
        {
            // Arrange
            // Act
            var result = _establishments.Sort(SortType.UserRating).ToList();

            // Assert
            result.Should().BeInDescendingOrder(e => e.UserRating);
        }

        [Fact]
        public void Sort_SortEstablishmentsByDistance_ReturnEstablishmentsSortedByDistance()
        {
            // Arrange
            // Act
            var result = _establishments.Sort(SortType.Distance).ToList();

            // Assert
            result.Should().BeInAscendingOrder(e => e.Distance);
        }

        [Fact]
        public void Sort_SortEstablishmentsByStars_ReturnEstablishmentsSortedByStars()
        {
            // Arrange
            // Act
            var result = _establishments.Sort(SortType.Stars).ToList();

            // Assert
            result.Should().BeInDescendingOrder(e => e.Stars);
        }

        [Fact]
        public void Page_PageEstablishments_ReturnEstablishments()
        {
            // Arrange
            // Act
            var result = _establishments.Page(1, DefaultPageSize).ToList();

            // Assert
            result.Should().BeOfType<List<Establishment>>();
        }

        [Fact]
        public void Page_PageEstablishments_ReturnEstablishmentsPagedByPageSize()
        {
            // Arrange
            // Act
            var result = _establishments.Page(1, DefaultPageSize).ToList();

            // Assert
            result.Should().HaveCount(DefaultPageSize);
        }

        [Fact]
        public void Page_PageEstablishments_ReturnEstablishmentsPagedByPageIndex()
        {
            // Arrange
            var pageIndex = 2;
            var skipByIndex = (pageIndex - 1) * DefaultPageSize;
            var firstEstablishmentNameOnPage = _establishments[skipByIndex].Name;

            // Act
            var result = _establishments.Page(pageIndex, DefaultPageSize).ToList();

            // Assert
            result.First().Name.Should().Be(firstEstablishmentNameOnPage);
        }

        [Fact]
        public void PageCount_GetEstablishmentsPageCount_ReturnEstablishmentsPageCount()
        {
            // Arrange
            var expectedPageCount = (int)Math.Ceiling(_establishments.Count() / (double)DefaultPageSize);

            // Act
            var result = _establishments.PageCount(DefaultPageSize);

            // Assert
            result.Should().Be(expectedPageCount);
        }

        private List<Establishment> GetFakeAvailabilitySearch()
        {
            return new List<Establishment>()
            {
                new Establishment() { Name = "Hotel Paris", Stars = 5, Location = "Paris", UserRating = 10, MinCost = 250 },
                new Establishment() { Name = "Hotel London", Stars = 2, Location = "London", UserRating = 9, MinCost = 150 },
                new Establishment() { Name = "Hotel Rome", Stars = 4, Location = "Rome", UserRating = 8, MinCost = 100 },
                new Establishment() { Name = "Hotel Madrid", Stars = 3, Location = "Madrid", UserRating = 7, MinCost = 125 },
                new Establishment() { Name = "Hotel Athens", Stars = 1, Location = "Athens", UserRating = 9, MinCost = 200 },
                new Establishment() { Name = "Hotel Barcelona", Stars = 5, Location = "Barcelona", UserRating = 10, MinCost = 250 },
                new Establishment() { Name = "Hotel Geneva", Stars = 2, Location = "Geneva", UserRating = 9, MinCost = 150 },
                new Establishment() { Name = "Hotel Brussels", Stars = 4, Location = "Brussels", UserRating = 8, MinCost = 100 },
                new Establishment() { Name = "Hotel Amsterdam", Stars = 3, Location = "Copenhagen", UserRating = 7, MinCost = 125 },
                new Establishment() { Name = "Hotel Florence", Stars = 1, Location = "Florence", UserRating = 9, MinCost = 200 }
            };
        }
    }
}
