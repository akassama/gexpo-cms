using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NgoExpoApp.Models;
using GexpoTechCMS.Models.AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace NgoExpoApp.Controllers
{
    public class SearchController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<SearchController> _logger;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;
        private readonly SessionManager _sessionManager;

        public SearchController(DBConnection context, ILogger<SearchController> logger, IOptions<SystemConfiguration> systemConfiguration, IDetectionService detectionService, IHttpContextAccessor accessor, SessionManager sessionManager)
        {
            _context = context;
            _logger = logger;
            _systemConfiguration = systemConfiguration.Value;
            _detectionService = detectionService;
            _accessor = accessor;
            _sessionManager = sessionManager;
        }

        public IActionResult Index([FromQuery(Name = "p")] string p = "1", [FromQuery(Name = "s")] string s = "10", [FromQuery(Name = "q")] string q = null)
        {
            if (string.IsNullOrEmpty(q) || q == "Index")
            {
                return RedirectToAction("Index", "Home");
            }

            if (q.Length < 2)
            {
                TempData["ErrorMessage"] = "Query too short...";
                return RedirectToAction("Index", "Home");
            }

            //log visit
            string VisitorIP = functions.FormatVisitorIP(_sessionManager.SessionIP, _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
            functions.LogVisit(q, "SearchQuery", VisitorIP, _detectionService.Browser.Name.ToString(), _detectionService.Device.Type.ToString());

            ViewBag.SearchQuery = q;

            //set default pagination values
            ViewBag.PageNo = 1;
            ViewBag.PageSize = 10;
            if (!string.IsNullOrEmpty(p) && !string.IsNullOrEmpty(s))
            {
                ViewBag.PageNo = Int32.Parse(p);
                ViewBag.PageSize = Int32.Parse(s);
            }

            ViewBag.PageSkip = (ViewBag.PageNo - 1) * ViewBag.PageSize;
            int PageSkip = ViewBag.PageSkip;
            int PageSize = ViewBag.PageSize;


            //set empty list
            List<SearchResultsModel> IList = new List<SearchResultsModel>()
            {

            };

            //get data for posts
            var PostsData = _context.Posts.Where(s => s.Status == 1 && (s.Title.Contains(q) || s.ShortDescription.Contains(q) ||
                    s.Categories.Contains(q) || s.Content.Contains(q) || s.PostTags.Contains(q) || s.SEOKeywords.Contains(q))).OrderByDescending(x => x.CreatedAt);

            ViewBag.TotalPostsData = PostsData.Count();

            //get data for pages
            var PagesData = _context.Pages.Where(s => s.Status == 1 && (s.Title.Contains(q) || s.ShortDescription.Contains(q) || s.Content.Contains(q) || 
                    s.SEOKeywords.Contains(q))).OrderByDescending(x => x.CreatedAt);

            ViewBag.TotalPagesData = PagesData.Count();

            //get total records for pagination
            ViewBag.TotalRecords = ViewBag.TotalPostsData + ViewBag.TotalPagesData;

            //add posts records
            foreach (var item in PostsData)
            {
                //add values to list
                IList.Add(
                        new SearchResultsModel
                        {
                            Title = item.Title,
                            Summary = item.ShortDescription,
                            Link = $"/Post/{item.Slug}",
                            UpdatedAt = item.UpdatedAt,
                        }
                   );
            }

            //add pages records
            foreach (var item in PagesData)
            {
                //add values to list
                IList.Add(
                        new SearchResultsModel
                        {
                            Title = item.Title,
                            Summary = item.ShortDescription,
                            Link = $"/Page/{item.Slug}",
                            UpdatedAt = item.UpdatedAt,
                        }
                   );
            }

            //sort and take required number from list
            IList.OrderByDescending(s => s.UpdatedAt).Skip(PageSkip).Take(PageSize).ToList();

            //get popular posts
            ViewBag.PopularPostsData = _context.PopularThisWeek.OrderByDescending(x => x.ValueOccurrence).Take(8);
            ViewBag.PopularPostsCount = _context.PopularThisWeek.Count();

            //get popular categories
            ViewBag.PopularCategories = functions.RemoveDuplicateCSV(functions.GetAllPopularCategories());

            //get popular tags
            ViewBag.PopularTags = functions.RemoveDuplicateCSV(functions.GetAllPopularTags());

            return View(IList);
        }
    }
}
