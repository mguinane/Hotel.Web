using FluentAssertions;
using Hotel.Web.Core.Models;
using Hotel.Web.Core.Models.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Hotel.Web.Tests.Core.Models.Extensions
{
    public class EstablishmentExtensionsTests
    {
        [Fact]
        public void Filter_FilterEstablishmentsByName_ReturnEstablishmentsMatchingName()
        {
            // Arrange
            var establishments = GetMockEstablishments();
            var name = "London";
            var searchCriteria = new SearchCriteria() { Name = name };

            // Act
            var result = establishments.Filter(searchCriteria).ToList();

            // Assert
            var filteredResult = result.Should().BeOfType<List<Establishment>>().Subject;
            filteredResult.Should().OnlyContain(e => e.Name.Contains(name));
        }

        [Fact]
        public void Filter_FilterEstablishmentsByStars_ReturnEstablishmentsMatchingStars()
        {
            // Arrange
            var establishments = GetMockEstablishments();
            var stars = new int[] { 4, 5 };
            var searchCriteria = new SearchCriteria() { Stars = stars };

            // Act
            var result = establishments.Filter(searchCriteria).ToList();

            // Assert
            var filteredResult = result.Should().BeOfType<List<Establishment>>().Subject;
            filteredResult.Should().OnlyContain(e => stars.Contains(e.Stars));
        }

        [Fact]
        public void Filter_FilterEstablishmentsByUserRating_ReturnEstablishmentsMatchingUserRating()
        {
            // Arrange
            var establishments = GetMockEstablishments();
            var minUserRating = 7;
            var maxUserRating = 9;
            var searchCriteria = new SearchCriteria() { MinUserRating = minUserRating, MaxUserRating = maxUserRating };

            // Act
            var result = establishments.Filter(searchCriteria).ToList();

            // Assert
            var filteredResult = result.Should().BeOfType<List<Establishment>>().Subject;
            filteredResult.Should().OnlyContain(e => e.UserRating >= minUserRating && e.UserRating <= maxUserRating);
        }

        [Fact]
        public void Filter_FilterEstablishmentsByMinCost_ReturnEstablishmentsMatchingMinCost()
        {
            // Arrange
            var establishments = GetMockEstablishments();
            var minCost = 150;
            var searchCriteria = new SearchCriteria() { MinCost = minCost };

            // Act
            var result = establishments.Filter(searchCriteria).ToList();

            // Assert
            var filteredResult = result.Should().BeOfType<List<Establishment>>().Subject;
            filteredResult.Should().OnlyContain(e => e.MinCost >= minCost);
        }

        [Fact]
        public void Sort_SortEstablishmentsByMinCost_ReturnEstablishmentsSortedByMinCost()
        {
            // Arrange
            var establishments = GetMockEstablishments();

            // Act
            var result = establishments.Sort(SortType.MinCost).ToList();

            // Assert
            var filteredResult = result.Should().BeOfType<List<Establishment>>().Subject;
            filteredResult.Should().BeInAscendingOrder(e => e.MinCost);
        }

        [Fact]
        public void Sort_SortEstablishmentsByUserRating_ReturnEstablishmentsSortedByUserRating()
        {
            // Arrange
            var establishments = GetMockEstablishments();

            // Act
            var result = establishments.Sort(SortType.UserRating).ToList();

            // Assert
            var filteredResult = result.Should().BeOfType<List<Establishment>>().Subject;
            filteredResult.Should().BeInDescendingOrder(e => e.UserRating);
        }

        [Fact]
        public void Sort_SortEstablishmentsByDistance_ReturnEstablishmentsSortedByDistance()
        {
            // Arrange
            var establishments = GetMockEstablishments();

            // Act
            var result = establishments.Sort(SortType.Distance).ToList();

            // Assert
            var filteredResult = result.Should().BeOfType<List<Establishment>>().Subject;
            filteredResult.Should().BeInAscendingOrder(e => e.Distance);
        }

        [Fact]
        public void Sort_SortEstablishmentsByStarts_ReturnEstablishmentsSortedByStars()
        {
            // Arrange
            var establishments = GetMockEstablishments();

            // Act
            var result = establishments.Sort(SortType.Stars).ToList();

            // Assert
            var filteredResult = result.Should().BeOfType<List<Establishment>>().Subject;
            filteredResult.Should().BeInDescendingOrder(e => e.Stars);
        }

        private IEnumerable<Establishment> GetMockEstablishments()
        {
            return new List<Establishment>()
            {
                new Establishment() { Name = "Hotel Paris", Stars = 5, Location = "Paris", UserRating = 10, MinCost = 250 },
                new Establishment() { Name = "Hotel London", Stars = 2, Location = "London", UserRating = 9, MinCost = 150 },
                new Establishment() { Name = "Hotel Rome", Stars = 4, Location = "Rome", UserRating = 8, MinCost = 100 },
                new Establishment() { Name = "Hotel Madrid", Stars = 3, Location = "Madrid", UserRating = 7, MinCost = 125 },
                new Establishment() { Name = "Hotel Athens", Stars = 1, Location = "Athens", UserRating = 9, MinCost = 200 }
            };
        }
    }
}
