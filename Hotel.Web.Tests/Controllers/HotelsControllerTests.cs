using AutoMapper;
using FluentAssertions;
using Hotel.Web.Controllers;
using Hotel.Web.Core.Models;
using Hotel.Web.Core.Repositories;
using Hotel.Web.Infrastructure.Services.Logging;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Hotel.Web.Tests.Controllers
{
    public class HotelsControllerTests
    {
        private HotelsController _controller;
        private Mock<IHotelRepository> _mockRepository;
        private Mock<ILoggerAdapter<HotelsController>> _mockLogger;
        private Mock<IOptions<PageSettings>> _mockOptions;
        private PageSettings _pageSettings;

        public HotelsControllerTests()
        {
            _mockRepository = new Mock<IHotelRepository>();

            _mockLogger = new Mock<ILoggerAdapter<HotelsController>>();

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
            mockRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => GetMockAvailabilitySearch());
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
            mockRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => GetMockAvailabilitySearch());
            var controller = new HotelsController(mockRepository.Object, _mockLogger.Object, _mockOptions.Object);

            // Act
            var result = controller.Results(new SearchCriteriaViewModel());

            // Assert
            var partialViewResult = result.Should().BeOfType<PartialViewResult>().Subject;
            partialViewResult.Model.Should().BeOfType<AvailabilitySearchViewModel>();
        }

        [Fact]
        public void Index_RepositoryThrowsException_ReturnRedirectToErrorActionResult()
        {
            // Arrange
            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Throws<Exception>();
            var controller = new HotelsController(mockRepository.Object, _mockLogger.Object, _mockOptions.Object);

            // Act
            var result = controller.Index();

            // Assert
            var redirectToActionResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectToActionResult.ActionName.Should().Be("Error");
        }

        [Fact]
        public void Results_RepositoryThrowsException_ReturnPartialViewResultAndNullViewModel()
        {
            // Arrange
            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Throws<Exception>();
            var controller = new HotelsController(mockRepository.Object, _mockLogger.Object, _mockOptions.Object);

            // Act
            var result = controller.Results(new SearchCriteriaViewModel());

            // Assert
            var partialViewResult = result.Should().BeOfType<PartialViewResult>().Subject;
            partialViewResult.Model.Should().BeNull();
        }

        [Fact]
        public void Results_ModelStateInvalid_ReturnPartialViewResultAndNullViewModel()
        {
            // Arrange
            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => GetMockAvailabilitySearch());
            var controller = new HotelsController(mockRepository.Object, _mockLogger.Object, _mockOptions.Object);
            controller.ModelState.AddModelError("Name", "name property is too long");

            // Act
            var result = controller.Results(new SearchCriteriaViewModel());

            // Assert
            var partialViewResult = result.Should().BeOfType<PartialViewResult>().Subject;
            partialViewResult.Model.Should().BeNull();
        }

        [Fact]
        public void Index_ExceptionThrown_ErrorLogged()
        {
            // Arrange
            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Throws<Exception>();
            var controller = new HotelsController(mockRepository.Object, _mockLogger.Object, _mockOptions.Object);

            // Act
            var result = controller.Index();

            // Assert
            _mockLogger.Verify(l => l.LogError(It.IsAny<string>()));
        }

        [Fact]
        public void Results_ExceptionThrown_ErrorLogged()
        {
            // Arrange
            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Throws<Exception>();
            var controller = new HotelsController(mockRepository.Object, _mockLogger.Object, _mockOptions.Object);

            // Act
            var result = controller.Results(new SearchCriteriaViewModel());

            // Assert
            _mockLogger.Verify(l => l.LogError(It.IsAny<string>()));
        }

        private AvailabilitySearch GetMockAvailabilitySearch()
        {
            var establishments = new List<Establishment>()
            {
                new Establishment() { Name = "Hotel 1", Stars = 1, Location = "Town 1" },
                new Establishment() { Name = "Hotel 2", Stars = 2, Location = "Town 2" },
                new Establishment() { Name = "Hotel 3", Stars = 3, Location = "Town 3" },
                new Establishment() { Name = "Hotel 4", Stars = 4, Location = "Town 4" },
                new Establishment() { Name = "Hotel 5", Stars = 5, Location = "Town 5" },
            };

            return new AvailabilitySearch("1", establishments, 1, 1);
        }
    }
}
