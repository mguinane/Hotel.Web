using AutoMapper;
using Hotel.Web.Data;
using Hotel.Web.Models;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Hotel.Web.Controllers
{
    public class HotelsController : Controller
    {
        private readonly IHotelRepository _repository;
        private readonly ILogger<HotelsController> _logger;

        private readonly int _pageSize = int.Parse(Startup.Configuration["pageSettings:pageSize"]);

        public HotelsController(IHotelRepository repository, ILogger<HotelsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var availability = _repository.GetHotels(new SearchResultsCriteria() { PageIndex = 1, SortType = (int)SortType.Distance }, _pageSize);

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
        public IActionResult Results([FromBody]SearchResultsCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                var availability = _repository.GetHotels(criteria, _pageSize);

                var results = Mapper.Map<AvailabilitySearchViewModel>(availability);

                return PartialView("HotelResults", results);
            }

            return PartialView("HotelResults", null);
        }
    }
}
