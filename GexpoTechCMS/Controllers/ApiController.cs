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

namespace NgoExpoApp.Controllers
{
    public class ApiController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<ApiController> _logger;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;
        private readonly SessionManager _sessionManager;

        public ApiController(DBConnection context, ILogger<ApiController> logger, IOptions<SystemConfiguration> systemConfiguration, IDetectionService detectionService, IHttpContextAccessor accessor, SessionManager sessionManager)
        {
            _context = context;
            _logger = logger;
            _systemConfiguration = systemConfiguration.Value;
            _detectionService = detectionService;
            _accessor = accessor;
            _sessionManager = sessionManager;
        }

        /// <summary>
        /// Validates if record exists in database on input keyup
        /// </summary>
        /// <param name="key">
        /// key format, e.g. username@gmail.com[#]Accounts|Email
        /// </param>
        /// <returns>
        /// empty string or success#ModelName
        /// </returns>
        public IActionResult VerifyUniqueData(string key)
        {
            if (!string.IsNullOrEmpty(key) && key.Contains("[#]"))
            {
                string input = key.Split("[#]")[0];
                string model_data = key.Split("[#]")[1];

                //check if model data is formatted properly
                if (model_data.Contains("|"))
                {
                    string model_name = model_data.Split("|")[0];
                    string model_key = model_data.Split("|")[1];

                    string record_data = functions.GetTableData(model_name, model_key, input, model_key); //get data if any

                    //check in db and return "exists" if record exists
                    if (!string.IsNullOrEmpty(record_data))
                    {
                        string return_data = "success#" + model_key;
                        return Json(return_data);
                    }
                }
            }
            return Json("");
        }


        /// <summary>
        /// Gets search data from seach input
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetSearchResults()
        {
            //get input values
            string SearchValue = HttpContext.Request.Form["searchValue"];
            string result_data = "";

            //if input not empty and greater than 1 character
            if (!string.IsNullOrEmpty(SearchValue) && SearchValue.Length > 1)
            {
                result_data = functions.GetPostSearchResults(SearchValue); //get post results
                result_data += functions.GetPageSearchResults(SearchValue); //get page results

                result_data = (!string.IsNullOrEmpty(result_data)) ? result_data : "<li class='list-group-item'>No recent results found. Press 'Enter' for detailed search.</li>";


                return Json(result_data);
            }
            return Json("");
        } 


    }
}
