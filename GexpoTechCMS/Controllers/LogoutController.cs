using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NgoExpoApp.Models;

namespace NgoExpoApp.Controllers
{
    public class LogoutController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly SessionManager _sessionManager;
        private readonly SystemConfiguration _systemConfiguration;

        public LogoutController(SessionManager sessionManager, DBConnection context, IOptions<SystemConfiguration> systemConfiguration)
        {
            _sessionManager = sessionManager;
            _context = context;
            _systemConfiguration = systemConfiguration.Value;
        }

        public IActionResult Index()
        {
            var LoginAccountId = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            //remove session key
            functions.UpdateTableData("Accounts", "AccountID", LoginAccountId, "SessionKey", null);


            //remove sessions
            _sessionManager.ClearSessions();

            //remove cookie
            RemoveCookie(_systemConfiguration.authCookieKey);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>  
        /// Delete the cokie key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }

    }
}