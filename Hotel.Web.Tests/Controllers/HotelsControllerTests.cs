using AutoMapper;
using FluentAssertions;
using Hotel.Web.Controllers;
using Hotel.Web.Core.Models;
using Hotel.Web.Core.Repositories;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Hotel.Web.Tests.Controllers
{
    public class HotelsControllerTests
    {
        private HotelsController _controller;
        private Mock<IHotelRepository> _mockRepository;
        private Mock<ILogger<HotelsController>> _mockLogger;
        private Mock<IOptions<PageSettings>> _mockOptions;
        private PageSettings _pageSettings;

        public HotelsControllerTests()
        {
            _mockRepository = new Mock<IHotelRepository>();

            _mockLogger = new Mock<ILogger<HotelsController>>();

            _mockOptions = new Mock<IOptions<PageSettings>>();
            _pageSettings = new PageSettings() { PageSize = 5 };
            _mockOptions.Setup(p => p.Value).Returns(_pageSettings);

            _controller = new HotelsController(_mockRepository.Object, _mockLogger.Object, _mockOptions.Object);

            Mapper.Initialize(mapConfig =>
            {
                mapConfig.CreateMap<SearchCriteriaViewModel, SearchCriteria>();
                mapConfig.CreateMap<Establishment, EstablishmentViewModel>();
                mapConfig.CreateMap<AvailabilitySearch, AvailabilitySearchViewModel>();
            });
        }

        [Fact]
        public void Index_AnySearchCriteriaAndEmptyRepository_ReturnValidViewResultAndViewModel()
        {
            // Arrange
            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => new AvailabilitySearch());
            var controller = new HotelsController(mockRepository.Object, _mockLogger.Object, _mockOptions.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeOfType<AvailabilitySearchViewModel>();
        }

        [Fact]
        public void Index_AnySearchCriteriaAndNonEmptyRepository_ReturnValidViewResultAndViewModel()
        {
            // Arrange
            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => GetMockSearchResults());
            var controller = new HotelsController(mockRepository.Object, _mockLogger.Object, _mockOptions.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeOfType<AvailabilitySearchViewModel>();
        }

        [Fact]
        public void Results_AnySearchCriteriaAndEmptyRepository_ReturnValidPartialViewResultAndViewModel()
        {
            // Arrange
            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => new AvailabilitySearch());
            var controller = new HotelsController(mockRepository.Object, _mockLogger.Object, _mockOptions.Object);

            // Act
            var result = controller.Results(new SearchCriteriaViewModel());

            // Assert
            var partialViewResult = result.Should().BeOfType<PartialViewResult>().Subject;
            partialViewResult.Model.Should().BeOfType<AvailabilitySearchViewModel>();
        }

        [Fact]
        public void Results_AnySearchCriteriaAndNonEmptyRepository_ReturnValidPartialViewResultAndViewModel()
        {
            // Arrange
            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => GetMockSearchResults());
            var controller = new HotelsController(mockRepository.Object, _mockLogger.Object, _mockOptions.Object);

            // Act
            var result = controller.Results(new SearchCriteriaViewModel());

            // Assert
            var partialViewResult = result.Should().BeOfType<PartialViewResult>().Subject;
            partialViewResult.Model.Should().BeOfType<AvailabilitySearchViewModel>();
        }

        private AvailabilitySearch GetMockSearchResults()
        {
            // TODO https://www.davepaquette.com/archive/2015/11/15/realistic-sample-data-with-genfu.aspx

            return new AvailabilitySearch()
            {
                Establishments = new List<Establishment>()
                {
                    new Establishment() { Name = "Hotel 1", Stars = 1, Location = "Town 1" },
                    new Establishment() { Name = "Hotel 2", Stars = 2, Location = "Town 2" },
                    new Establishment() { Name = "Hotel 3", Stars = 3, Location = "Town 3" },
                    new Establishment() { Name = "Hotel 4", Stars = 4, Location = "Town 4" },
                    new Establishment() { Name = "Hotel 5", Stars = 5, Location = "Town 5" },
                },
                PageIndex = 1,
                PageCount = 1,
                TotalResults = 5
            };
        }
    }
}
