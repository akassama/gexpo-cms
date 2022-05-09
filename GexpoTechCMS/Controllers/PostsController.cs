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
    public class PostsController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<PostsController> _logger;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;
        private readonly SessionManager _sessionManager;

        public PostsController(DBConnection context, ILogger<PostsController> logger, IOptions<SystemConfiguration> systemConfiguration, IDetectionService detectionService, IHttpContextAccessor accessor, SessionManager sessionManager)
        {
            _context = context;
            _logger = logger;
            _systemConfiguration = systemConfiguration.Value;
            _detectionService = detectionService;
            _accessor = accessor;
            _sessionManager = sessionManager;
        }

        //Posts view
        [Route("Posts")]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] string p = "1", [FromQuery(Name = "s")] string s = "12")
        {
            //if posts disabled
            if (!functions.ContentDisplay("Posts"))
            {
                return RedirectToAction("Index", "Home");
            }

            //log visit
            string VisitorIP = functions.FormatVisitorIP(_sessionManager.SessionIP, _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
            functions.LogVisit(null, "Posts", VisitorIP, _detectionService.Browser.Name.ToString(), _detectionService.Device.Type.ToString());

            ViewBag.ShowOGProperty = "false";
            ViewBag.ActiveNav = "Posts";

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
            ViewBag.TotalRecords = _context.Posts.Count(s => s.Status == 1);

            var postsModel = _context.Posts.Where(s => s.Status == 1).OrderByDescending(s => s.ID).Skip(PageSkip).Take(PageSize);

            //get popular posts
            ViewBag.PopularPostsData = _context.PopularThisWeek.OrderByDescending(x => x.ValueOccurrence).Take(8);
            ViewBag.PopularPostsCount = _context.PopularThisWeek.Count();

            //get popular categories
            ViewBag.PopularCategories = functions.RemoveDuplicateCSV(functions.GetAllPopularCategories());

            //get popular tags
            ViewBag.PopularTags = functions.RemoveDuplicateCSV(functions.GetAllPopularTags());

            return View(await postsModel.ToListAsync());
        }

    }
}
