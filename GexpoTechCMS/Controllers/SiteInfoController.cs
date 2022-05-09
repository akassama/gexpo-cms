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

namespace GexpoTechCMS.Controllers
{
    public class SiteInfoController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<SiteInfoController> _logger;
        private readonly SessionManager _sessionManager;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly AppMessages _appMessages;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;

        public SiteInfoController(DBConnection context, ILogger<SiteInfoController> logger, SessionManager sessionManager, IOptions<SystemConfiguration> systemConfiguration, IOptions<AppMessages> appMessages, IDetectionService detectionService, IHttpContextAccessor accessor)
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
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Support view
        /// </summary>
        /// <returns></returns>
        public IActionResult Support()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["Description"] = functions.GetCmsData("SEODescription", _systemConfiguration.seoDescription);
            ViewData["Author"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.seoAuthor);
            ViewData["Keywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.seoKeywords);

            return View();
        }

        /// <summary>
        /// Help Center view
        /// </summary>
        /// <returns></returns>
        public IActionResult HelpCenter()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["Description"] = functions.GetCmsData("SEODescription", _systemConfiguration.seoDescription);
            ViewData["Author"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.seoAuthor);
            ViewData["Keywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.seoKeywords);

            return View();
        }

        /// <summary>
        /// Privacy view
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["Description"] = functions.GetCmsData("SEODescription", _systemConfiguration.seoDescription);
            ViewData["Author"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.seoAuthor);
            ViewData["Keywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.seoKeywords);

            return View();
        }

        /// <summary>
        /// Terms view
        /// </summary>
        /// <returns></returns>
        public IActionResult Terms()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["Description"] = functions.GetCmsData("SEODescription", _systemConfiguration.seoDescription);
            ViewData["Author"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.seoAuthor);
            ViewData["Keywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.seoKeywords);

            return View();
        }
    }
}
