using DNTCaptcha.Core;
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
    public class SignInController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<SignInController> _logger;
        private readonly SessionManager _sessionManager;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly AppMessages _appMessages;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;

        public SignInController(DBConnection context, ILogger<SignInController> logger, SessionManager sessionManager, IOptions<SystemConfiguration> systemConfiguration, IOptions<AppMessages> appMessages, IDetectionService detectionService, IHttpContextAccessor accessor)
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
            //if already logged in, redirect home
            if (functions.IsLoggedIn(_sessionManager.SessionKey))
            {
                return RedirectToAction("Index", "Admin");
            }

            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["Description"] = functions.GetCmsData("SEODescription", _systemConfiguration.seoDescription);
            ViewData["Author"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.seoAuthor);
            ViewData["Keywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.seoKeywords);

            return View();
        }

        // POST: SignUp/Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(
        ErrorMessage = "Please Enter Valid Captcha",
        CaptchaGeneratorLanguage = Language.English,
        CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public IActionResult Index(LoginViewModel loginModel)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["Description"] = functions.GetCmsData("SEODescription", _systemConfiguration.seoDescription);
            ViewData["Author"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.seoAuthor);
            ViewData["Keywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.seoKeywords);

            if (ModelState.IsValid)
            {
                try
                {
                    // check password
                    var query = _context.Accounts.Where(s => s.Email == loginModel.Email);
                    string hashedPassword = (query.Any()) ? query.FirstOrDefault().Password : "";
                    if (!string.IsNullOrEmpty(hashedPassword))
                    {
                        //if login is valid
                        if (BCrypt.Net.BCrypt.Verify(loginModel.Password, hashedPassword))
                        {
                            //If account not activated
                            if (!query.FirstOrDefault().Active)
                            {
                                TempData["ErrorMessage"] = "Account not activated.";
                                return View(loginModel);
                            }


                            //set user info
                            var ID = query.FirstOrDefault().ID;
                            var LoginAccountId = query.FirstOrDefault().AccountID;
                            var LoginUsername = query.FirstOrDefault().Email.Split('@')[0];
                            var LoginEmail = query.FirstOrDefault().Email;
                            var LoginFirstName = (query.FirstOrDefault().FirstName != null) ? query.FirstOrDefault().FirstName : "";
                            var LoginLastName = (query.FirstOrDefault().LastName != null) ? query.FirstOrDefault().LastName : "";
                            var LoginDirectoryName = query.FirstOrDefault().DirectoryName;
                            var LoginSessionKey = functions.GetGuid()+"-"+functions.GetGuid();

                            //Set session key
                            _sessionManager.SessionKey = LoginSessionKey;

                            //add session key
                            functions.UpdateTableData("Accounts", "AccountID", LoginAccountId, "SessionKey", LoginSessionKey);

                            //if cookie exists, remove it
                            if (Request.Cookies["Key"] != null)
                            {
                                //remove cookie
                                RemoveCookie(_systemConfiguration.authCookieKey);
                            }

                            //set the key value in cookie  
                            SetCookie(_systemConfiguration.authCookieKey, LoginSessionKey, 60);

                            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";


                            //log activity
                            string LogAction = $@"User '<a href='/Admin/UserDetails/{LoginAccountId}'>{LoginFirstName} {LoginLastName}</a>' logged in.";
                            functions.LogActivity(LoginAccountId, "UserLogin", LogAction);

                            //goto
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Login failed. You have entered an invalid email or password";
                            return View(loginModel);
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Login failed. You have entered an invalid email or password";
                        return View(loginModel);
                    }
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Login Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(loginModel);
                }
            }
            TempData["ErrorMessage"] = "Login failed";
            return View(loginModel);
        }


        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }


        /// <summary>  
        /// Delete the cokie key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }


        //ovveride NotFound() to E404 error page
        public new IActionResult NotFound()
        {
            return RedirectToAction("E404", "Error");
        }
    }
}
