using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NgoExpoApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace GexpoTechCMS.Controllers
{
    public class ResourcesController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<ResourcesController> _logger;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly AppMessages _appMessages;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;
        private readonly SessionManager _sessionManager;
        // import hosting envoroment and set global variable
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ResourcesController(DBConnection context, ILogger<ResourcesController> logger, IOptions<SystemConfiguration> systemConfiguration, IOptions<AppMessages> appMessages, IDetectionService detectionService, IHttpContextAccessor accessor, SessionManager sessionManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _logger = logger;
            _systemConfiguration = systemConfiguration.Value;
            _appMessages = appMessages.Value;
            _detectionService = detectionService;
            _accessor = accessor;
            _sessionManager = sessionManager;
            _hostingEnvironment = hostingEnvironment;
        }


        public async Task<IActionResult> Index([FromQuery(Name = "p")] string p = "1", [FromQuery(Name = "s")] string s = "12") 
        {

            //if posts disabled, category also disabled
            if (!functions.ContentDisplay("Posts"))
            {
                return RedirectToAction("Index", "Home");
            }

            //log visit
            string VisitorIP = functions.FormatVisitorIP(_sessionManager.SessionIP, _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
            functions.LogVisit(null, "Resources", VisitorIP, _detectionService.Browser.Name.ToString(), _detectionService.Device.Type.ToString());

            ViewBag.ShowOGProperty = "false";
            ViewBag.ActiveNav = "Resources";

            //Set mata & properties
            ViewData["ContentKeywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.organizationName);
            ViewData["ContentDescription"] = functions.GetCmsData("SEODescription", _systemConfiguration.organizationName);
            ViewData["PostAuthor"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.organizationName);
            ViewData["OrganizationName"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["PropertyImage"] = AppSettings.BaseUrl() + "/images/" + functions.GetCmsData("Favicon", "default-favicon.jpg");
            ViewData["PropertyDescription"] = "By " + functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["PropertySection"] = null;
            ViewData["PropertyUpdatedTime"] = DateTime.Now;

            ViewBag.PageNo = 1;
            ViewBag.PageSize = 12;
            if (!string.IsNullOrEmpty(p) && !string.IsNullOrEmpty(s))
            {
                ViewBag.PageNo = Int32.Parse(p);
                ViewBag.PageSize = Int32.Parse(s);
            }
            ViewBag.PageSkip = (ViewBag.PageNo - 1) * ViewBag.PageSize;
            int PageSkip = ViewBag.PageSkip;
            int PageSize = ViewBag.PageSize;

            ViewBag.TotalRecords = _context.DocumentResources.Count(s => s.Status == 1);

            var resourcesModel = _context.DocumentResources.Where(s => s.Status == 1).OrderByDescending(s => s.ID).Skip(PageSkip).Take(PageSize);

            return View(await resourcesModel.ToListAsync());
        }


        /// <summary>
        /// Downlod file action
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        [HttpGet("DownloadFile/{docId}")]
        public IActionResult DownloadFile(string docId)
        {
            try
            {
                var DBQuery = _context.DocumentResources.Where(s => s.DocumentID == docId);
                string fileName = DBQuery.FirstOrDefault().FileName;
                string directoryName = Convert.ToDateTime(DBQuery.FirstOrDefault().CreatedAt).ToString("MM-yyyy");
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath + $"\\resources\\documents\\{directoryName}", fileName);

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                return File(fileBytes, "application/force-download", fileName);
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Download Resource Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);

                return RedirectToAction("Index");
            }
        }
    }
}
