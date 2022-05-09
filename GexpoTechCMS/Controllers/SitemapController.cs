using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NgoExpoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace GexpoTech.Controllers
{
    public class SitemapController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<SitemapController> _logger;
        private readonly SessionManager _sessionManager;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly AppMessages _appMessages;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;

        public SitemapController(DBConnection context, ILogger<SitemapController> logger, SessionManager sessionManager, IOptions<SystemConfiguration> systemConfiguration, IOptions<AppMessages> appMessages, IDetectionService detectionService, IHttpContextAccessor accessor)
        {
            _sessionManager = sessionManager;
            _context = context;
            _logger = logger;
            _systemConfiguration = systemConfiguration.Value;
            _appMessages = appMessages.Value;
            _detectionService = detectionService;
            _accessor = accessor;
        }

        public IActionResult Index()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["Description"] = functions.GetCmsData("SEODescription", _systemConfiguration.seoDescription);
            ViewData["Author"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.seoAuthor);
            ViewData["Keywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.seoKeywords);

            ViewBag.Posts = _context.Posts.Where(s => s.Status == 1).OrderByDescending(s => s.CreatedAt).Take(50).ToList();
            ViewBag.Navigations = _context.Navigations.Where(s => s.Status == 1).OrderBy(s => s.NavigationName).ToList();
            ViewBag.Categories = _context.Categories.Where(s => s.Status == 1).OrderBy(s => s.CategoryName).ToList();
            ViewBag.Teams = _context.Teams.Where(s => s.Status == 1).OrderBy(s => s.FirstName).ToList();
            ViewBag.PopularTags = functions.RemoveDuplicateCSV(functions.GetAllPopularTags());

            return View();
        }
    }
}
