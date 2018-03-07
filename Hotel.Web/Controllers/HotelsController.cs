using AutoMapper;
using Hotel.Web.Core.Models;
using Hotel.Web.Core.Repositories;
using Hotel.Web.Core.Services.Logging;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace Hotel.Web.Controllers
{
    public class HotelsController : Controller
    {
        private readonly IHotelRepository _repository;
        private readonly ILoggerAdapter<HotelsController> _logger;
        private readonly PageSettings _pageSettings;

        public HotelsController(IHotelRepository repository, ILoggerAdapter<HotelsController> logger, IOptions<PageSettings> pageSettings)
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
            try
            {
                if (ModelState.IsValid)
                {
                    var availability = _repository.GetHotels(Mapper.Map<SearchCriteria>(criteria), _pageSettings.PageSize);

                    var results = Mapper.Map<AvailabilitySearchViewModel>(availability);

                    return PartialView("HotelResults", results);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Hotel search results: {ex}");
            }

            return PartialView("HotelResults", null);
        }
    }
}
