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
    public class PostController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<PostController> _logger;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;
        private readonly SessionManager _sessionManager;

        public PostController(DBConnection context, ILogger<PostController> logger, IOptions<SystemConfiguration> systemConfiguration, IDetectionService detectionService, IHttpContextAccessor accessor, SessionManager sessionManager)
        {
            _context = context;
            _logger = logger;
            _systemConfiguration = systemConfiguration.Value;
            _detectionService = detectionService;
            _accessor = accessor;
            _sessionManager = sessionManager;
        }

        //Post details
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Admin");
            }


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

            //get other recent posts
            ViewBag.RecentPosts = _context.Posts.Where(s => s.Slug != id && s.Status == 1).OrderByDescending(x => x.CreatedAt).Take(4);
            ViewBag.RecentPostsCount = _context.Posts.Count(s => s.Slug != id && s.Status == 1);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var postsModel = await _context.Posts
               .FirstOrDefaultAsync(m => m.Slug == id);
            if (postsModel == null)
            {
                return NotFound();
            }

            //log visit
            string VisitorIP = functions.FormatVisitorIP(_sessionManager.SessionIP, _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
            string DocumentID = _context.Posts.Where(s => s.Slug == id).FirstOrDefault().PostID;
            functions.LogVisit(DocumentID, "PostDetails", VisitorIP, _detectionService.Browser.Name.ToString(), _detectionService.Device.Type.ToString());

            //get popular posts
            ViewBag.PopularPostsData = _context.PopularThisWeek.OrderByDescending(x => x.ValueOccurrence).Take(8);
            ViewBag.PopularPostsCount = _context.PopularThisWeek.Count();

            //get popular categories
            ViewBag.PopularCategories = functions.RemoveDuplicateCSV(functions.GetAllPopularCategories());

            //get popular tags
            ViewBag.PopularTags = functions.RemoveDuplicateCSV(functions.GetAllPopularTags());

            return View(postsModel);
        }


        //ovveride NotFound() to E404 error page
        public new IActionResult NotFound()
        {
            return RedirectToAction("E404", "Error");
        }
    }


}
