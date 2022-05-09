using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NgoExpoApp.Models;
using NgoExpoApp.Models.Emailer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Wangkanai.Detection.Services;

namespace NgoExpoApp.Controllers
{
    public class HomeController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<HomeController> _logger;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly AppMessages _appMessages;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;
        private readonly SessionManager _sessionManager;

        public HomeController(DBConnection context, ILogger<HomeController> logger, IOptions<SystemConfiguration> systemConfiguration, IOptions<AppMessages> appMessages, IDetectionService detectionService, IHttpContextAccessor accessor, SessionManager sessionManager)
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

            //get pages section layout order
            ViewBag.PageSectionsData = _context.PageSections.Where(s => s.SectionName != "HomeSliders").OrderBy(s => s.SectionOrder);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        //Set Session IP Address
        public ActionResult SetSessionIP(string key)
        {
            try
            {
                //log visit
                _sessionManager.SessionIP = key;
                return Json("success");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Set Session Log Error: " + ex.ToString());

                _sessionManager.SessionIP = functions.FormatVisitorIP(_accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());

                return Json("");
            }
        }


        //Set Language Session
        public ActionResult SetLanguageSession([FromQuery(Name = "l")] string language)
        {
            try
            {
                if (!string.IsNullOrEmpty(language))
                {
                    _sessionManager.SessionLanguage = language;
                }
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Set Language Session Error: " + ex.ToString());
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendContact()
        {
            try
            {
                string Subject = HttpContext.Request.Form["Subject"];
                string Message = HttpContext.Request.Form["Message"];
                string Name = HttpContext.Request.Form["Name"];
                string Email = HttpContext.Request.Form["Email"];

                string[] ValidationInputs = { Message, Name, Email };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";

                    return RedirectToAction("Index", "Contact");
                }

                //send email 
                string ToName = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
                string[] MessageParagraphs = { "Contact message from " + Name + "<br/>", Message };
                string PreHeader = "New contact message";
                bool Button = false;
                int ButtonPosition = 0;
                string ButtonLink = null;
                string ButtonLinkText = null;
                string Closure = functions.GetConfigurationsData("EmailClosureText", null);
                string Company = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
                string UnsubscribeLink = null;
                string MessageBody = EmailFormating.FormatEmail(MessageParagraphs, PreHeader, Button, ButtonPosition, ButtonLink, ButtonLinkText, Closure, Company, UnsubscribeLink);

                string FromEmail = _systemConfiguration.smtpEmail;
                string ToEmail = functions.GetCmsData("Email", _systemConfiguration.organizationName);
                string DisplayName = functions.GetConfigurationsData("EmailDisplayName", Subject);
                if (string.IsNullOrEmpty(Subject))
                {
                    Subject = "New Contact Message";
                }

                //TODO SEND EMAIL
                //EmailService.SendEmail(FromEmail, ToEmail, Subject, MessageBody, DisplayName);

                TempData["SuccessMessage"] = $"Thank you for getting in touch! We appreciate you contacting {functions.GetCmsData("Email", _systemConfiguration.organizationName)}. <br/> One of our colleagues will get back in touch with you soon! <br/><br/> Have a great day!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Send Contact Message Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSubscriber()
        {
            string SubscriberEmail = HttpContext.Request.Form["Email"];
            if (!string.IsNullOrEmpty(SubscriberEmail))
            {
                try
                {
                    if (_context.Subscribers.Any(s => s.Email == SubscriberEmail))
                    {
                        TempData["SuccessMessage"] = $@"Thank you for confirming your subscription.";
                        return RedirectToAction("Index", "Home");
                    }

                    //add subscriber
                    functions.AddSubscriber(SubscriberEmail);

                    //TODO Send Email

                    TempData["SuccessMessage"] = @"Thank You For Subscribing!";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Subscription Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                }

            }
            return RedirectToAction("Index", "Home");
        }


        //generate dynamic sitemap
        [Route("/sitemap.xml")]
        [Route("/sitemap_index.xml")]
        public void SitemapXml()
        {

            var syncIOFeature = HttpContext.Features.Get<IHttpBodyControlFeature>();
            if (syncIOFeature != null)
            {
                syncIOFeature.AllowSynchronousIO = true;
            }

            string host = Request.Scheme + "://" + Request.Host;

            Response.ContentType = "application/xml";

            using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
            {
                xml.WriteStartDocument();
                xml.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                //write constant sitemaps
                xml.WriteStartElement("url");
                xml.WriteElementString("loc", host);
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+01:00");
                xml.WriteElementString("priority", "1.00");
                xml.WriteEndElement();

                xml.WriteStartElement("url");
                xml.WriteElementString("loc", host + "/Contact");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+01:00");
                xml.WriteElementString("priority", "0.80");
                xml.WriteEndElement();

                //Get recent 50 posts
                var Posts = _context.Posts.Where(s => s.Status == 1).OrderByDescending(s => s.CreatedAt).Take(50).ToList();
                foreach (var item in Posts)
                {
                    xml.WriteStartElement("url");
                    xml.WriteElementString("loc", host + "/Posts/" + item.Slug);
                    xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+01:00");
                    xml.WriteElementString("priority", "0.90");
                    xml.WriteEndElement();
                }

                xml.WriteStartElement("url");
                xml.WriteElementString("loc", host + "/Sitemap");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+01:00");
                xml.WriteElementString("priority", "0.60");
                xml.WriteEndElement();

                xml.WriteStartElement("url");
                xml.WriteElementString("loc", host + "/SignIn");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+01:00");
                xml.WriteElementString("priority", "0.60");
                xml.WriteEndElement();

                xml.WriteStartElement("url");
                xml.WriteElementString("loc", host + "/SignUp");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+01:00");
                xml.WriteElementString("priority", "0.60");
                xml.WriteEndElement();

                //Get all navigation
                var Navigations = _context.Navigations.Where(s => s.Status == 1).OrderBy(s => s.NavigationName).ToList();
                foreach (var item in Navigations)
                {
                    xml.WriteStartElement("url");
                    xml.WriteElementString("loc", host + "/" + item.NavigationLink);
                    xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+01:00");
                    xml.WriteElementString("priority", "0.80");
                    xml.WriteEndElement();
                }

                //Get all categories
                var Categories = _context.Categories.Where(s=> s.Status == 1).OrderBy(s => s.CategoryName).ToList();
                foreach (var item in Categories)
                {
                    xml.WriteStartElement("url");
                    xml.WriteElementString("loc", host + "/Category/" + item.CategoryName.Replace(" ", string.Empty));
                    xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+01:00");
                    xml.WriteElementString("priority", "0.80");
                    xml.WriteEndElement();
                }

                //Get all team members
                var TeamMembers = _context.Teams.Where(s => s.Status == 1).OrderBy(s => s.FirstName).ToList();
                foreach (var item in TeamMembers)
                {
                    xml.WriteStartElement("url");
                    xml.WriteElementString("loc", host + "/#team");
                    xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+01:00");
                    xml.WriteElementString("priority", "0.60");
                    xml.WriteEndElement();
                }

                //Get popular all tags
                string PopularTags = functions.RemoveDuplicateCSV(functions.GetAllPopularTags());
                if (!string.IsNullOrEmpty(PopularTags))
                {
                    string[] PopularTagsList = PopularTags.Split(",");

                    // Loop over categories list.
                    foreach (string tag in PopularTagsList)
                    {
                        xml.WriteStartElement("url");
                        xml.WriteElementString("loc", host + "/Tags/" + tag.Replace(" ", string.Empty));
                        xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+01:00");
                        xml.WriteElementString("priority", "0.80");
                        xml.WriteEndElement();
                    }
                }
            }
        }



        //generate robots.txt
        [Route("/robots.txt")]
        public ContentResult RobotsTxt()
        {
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *")
                .AppendLine("")
                .Append("sitemap: ")
                .Append(this.Request.Scheme)
                .Append("://")
                .Append(this.Request.Host)
                .AppendLine("/sitemap.xml")
                .Append("sitemap: ")
                .Append(this.Request.Scheme)
                .Append("://")
                .Append(this.Request.Host)
                .AppendLine("/sitemap_index.xml");

            return this.Content(sb.ToString(), "text/plain", Encoding.UTF8);
        }


        //ovveride NotFound() to E404 error page
        public new IActionResult NotFound()
        {
            return RedirectToAction("E404", "Error");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
