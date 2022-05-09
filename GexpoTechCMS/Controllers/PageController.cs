using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NgoExpoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace NgoExpoApp.Controllers
{
    public class PageController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<PageController> _logger;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;
        private readonly SessionManager _sessionManager;

        public PageController(DBConnection context, ILogger<PageController> logger, IOptions<SystemConfiguration> systemConfiguration, IDetectionService detectionService, IHttpContextAccessor accessor, SessionManager sessionManager)
        {
            _context = context;
            _logger = logger;
            _systemConfiguration = systemConfiguration.Value;
            _detectionService = detectionService;
            _accessor = accessor;
            _sessionManager = sessionManager;
        }

        //Page details
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Admin");
            }

            //log visit
            string VisitorIP = functions.FormatVisitorIP(_sessionManager.SessionIP, _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
            functions.LogVisit(id, "PageDetails", VisitorIP, _detectionService.Browser.Name.ToString(), _detectionService.Device.Type.ToString());

            ViewBag.ShowOGProperty = "true";

            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            //Set mata & properties
            ViewData["ContentKeywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.organizationName);
            ViewData["ContentDescription"] = functions.GetCmsData("SEODescription", _systemConfiguration.organizationName);
            ViewData["PostAuthor"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.organizationName);
            ViewData["OrganizationName"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["PropertyImage"] = AppSettings.BaseUrl() + "/images/" + functions.GetCmsData("Favicon", "default-favicon.jpg");
            ViewData["PropertyDescription"] = "By " + functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["PropertySection"] = null;
            ViewData["PropertyUpdatedTime"] = DateTime.Now;

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var pagesModel = await _context.Pages
               .FirstOrDefaultAsync(m => m.Slug == id);
            if (pagesModel == null)
            {
                return NotFound();
            }

            //get popular posts
            ViewBag.PopularPostsData = _context.PopularThisWeek.OrderByDescending(x => x.ValueOccurrence).Take(8);
            ViewBag.PopularPostsCount = _context.PopularThisWeek.Count();

            //get popular categories
            ViewBag.PopularCategories = functions.RemoveDuplicateCSV(functions.GetAllPopularCategories());

            //get popular tags
            ViewBag.PopularTags = functions.RemoveDuplicateCSV(functions.GetAllPopularTags());

            return View(pagesModel);
        }


        //ovveride NotFound() to E404 error page
        public new IActionResult NotFound()
        {
            return RedirectToAction("E404", "Error");
        }

    }
}
