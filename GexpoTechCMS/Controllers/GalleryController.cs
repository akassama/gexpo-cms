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
    public class GalleryController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<GalleryController> _logger;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly AppMessages _appMessages;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;
        private readonly SessionManager _sessionManager;

        public GalleryController(DBConnection context, ILogger<GalleryController> logger, IOptions<SystemConfiguration> systemConfiguration, IOptions<AppMessages> appMessages, IDetectionService detectionService, IHttpContextAccessor accessor, SessionManager sessionManager)
        {
            _context = context;
            _logger = logger;
            _systemConfiguration = systemConfiguration.Value;
            _appMessages = appMessages.Value;
            _detectionService = detectionService;
            _accessor = accessor;
            _sessionManager = sessionManager;
        }

        public IActionResult Index()
        {
            //log visit
            string VisitorIP = functions.FormatVisitorIP(_sessionManager.SessionIP, _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
            functions.LogVisit(null, "Home", VisitorIP, _detectionService.Browser.Name.ToString(), _detectionService.Device.Type.ToString());

            ViewBag.ShowOGProperty = "false";

            //Set mata & properties
            ViewData["ContentKeywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.organizationName);
            ViewData["ContentDescription"] = functions.GetCmsData("SEODescription", _systemConfiguration.organizationName);
            ViewData["PostAuthor"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.organizationName);
            ViewData["OrganizationName"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["PropertyImage"] = AppSettings.BaseUrl() + "/images/" + functions.GetCmsData("Favicon", "default-favicon.jpg");
            ViewData["PropertyDescription"] = "By " + functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["PropertySection"] = null;
            ViewData["PropertyUpdatedTime"] = DateTime.Now;

            //sets sessions
            if (string.IsNullOrEmpty(_sessionManager.SessionLanguage))
            {
                _sessionManager.SessionLanguage = "en";
            }

            return View();
        }
    }
}
