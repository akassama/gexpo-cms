using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using NgoExpoApp.Models;
using GexpoTechCMS.Models.AppModels;
using Wangkanai.Detection.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using NgoExpoApp.Models.Emailer;
using DNTCaptcha.Core;

namespace NgoExpoApp.Controllers
{
    public class SignUpController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<SignUpController> _logger;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly AppMessages _appMessages;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;
        private readonly SessionManager _sessionManager;

        public SignUpController(DBConnection context, ILogger<SignUpController> logger, IOptions<SystemConfiguration> systemConfiguration, IOptions<AppMessages> appMessages, IDetectionService detectionService, IHttpContextAccessor accessor, SessionManager sessionManager)
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
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["Description"] = functions.GetCmsData("SEODescription", _systemConfiguration.seoDescription);
            ViewData["Author"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.seoAuthor);
            ViewData["Keywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.seoKeywords);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(
        ErrorMessage = "Please Enter Valid Captcha",
        CaptchaGeneratorLanguage = Language.English,
        CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public async Task<IActionResult> Index(
        [Bind("FirstName,LastName,Email,Password")] AccountModel accountsModel)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            ViewData["Description"] = functions.GetCmsData("SEODescription", _systemConfiguration.seoDescription);
            ViewData["Author"] = functions.GetCmsData("SEOAuthor", _systemConfiguration.seoAuthor);
            ViewData["Keywords"] = functions.GetCmsData("SEOKeywords", _systemConfiguration.seoKeywords);

            try
            {
                if (ModelState.IsValid)
                {
                    //verify password match
                    string ConfirmPassword = Request.Form["RepeatPassword"];
                    if (!functions.PasswordsMatch(accountsModel.Password, ConfirmPassword))
                    {
                        TempData["ErrorMessage"] = "Passwords do not match";
                        return View(accountsModel);
                    }

                    //verify email does not exist
                    if (_context.Accounts.Any(s => s.Email == accountsModel.Email))
                    {
                        TempData["ErrorMessage"] = "Email already exists, please choose a different email";
                        return View(accountsModel);
                    }

                    //set registration default values
                    accountsModel.AccountID = functions.GetGuid();
                    accountsModel.DirectoryName = functions.GenerateDirectoryName(accountsModel.Email);
                    accountsModel.Active = false;
                    accountsModel.Oauth = false;
                    accountsModel.EmailVerification = false;
                    accountsModel.EmailNotification = false;
                    accountsModel.UpdatedAt = DateTime.Now;
                    accountsModel.CreatedAt = DateTime.Now;

                    //hashing password with BCrypt
                    accountsModel.Password = BCrypt.Net.BCrypt.HashPassword(accountsModel.Password);


                    _context.Add(accountsModel);
                    await _context.SaveChangesAsync();

                    /* Send user email */
                    //set email data
                    string ToName = functions.GetAccountData(accountsModel.AccountID, "FullName");
                    string[] MessageParagraphs = { "Hello " + ToName + ", ", "Thank you for registering to " + functions.GetConfigurationsData("OrganizationName", _systemConfiguration.organizationName) + ". <br/>", functions.GetAppMessage("SignUpMessage", _appMessages.signUpMessage) };
                    string PreHeader = "New account registration notification.";
                    bool Button = false;
                    int ButtonPosition = 2;
                    string ButtonLink = null;
                    string ButtonLinkText = null;
                    string Closure = functions.GetConfigurationsData("EmailClosureText", null);
                    string Company = functions.GetConfigurationsData("OrganizationName", _systemConfiguration.organizationName);
                    string UnsubscribeLink = null;
                    string MessageBody = EmailFormating.FormatEmail(MessageParagraphs, PreHeader, Button, ButtonPosition, ButtonLink, ButtonLinkText, Closure, Company, UnsubscribeLink);

                    string FromEmail = _systemConfiguration.smtpEmail;
                    string ToEmail = accountsModel.Email;
                    string Subject = "Account Registration Email";
                    string DisplayName = functions.GetConfigurationsData("EmailDisplayName", Subject);

                    //TODO SEND EMAIL
                    //EmailService.SendEmail(FromEmail, ToEmail, Subject, MessageBody, DisplayName);

                    //log activity
                    string LogAction = $@"User '<a href='/Admin/UserDetails/{accountsModel.AccountID}'>{ToName}</a>' registered.";
                    functions.LogActivity(accountsModel.AccountID, "NewRegistration", LogAction);

                    TempData["SuccessMessage"] = functions.GetAppMessage("SignUpMessage", _appMessages.signUpMessage);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Sign Up Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
            }

            return View(accountsModel);
        }


        //ovveride NotFound() to E404 error page
        public new IActionResult NotFound()
        {
            return RedirectToAction("E404", "Error");
        }
    }
}
