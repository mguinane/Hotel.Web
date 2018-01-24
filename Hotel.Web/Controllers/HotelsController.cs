using AutoMapper;
using Hotel.Web.Core.Models;
using Hotel.Web.Core.Repositories;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Hotel.Web.Controllers
{
    public class HotelsController : Controller
    {
        private readonly IHotelRepository _repository;
        private readonly ILogger<HotelsController> _logger;
        private readonly PageSettings _pageSettings;

        public HotelsController(IHotelRepository repository, ILogger<HotelsController> logger, IOptions<PageSettings> pageSettings)
        {
            _repository = repository;
            _logger = logger;
            _pageSettings = pageSettings.Value;
        }

        public IActionResult Index()
        {
            try
            {
                var availability = _repository.GetHotels(new SearchCriteria(), _pageSettings.PageSize);

                var results = Mapper.Map<AvailabilitySearchViewModel>(availability);

                return View(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Hotel search results: {ex}");
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public IActionResult Results([FromBody]SearchCriteriaViewModel criteria)
        {
            if (ModelState.IsValid)
            {
                var availability = _repository.GetHotels(Mapper.Map<SearchCriteria>(criteria), _pageSettings.PageSize);

                var results = Mapper.Map<AvailabilitySearchViewModel>(availability);

                return PartialView("HotelResults", results);
            }

            return PartialView("HotelResults", null);
        }
    }
}
