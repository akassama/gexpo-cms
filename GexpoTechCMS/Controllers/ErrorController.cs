using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NgoExpoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoExpoApp.Controllers
{
    public class ErrorController : Controller
    {
        AppFunctions functions = new AppFunctions();
        private readonly SystemConfiguration _systemConfiguration;
        public ErrorController(IOptions<SystemConfiguration> systemConfiguration) {
            _systemConfiguration = systemConfiguration.Value;
        }

        public IActionResult E404()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["Description"] = functions.GetCmsData("SEODescription", _systemConfiguration.seoDescription);
            ViewData["Author"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.seoAuthor);
            ViewData["Keywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.seoKeywords);

            return View();
        }


        public IActionResult E400()
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
