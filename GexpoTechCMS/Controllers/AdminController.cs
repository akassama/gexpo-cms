using GexpoTechCMS.Models.AppModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NgoExpoApp.Models;
using NgoExpoApp.Models.Session;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace NgoExpoApp.Controllers
{
    //Authorize only logged in users
    [TypeFilter(typeof(SessionAuthorize))]
    public class AdminController : Controller
    {
        AppFunctions functions = new AppFunctions();

        private readonly DBConnection _context;
        private readonly ILogger<AdminController> _logger;
        private readonly SystemConfiguration _systemConfiguration;
        private readonly AppMessages _appMessages;
        private readonly IDetectionService _detectionService;
        private static IHttpContextAccessor _accessor;
        private readonly SessionManager _sessionManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminController(DBConnection context, ILogger<AdminController> logger, IOptions<SystemConfiguration> systemConfiguration, IOptions<AppMessages> appMessages, IDetectionService detectionService, IHttpContextAccessor accessor, SessionManager sessionManager, IWebHostEnvironment hostingEnvironment)
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


        /// <summary>
        /// Dashboard view
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //Remove old nLog files if checked
            functions.RemoveNlogFiles(_systemConfiguration.nLogDelete, _systemConfiguration.nLogDeletePeriod);
            

            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);


            ViewBag.TotalPosts = _context.Posts.Count();
            ViewBag.RecentPostsData = _context.Posts.OrderByDescending(s => s.ID).Take(25);

            return View();
        }



        //      ██╗ ██╗ ██╗     ██████╗ ██████╗  ██████╗ ███████╗██╗██╗     ███████╗       ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔══██╗██╔══██╗██╔═══██╗██╔════╝██║██║     ██╔════╝      ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗██████╔╝██████╔╝██║   ██║█████╗  ██║██║     █████╗█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██╔═══╝ ██╔══██╗██║   ██║██╔══╝  ██║██║     ██╔══╝╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ██║     ██║  ██║╚██████╔╝██║     ██║███████╗███████╗   ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚═╝     ╚═╝╚══════╝╚══════╝   ╚═╝ ╚═╝ ╚═╝    
        //                                                                                            
        /// <summary>
        /// Profile view
        /// </summary>
        /// <returns></returns>

        /// User access control
        [AccessControlFilter(PermissionName = "Author Permissions")]
        public async Task<IActionResult> Profile()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            var accountModel = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountID == AccountID);
            if (accountModel == null)
            {
                return NotFound();
            }

            //Activity data for user
            var RecentActivityData = _context.ActivityLogs.Where(s => s.ActivityBy == AccountID).OrderByDescending(x => x.CreatedAt);
            ViewBag.RecentActivityData = RecentActivityData.Take(6);
            ViewBag.RecentActivityCount = RecentActivityData.Count();
            ViewBag.ShowLoadButton = (ViewBag.RecentActivityCount > 6) ? "" : "d-none"; //hide load more button if total 8 or less

            return View(accountModel);
        }

        /// <summary>
        /// Gets recent user activity through ajax call
        /// </summary>
        /// <param name="newActivityCount"></param>
        /// <returns></returns>
        public ActionResult GetRecentActivitiesCall(string newActivityCount)
        {
            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            //check if empty
            if (!string.IsNullOrEmpty(newActivityCount))
            {
                return Json(functions.GetRecentActivities(AccountID, functions.Int32Parse(newActivityCount, 0), 8).ToString());
            }
            return Json("");
        }


        /// <summary>
        /// My activities view
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        /// 
        /// User access control
        [AccessControlFilter(PermissionName = "Author Permissions")]
        public async Task<IActionResult> MyActivities([FromQuery(Name = "p")] string p = "1", [FromQuery(Name = "s")] string s = "500")
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            ViewBag.PageNo = 1;
            ViewBag.PageSize = 500;
            if (!string.IsNullOrEmpty(p) && !string.IsNullOrEmpty(s))
            {
                ViewBag.PageNo = Int32.Parse(p);
                ViewBag.PageSize = Int32.Parse(s);
            }
            ViewBag.PageSkip = (ViewBag.PageNo - 1) * ViewBag.PageSize;
            int PageSkip = ViewBag.PageSkip;
            int PageSize = ViewBag.PageSize;
            ViewBag.TotalRecords = _context.ActivityLogs.Count(s => s.ActivityBy == AccountID);

            //TODO sort order by
            var activitiesModel = _context.ActivityLogs.Where(s => s.ActivityBy == AccountID).OrderByDescending(s => s.ID).Skip(PageSkip).Take(PageSize);

            return View(await activitiesModel.ToListAsync());
        }



        //      ██╗ ██╗ ██╗     ███████╗███████╗████████╗████████╗██╗███╗   ██╗ ██████╗ ███████╗         ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔════╝██╔════╝╚══██╔══╝╚══██╔══╝██║████╗  ██║██╔════╝ ██╔════╝        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗███████╗█████╗     ██║      ██║   ██║██╔██╗ ██║██║  ███╗███████╗█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝╚════██║██╔══╝     ██║      ██║   ██║██║╚██╗██║██║   ██║╚════██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ███████║███████╗   ██║      ██║   ██║██║ ╚████║╚██████╔╝███████║     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚══════╝╚══════╝   ╚═╝      ╚═╝   ╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚══════╝     ╚═╝ ╚═╝ ╚═╝    
        //                                                                                                          
        /// <summary>
        /// settings view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Author Permissions")]
        public IActionResult Settings()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            //Get countries list
            ViewBag.CountriesList = functions.GetCountryList();

            return View();
        }

        /// <summary>
        /// updates account information
        /// </summary>
        /// <param name="accountModel"></param>
        /// <param name="ProfileImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccountInfo(AccountModel accountModel, List<IFormFile> ProfileImage)
        {
            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            try
            {
                string UploadPicture = null;
                //check if image is posted
                if (Request.Form.Files.Count > 0)
                {
                    var DirectoryName = functions.GetSessionAccountData(_sessionManager.SessionKey, "DirectoryName");
                    var SavePath = @"wwwroot\\admin\\images\\accounts\\" + DirectoryName + "\\";
                    UploadPicture = functions.UploadImage(ProfileImage, 1, 150, 150, null, SavePath);
                    accountModel.ProfilePicture = UploadPicture;
                    //TODO reset profile image session link
                    //_sessionManager.LoginProfilePicture = AppSettings.BaseUrl() + "/admin/images/accounts/" + functions.GetSessionAccountData(_sessionManager.SessionKey, "DirectoryName") + "/" + UploadPicture;
                }
                else
                {
                    accountModel.ProfilePicture = functions.GetAccountData(AccountID, "ProfilePicture");
                }

                if (Request.Form.Any())
                {
                    //get account primary key
                    accountModel.ID = functions.Int32Parse(functions.GetAccountData(AccountID, "ID"), 0);

                    //set defaults
                    accountModel.AccountID = AccountID;
                    accountModel.Password = functions.GetAccountData(AccountID, "Password");
                    accountModel.DirectoryName = functions.GetAccountData(AccountID, "DirectoryName");
                    accountModel.Active = true;

                    //set account data
                    accountModel.FirstName = HttpContext.Request.Form["FirstName"];
                    accountModel.LastName = HttpContext.Request.Form["LastName"];
                    accountModel.Description = HttpContext.Request.Form["Description"];
                    accountModel.Gender = HttpContext.Request.Form["Gender"];
                    accountModel.PhoneNumber = HttpContext.Request.Form["PhoneNumber"];
                    accountModel.DateOfBirth = HttpContext.Request.Form["DateOfBirth"];
                    accountModel.Address = HttpContext.Request.Form["Address"];
                    accountModel.Country = HttpContext.Request.Form["Country"];
                    accountModel.UpdatedAt = DateTime.Now;
                    accountModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("Accounts", "AccountID", accountModel.AccountID, "CreatedAt"));
                    _context.Update(accountModel);
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Settings", "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Account Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Settings", "Admin");
            }
        }


        /// <summary>
        /// updates account password
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAccountPassword()
        {
            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            try
            {
                //get input values
                string CurrentPassword = HttpContext.Request.Form["CurrentPassword"];
                string NewPassword = HttpContext.Request.Form["NewPassword"];
                string ConfirmPassword = HttpContext.Request.Form["RepeatPassword"];

                string[] ValidationInputs = { CurrentPassword, NewPassword, ConfirmPassword };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                    return RedirectToAction("Settings", new { tab = "password" });
                }

                //verify password match
                if (!functions.PasswordsMatch(NewPassword, ConfirmPassword))
                {
                    TempData["ErrorMessage"] = "Passwords do not match";
                    return RedirectToAction("Settings", new { tab = "password" });
                }

                // get password
                var query = _context.Accounts.Where(s => s.AccountID == AccountID);
                string hashedPassword = (query.Any()) ? query.FirstOrDefault().Password : "";

                //check in db and return "exists" if record exists
                if (BCrypt.Net.BCrypt.Verify(NewPassword, hashedPassword))
                {
                    TempData["ErrorMessage"] = "Please choose a different password from the current one";
                    return RedirectToAction("Settings", new { tab = "password" });
                }

                //Update values
                NewPassword = BCrypt.Net.BCrypt.HashPassword(NewPassword);
                functions.UpdateTableData("Accounts", "AccountID", AccountID, "Password", NewPassword);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);

                //log activity
                string LogAction = $@"User '<a href='/Admin/UserDetails/{AccountID}'>{functions.GetAccountData(AccountID, "FullName")}</a>' updated account password.";
                functions.LogActivity(AccountID, "PasswordUpdate", LogAction);

                return RedirectToAction("Settings", new { tab = "password" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Account Password Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Settings", new { tab = "password" });
            }
        }


        /// <summary>
        /// Updates email notification settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateNotificationSetting()
        {
            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            //get input values
            string EmailNotification = HttpContext.Request.Form["EmailNotification"];
            try
            {
                functions.UpdateTableData("Accounts", "AccountID", AccountID, "EmailNotification", EmailNotification);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Settings", new { tab = "notifications" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Account Notification Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Settings", new { tab = "notifications" });
            }
        }



        /// <summary>
        /// Deletes user account
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccount()
        {
            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            //get input values
            string AccountEmail = HttpContext.Request.Form["AccountEmail"];
            string ReturnView = HttpContext.Request.Form["ReturnView"];
            ReturnView = (!string.IsNullOrEmpty(ReturnView)) ? ReturnView : "Settings";
            var SessionEmail = functions.GetSessionAccountData(_sessionManager.SessionKey, "Email");


            if (AccountEmail != SessionEmail)
            {
                TempData["ErrorMessage"] = "Email provided does not match your email.";
                if (!string.IsNullOrEmpty(ReturnView))
                {
                    return RedirectToAction(ReturnView, "Admin");
                }
                return RedirectToAction(ReturnView, "Admin", new { tab = "delete-account" });
            }

            try
            {
                //Delete account and other account details
                functions.DeleteTableData("Accounts", "AccountID", AccountID);
                functions.DeleteTableData("AccountToPermission", "AccountID", AccountID);

                //log activity
                var SessionFirstName = functions.GetSessionAccountData(_sessionManager.SessionKey, "FirstName");
                var SessionLastName = functions.GetSessionAccountData(_sessionManager.SessionKey, "LastName");
                string LogAction = $@"User '{SessionFirstName} {SessionLastName}' deleted account.";
                functions.LogActivity(functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID"), "UserLogin", LogAction);

                //view for admin removing account
                if (!string.IsNullOrEmpty(ReturnView))
                {
                    TempData["SuccessMessage"] = functions.GetAppMessage("DeleteSuccessMessage", _appMessages.deleteSuccessMessage);
                    return RedirectToAction(ReturnView, "Admin");
                }
                return RedirectToAction("Index", "Logout");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Delete Account Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Settings", new { tab = "delete-account" });
            }
        }

        //      ██╗ ██╗ ██╗     ██████╗  ██████╗ ███████╗████████╗███████╗         ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔══██╗██╔═══██╗██╔════╝╚══██╔══╝██╔════╝        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗██████╔╝██║   ██║███████╗   ██║   ███████╗█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██╔═══╝ ██║   ██║╚════██║   ██║   ╚════██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ██║     ╚██████╔╝███████║   ██║   ███████║     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚═╝      ╚═════╝ ╚══════╝   ╚═╝   ╚══════╝     ╚═╝ ╚═╝ ╚═╝    
        //                                                                                    

        /// <summary>
        /// Posts view
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        /// 
        /// User access control
        [AccessControlFilter(PermissionName = "Author Permissions")]
        public async Task<IActionResult> Posts([FromQuery(Name = "p")] string p = "1", [FromQuery(Name = "s")] string s = "100")
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            ViewBag.PageNo = 1;
            ViewBag.PageSize = 100;
            if (!string.IsNullOrEmpty(p) && !string.IsNullOrEmpty(s))
            {
                ViewBag.PageNo = Int32.Parse(p);
                ViewBag.PageSize = Int32.Parse(s);
            }
            ViewBag.PageSkip = (ViewBag.PageNo - 1) * ViewBag.PageSize;
            int PageSkip = ViewBag.PageSkip;
            int PageSize = ViewBag.PageSize;
            ViewBag.TotalRecords = _context.Posts.Count(s => s.Author == AccountID);

            var postsModel = _context.Posts.Where(s => s.Author == AccountID).OrderByDescending(s => s.ID).Skip(PageSkip).Take(PageSize);

            return View(await postsModel.ToListAsync());
        }



        /// <summary>
        /// New Post view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Author Permissions")]
        public IActionResult NewPost()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            //get categories
            ViewBag.CategoriesData = _context.Categories.Where(x => x.Status == 1).OrderBy(s => s.Order);

            return View();
        }


        /// <summary>
        /// Adds new post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPost(PostModel postsModel, List<IFormFile> PostImage, List<IFormFile> ImageGallery)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            //get categories
            ViewBag.CategoriesData = _context.Categories.Where(x => x.Status == 1).OrderBy(s => s.Order);

            //validate required content
            string[] ValidationInputs = { postsModel.Content };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required content.";
                return View(postsModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Set default post values
                    postsModel.PostID = functions.GetGuid();

                    //if post image posted, upload image and set value
                    if (PostImage.Count > 0)
                    {
                        var DirectoryName = DateTime.Now.ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\posts\\images\\" + DirectoryName + "\\";
                        postsModel.PostImage = functions.UploadImage(PostImage, 1, 1420, 820, null, SavePath);
                    }

                    //if gallery images posted, upload gallery
                    if (ImageGallery.Count > 0)
                    {
                        var DirectoryName = DateTime.Now.ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\posts\\images\\" + DirectoryName + "\\";
                        functions.UploadImages(ImageGallery, 25, 1420, 820, null, SavePath, postsModel.PostID); 
                    }

                    postsModel.Author = AccountID;
                    postsModel.Slug = functions.GenerateSlug(postsModel.Title, "Post");
                    postsModel.PostType = "Default";
                    postsModel.FeaturedPost = (string.IsNullOrEmpty(HttpContext.Request.Form["FeaturedPost"])) ? false : functions.BooleanParse(HttpContext.Request.Form["FeaturedPost"]);

                    postsModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    postsModel.Categories = HttpContext.Request.Form["PostCategory"];
                    postsModel.PostTags = HttpContext.Request.Form["PostTags"];
                    postsModel.SEOTitle = (string.IsNullOrEmpty(HttpContext.Request.Form["SEOTitle"])) ? postsModel.Title : HttpContext.Request.Form["SEOTitle"].ToString();
                    postsModel.SEODescription = (string.IsNullOrEmpty(HttpContext.Request.Form["SEODescription"])) ? postsModel.Title : HttpContext.Request.Form["SEODescription"].ToString();
                    postsModel.SEOKeywords = (string.IsNullOrEmpty(HttpContext.Request.Form["SEOKeywords"])) ? postsModel.PostTags : HttpContext.Request.Form["SEOKeywords"].ToString();
                    postsModel.UpdatedAt = DateTime.Now;
                    postsModel.CreatedAt = DateTime.Now;

                    _context.Add(postsModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                    return RedirectToAction("Posts", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("New Post Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(postsModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(postsModel);
            }
        }



        /// <summary>
        /// Edit post view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Author Permissions")]
        public async Task<IActionResult> EditPost(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            //get categories
            ViewBag.CategoriesData = _context.Categories.Where(x => x.Status == 1).OrderBy(s => s.Order);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var postsModel = await _context.Posts
               .FirstOrDefaultAsync(m => m.PostID == id);
            if (postsModel == null)
            {
                return NotFound();
            }
            return View(postsModel);
        }



        /// <summary>
        /// Edits post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(PostModel postsModel, List<IFormFile> PostImage, List<IFormFile> ImageGallery)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            //get categories
            ViewBag.CategoriesData = _context.Categories.Where(x => x.Status == 1).OrderBy(s => s.Order);

            //validate required content
            string[] ValidationInputs = { postsModel.Content };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required content.";
                return View(postsModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //if post image posted, upload image and set value
                    if (PostImage.Count > 0)
                    {
                        var DirectoryName = Convert.ToDateTime(functions.GetPostData(postsModel.PostID, "CreatedAt")).ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\posts\\images\\" + DirectoryName + "\\";
                        postsModel.PostImage = functions.UploadImage(PostImage, 1, 1420, 820, null, SavePath);
                    }
                    else
                    {
                        //set to current post image
                        postsModel.PostImage = functions.GetPostData(postsModel.PostID, "PostImage");
                    }

                    //if gallery images posted, upload gallery
                    if (ImageGallery.Count > 0)
                    {
                        var DirectoryName = Convert.ToDateTime(functions.GetPostData(postsModel.PostID, "CreatedAt")).ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\posts\\images\\" + DirectoryName + "\\";
                        functions.UploadImages(ImageGallery, 25, 1420, 820, null, SavePath, postsModel.PostID); 
                    }

                    postsModel.Author = AccountID;

                    //if title different, re-generate slug
                    string CurrentTitle= functions.GetTableData("Posts", "PostID", postsModel.PostID, "Title");
                    if (CurrentTitle != postsModel.Title)
                    {
                        postsModel.Slug = functions.GenerateSlug(postsModel.Title, "Post");
                    }
                    postsModel.PostType = "Default";
                    postsModel.FeaturedPost = (string.IsNullOrEmpty(HttpContext.Request.Form["FeaturedPost"])) ? false : functions.BooleanParse(HttpContext.Request.Form["FeaturedPost"]);

                    postsModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    postsModel.Categories = HttpContext.Request.Form["PostCategory"];
                    postsModel.PostTags = HttpContext.Request.Form["PostTags"];
                    postsModel.SEOTitle = (string.IsNullOrEmpty(HttpContext.Request.Form["SEOTitle"])) ? postsModel.Title : HttpContext.Request.Form["SEOTitle"].ToString();
                    postsModel.SEODescription = (string.IsNullOrEmpty(HttpContext.Request.Form["SEODescription"])) ? postsModel.Title : HttpContext.Request.Form["SEODescription"].ToString();
                    postsModel.SEOKeywords = (string.IsNullOrEmpty(HttpContext.Request.Form["SEOKeywords"])) ? postsModel.PostTags : HttpContext.Request.Form["SEOKeywords"].ToString();
                    postsModel.UpdatedAt = DateTime.Now;
                    postsModel.CreatedAt = Convert.ToDateTime(functions.GetPostData(postsModel.PostID, "CreatedAt"));

                    _context.Update(postsModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                    return RedirectToAction("Posts", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Edit Post Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(postsModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(postsModel);
            }
        }


        /// <summary>
        /// Categories view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Author Permissions")]
        public async Task<IActionResult> Categories()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            var categoriesData = _context.Categories.OrderBy(s => s.Order);
            return View(await categoriesData.ToListAsync());
        }

        /// <summary>
        /// Adds new category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory()
        {
            //get input values
            string CategoryID = functions.GetGuid();
            string CategoryName = HttpContext.Request.Form["CategoryName"];
            string ShortDescription = HttpContext.Request.Form["ShortDescription"];
            string CategoryIcon = HttpContext.Request.Form["CategoryIcon"];
            string CategoryStatus = HttpContext.Request.Form["CategoryStatus"];
            string CategoryOrder = HttpContext.Request.Form["CategoryOrder"];
            string ShortCategoryName = functions.ConvertCase(HttpContext.Request.Form["CategoryName"], "TitleTrim");
            string ReturnView = HttpContext.Request.Form["ReturnView"];
            string ReturnViewID = HttpContext.Request.Form["ReturnViewID"];
            ReturnView = (!string.IsNullOrEmpty(ReturnView)) ? ReturnView : "NewPost";

            //validate required inputs
            string[] ValidationInputs = { CategoryName, CategoryStatus };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                if (!string.IsNullOrEmpty(ReturnViewID))
                {
                    return RedirectToAction(ReturnView, "Admin", new { id = ReturnViewID });
                }
                return RedirectToAction(ReturnView, "Admin");
            }

            //validate unique category name
            if (_context.Categories.Any(s => s.CategoryName == CategoryName))
            {
                TempData["ErrorMessage"] = "Category with the same name already exist.";

                if (!string.IsNullOrEmpty(ReturnViewID))
                {
                    return RedirectToAction(ReturnView, "Admin", new { id = ReturnViewID });
                }
                return RedirectToAction(ReturnView, "Admin");
            }

            try
            {
                CategoryModel categoriesModel = new CategoryModel
                {
                    CategoryID = CategoryID,
                    CategoryName = CategoryName,
                    ShortCategoryName = ShortCategoryName,
                    ShortDescription = ShortDescription,
                    Icon = CategoryIcon,
                    Status = (CategoryStatus == "1") ? 1 : 0,
                    Order = functions.Int32Parse(CategoryOrder, 10), //set default order as 10
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                _context.Categories.Add(categoriesModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);

                if (!string.IsNullOrEmpty(ReturnViewID))
                {
                    return RedirectToAction(ReturnView, "Admin", new { id = ReturnViewID });
                }
                return RedirectToAction(ReturnView, "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Add Category Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);

                if (!string.IsNullOrEmpty(ReturnViewID))
                {
                    return RedirectToAction(ReturnView, "Admin", new { id = ReturnViewID });
                }
                return RedirectToAction(ReturnView, "Admin");
            }
        }


        /// <summary>
        /// Edits category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory()
        {
            //get input values
            string CategoryID = HttpContext.Request.Form["CategoryID"];
            string CategoryName = HttpContext.Request.Form["CategoryName"];
            string ShortDescription = HttpContext.Request.Form["ShortDescription"];
            string CategoryIcon = HttpContext.Request.Form["CategoryIcon"];
            string CategoryStatus = HttpContext.Request.Form["CategoryStatus"];
            string CategoryOrder = HttpContext.Request.Form["CategoryOrder"];
            string ShortCategoryName = functions.ConvertCase(HttpContext.Request.Form["CategoryName"], "TitleTrim");
            string ReturnView = HttpContext.Request.Form["ReturnView"];
            string ReturnViewID = HttpContext.Request.Form["ReturnViewID"];
            ReturnView = (!string.IsNullOrEmpty(ReturnView)) ? ReturnView : "NewPost";

            //validate required inputs
            string[] ValidationInputs = { CategoryID, CategoryName, CategoryStatus };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return RedirectToAction(ReturnView, "Admin");
            }

            //validate unique category name
            if (_context.Categories.Any(s => s.CategoryName == CategoryName && s.CategoryID != CategoryID))
            {
                TempData["ErrorMessage"] = "Category with the same name already exist.";
                return RedirectToAction(ReturnView, "Admin");
            }

            try
            {

                CategoryModel categoriesModel = _context.Categories.Single(u => u.CategoryID == CategoryID);
                categoriesModel.CategoryID = CategoryID;
                categoriesModel.CategoryName = CategoryName;
                categoriesModel.ShortCategoryName = ShortCategoryName;
                categoriesModel.ShortDescription = ShortDescription;
                categoriesModel.Icon = CategoryIcon;
                categoriesModel.Status = (CategoryStatus == "1") ? 1 : 0;
                categoriesModel.Order = functions.Int32Parse(CategoryOrder, 10); //set default order as 10
                categoriesModel.UpdatedAt = DateTime.Now;
                categoriesModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("Category", "CategoryID", categoriesModel.CategoryID, "CreatedAt"));

                _context.Categories.Update(categoriesModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction(ReturnView, "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Category Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction(ReturnView, "Admin");
            }
        }


        //      ██╗ ██╗ ██╗     ███╗   ██╗ █████╗ ██╗   ██╗██╗ ██████╗  █████╗ ████████╗██╗ ██████╗ ███╗   ██╗         ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ████╗  ██║██╔══██╗██║   ██║██║██╔════╝ ██╔══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗██╔██╗ ██║███████║██║   ██║██║██║  ███╗███████║   ██║   ██║██║   ██║██╔██╗ ██║█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██║╚██╗██║██╔══██║╚██╗ ██╔╝██║██║   ██║██╔══██║   ██║   ██║██║   ██║██║╚██╗██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ██║ ╚████║██║  ██║ ╚████╔╝ ██║╚██████╔╝██║  ██║   ██║   ██║╚██████╔╝██║ ╚████║     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚═╝  ╚═══╝╚═╝  ╚═╝  ╚═══╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝     ╚═╝ ╚═╝ ╚═╝    
        //                                                                                                                        
        //

        /// <summary>
        /// Navigation view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> Navigation()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            var navigationModel = _context.Navigations.OrderBy(s => s.Order);
            return View(await navigationModel.ToListAsync());
        }


        /// <summary>
        /// Adds new navigation
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNavigation()
        {
            //get input values
            string NavigationID = functions.GetGuid();
            string NavigationName = HttpContext.Request.Form["NavigationName"];
            string NavigationLink = HttpContext.Request.Form["NavigationLink"];
            string NavigationParent = HttpContext.Request.Form["NavigationParent"];
            string NavigationStatus = HttpContext.Request.Form["NavigationStatus"];
            string NavigationOrder = HttpContext.Request.Form["NavigationOrder"];
            string ReturnView = HttpContext.Request.Form["ReturnView"];
            string ReturnViewID = HttpContext.Request.Form["ReturnViewID"];
            ReturnView = (!string.IsNullOrEmpty(ReturnView)) ? ReturnView : "Navigation";

            //validate required inputs
            string[] ValidationInputs = { NavigationName, NavigationLink };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                if (!string.IsNullOrEmpty(ReturnViewID))
                {
                    return RedirectToAction(ReturnView, "Admin", new { id = ReturnViewID });
                }
                return RedirectToAction(ReturnView, "Admin");
            }

            //validate unique navigation name
            if (_context.Navigations.Any(s => s.NavigationName == NavigationName))
            {
                TempData["ErrorMessage"] = "Navigation with the same name already exist.";

                if (!string.IsNullOrEmpty(ReturnViewID))
                {
                    return RedirectToAction(ReturnView, "Admin", new { id = ReturnViewID });
                }
                return RedirectToAction(ReturnView, "Admin");
            }

            try
            {
                NavigationModel navigationsModel = new NavigationModel
                {
                    NavigationID = NavigationID,
                    NavigationName = NavigationName,
                    NavigationLink = NavigationLink,
                    Parent = NavigationParent,
                    Status = (NavigationStatus == "1") ? 1 : 0,
                    Order = functions.Int32Parse(NavigationOrder, 10), //set default order as 10
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                _context.Navigations.Add(navigationsModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);

                if (!string.IsNullOrEmpty(ReturnViewID))
                {
                    return RedirectToAction(ReturnView, "Admin", new { id = ReturnViewID });
                }
                return RedirectToAction(ReturnView, "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Add Navigation Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);

                if (!string.IsNullOrEmpty(ReturnViewID))
                {
                    return RedirectToAction(ReturnView, "Admin", new { id = ReturnViewID });
                }
                return RedirectToAction(ReturnView, "Admin");
            }
        }


        /// <summary>
        /// Edits navigation
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNavigation()
        {
            //get input values
            string NavigationID = HttpContext.Request.Form["NavigationID"];
            string NavigationName = HttpContext.Request.Form["NavigationName"];
            string NavigationLink = HttpContext.Request.Form["NavigationLink"];
            string NavigationParent = HttpContext.Request.Form["NavigationParent"];
            string NavigationStatus = HttpContext.Request.Form["NavigationStatus"];
            string NavigationOrder = HttpContext.Request.Form["NavigationOrder"];
            string ReturnView = HttpContext.Request.Form["ReturnView"];
            string ReturnViewID = HttpContext.Request.Form["ReturnViewID"];
            ReturnView = (!string.IsNullOrEmpty(ReturnView)) ? ReturnView : "Navigation";

            //validate required inputs
            string[] ValidationInputs = { NavigationID, NavigationName, NavigationLink };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return RedirectToAction(ReturnView, "Admin");
            }

            //validate unique category name
            if (_context.Navigations.Any(s => s.NavigationName == NavigationName && s.NavigationID != NavigationID))
            {
                TempData["ErrorMessage"] = "Navigation with the same name already exist.";
                return RedirectToAction(ReturnView, "Admin");
            }

            try
            {
                NavigationModel navigationsModel = _context.Navigations.Single(u => u.NavigationID == NavigationID);
                navigationsModel.NavigationID = NavigationID;
                navigationsModel.NavigationLink = NavigationLink;
                navigationsModel.Parent = (NavigationParent.Contains("select")) ? "" : NavigationParent; //sets to empty if select option posted
                navigationsModel.Status = (NavigationStatus == "1") ? 1 : 0;
                navigationsModel.Order = functions.Int32Parse(NavigationOrder, 10); //set default order as 10
                navigationsModel.UpdatedAt = DateTime.Now;
                navigationsModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("Navigation", "NavigationID", navigationsModel.NavigationID, "CreatedAt"));

                _context.Navigations.Update(navigationsModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction(ReturnView, "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Navigation Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction(ReturnView, "Admin");
            }
        }


        /// <summary>
        /// Navigation view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> FooterNavigation()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            var navigationModel = _context.FooterNavigations.OrderBy(s => s.Order);
            return View(await navigationModel.ToListAsync());
        }


        /// <summary>
        /// New footer navigation view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public IActionResult NewFooterNavigation()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            return View();
        }


        /// <summary>
        /// Adds new footer navigation
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewFooterNavigation(FooterNavigationModel footerNavigation)
        {
            //get input values
            string NavigationID = functions.GetGuid();
            string NavigationName = HttpContext.Request.Form["NavigationName"];
            string NavigationLink = HttpContext.Request.Form["NavigationLink"];
            string NavigationStatus = HttpContext.Request.Form["Status"];
            string NavigationOrder = HttpContext.Request.Form["Order"];

            //validate required inputs
            string[] ValidationInputs = { NavigationName, NavigationLink };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return RedirectToAction("NewFooterNavigation", "Admin");
            }

            //validate unique navigation name
            if (_context.FooterNavigations.Any(s => s.NavigationName == NavigationName))
            {
                TempData["ErrorMessage"] = "Footer Navigation with the same name already exist.";

                return RedirectToAction("NewFooterNavigation", "Admin");
            }

            try
            {
                FooterNavigationModel navigationsModel = new FooterNavigationModel
                {
                    NavigationID = NavigationID,
                    NavigationName = NavigationName,
                    NavigationLink = NavigationLink,
                    Status = (NavigationStatus == "1") ? 1 : 0,
                    Order = functions.Int32Parse(NavigationOrder, 10), //set default order as 10
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                _context.FooterNavigations.Add(navigationsModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);

                return RedirectToAction("FooterNavigation", "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Add Footer Navigation Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);

                return RedirectToAction("NewFooterNavigation", "Admin");
            }
        }



        /// <summary>
        /// Edit Footer Navigation view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> EditFooterNavigation(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var navigationModel = await _context.FooterNavigations
               .FirstOrDefaultAsync(m => m.NavigationID == id);
            if (navigationModel == null)
            {
                return NotFound();
            }
            return View(navigationModel);
        }


        /// <summary>
        /// Edits footer navigation
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFooterNavigation()
        {
            //get input values
            string NavigationID = HttpContext.Request.Form["NavigationID"];
            string NavigationName = HttpContext.Request.Form["NavigationName"];
            string NavigationLink = HttpContext.Request.Form["NavigationLink"];
            string NavigationStatus = HttpContext.Request.Form["Status"];
            string NavigationOrder = HttpContext.Request.Form["Order"];

            //validate required inputs
            string[] ValidationInputs = { NavigationID, NavigationName, NavigationLink };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return RedirectToAction("NewFooterNavigation", "Admin");
            }

            //validate unique category name
            if (_context.FooterNavigations.Any(s => s.NavigationName == NavigationName && s.NavigationID != NavigationID))
            {
                TempData["ErrorMessage"] = "Navigation with the same name already exist.";
                return RedirectToAction("NewFooterNavigation", "Admin");
            }

            try
            {
                FooterNavigationModel navigationsModel = _context.FooterNavigations.Single(u => u.NavigationID == NavigationID);
                navigationsModel.NavigationID = NavigationID;
                navigationsModel.NavigationName = NavigationName;
                navigationsModel.NavigationLink = NavigationLink;
                navigationsModel.Status = (NavigationStatus == "1") ? 1 : 0;
                navigationsModel.Order = functions.Int32Parse(NavigationOrder, 10); //set default order as 10
                navigationsModel.UpdatedAt = DateTime.Now;
                navigationsModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("FooterNavigation", "NavigationID", navigationsModel.NavigationID, "CreatedAt"));

                _context.FooterNavigations.Update(navigationsModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("FooterNavigation", "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Navigation Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("EditFooterNavigation", "Admin", new { id = NavigationID });
            }
        }

        //      ██╗ ██╗ ██╗     ██████╗  █████╗  ██████╗ ███████╗███████╗         ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔══██╗██╔══██╗██╔════╝ ██╔════╝██╔════╝        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗██████╔╝███████║██║  ███╗█████╗  ███████╗█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██╔═══╝ ██╔══██║██║   ██║██╔══╝  ╚════██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ██║     ██║  ██║╚██████╔╝███████╗███████║     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚══════╝     ╚═╝ ╚═╝ ╚═╝    
        //                                                                                   
        // GET: Pages
        /// <summary>
        /// My activities view
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> Pages([FromQuery(Name = "p")] string p = "1", [FromQuery(Name = "s")] string s = "100")
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            ViewBag.PageNo = 1;
            ViewBag.PageSize = 100;
            if (!string.IsNullOrEmpty(p) && !string.IsNullOrEmpty(s))
            {
                ViewBag.PageNo = Int32.Parse(p);
                ViewBag.PageSize = Int32.Parse(s);
            }
            ViewBag.PageSkip = (ViewBag.PageNo - 1) * ViewBag.PageSize;
            int PageSkip = ViewBag.PageSkip;
            int PageSize = ViewBag.PageSize;
            ViewBag.TotalRecords = _context.Posts.Count(s => s.Author == AccountID);

            var postsModel = _context.Pages.Where(s => s.Author == AccountID).OrderByDescending(s => s.ID).Skip(PageSkip).Take(PageSize);

            return View(await postsModel.ToListAsync());
        }



        /// <summary>
        /// New Page view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public IActionResult NewPage()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            return View();
        }


        /// <summary>
        /// Adds new page
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPage(PageModel pagesModel, List<IFormFile> PageImage, List<IFormFile> ImageGallery)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            //validate required content
            string[] ValidationInputs = { pagesModel.Content };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required content.";
                return View(pagesModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Set default post values
                    pagesModel.PageID = functions.GetGuid();

                    //if post image posted, upload image and set value
                    if (PageImage.Count > 0)
                    {
                        var DirectoryName = DateTime.Now.ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\pages\\images\\" + DirectoryName + "\\";
                        pagesModel.PageImage = functions.UploadImage(PageImage, 1, 1420, 820, null, SavePath);
                    }

                    //if gallery images posted, upload gallery
                    if (ImageGallery.Count > 0)
                    {
                        var DirectoryName = DateTime.Now.ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\pages\\images\\" + DirectoryName + "\\";
                        functions.UploadImages(ImageGallery, 25, 1420, 820, null, SavePath, pagesModel.PageID); 
                    }

                    pagesModel.Author = AccountID;
                    pagesModel.Slug = functions.GenerateSlug(pagesModel.Title, "Page");
                    pagesModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    pagesModel.SEOTitle = (string.IsNullOrEmpty(HttpContext.Request.Form["SEOTitle"])) ? pagesModel.Title : HttpContext.Request.Form["SEOTitle"].ToString();
                    pagesModel.SEODescription = (string.IsNullOrEmpty(HttpContext.Request.Form["SEODescription"])) ? pagesModel.Title : HttpContext.Request.Form["SEODescription"].ToString();
                    pagesModel.SEOKeywords = (string.IsNullOrEmpty(HttpContext.Request.Form["SEOKeywords"])) ? pagesModel.Title : HttpContext.Request.Form["SEOKeywords"].ToString();
                    pagesModel.UpdatedAt = DateTime.Now;
                    pagesModel.CreatedAt = DateTime.Now;

                    _context.Add(pagesModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                    return RedirectToAction("Pages", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("New Page Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(pagesModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(pagesModel);
            }

        }



        /// <summary>
        /// Edit page view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> EditPage(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var pagesModel = await _context.Pages
               .FirstOrDefaultAsync(m => m.PageID == id);
            if (pagesModel == null)
            {
                return NotFound();
            }
            return View(pagesModel);
        }



        /// <summary>
        /// Edits page
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPage(PageModel pagesModel, List<IFormFile> PageImage, List<IFormFile> ImageGallery)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            //get categories
            ViewBag.CategoriesData = _context.Categories.Where(x => x.Status == 1).OrderBy(s => s.Order);

            //validate required content
            string[] ValidationInputs = { pagesModel.Content };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required content.";
                return View(pagesModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //if post image posted, upload image and set value
                    if (PageImage.Count > 0)
                    {
                        var DirectoryName = Convert.ToDateTime(functions.GetPageData(pagesModel.PageID, "CreatedAt")).ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\pages\\images\\" + DirectoryName + "\\";
                        pagesModel.PageImage = functions.UploadImage(PageImage, 1, 1420, 820, null, SavePath);
                    }
                    else
                    {
                        //set to current post image
                        pagesModel.PageImage = functions.GetPageData(pagesModel.PageID, "PageImage");
                    }

                    //if gallery images posted, upload gallery
                    if (ImageGallery.Count > 0)
                    {
                        var DirectoryName = Convert.ToDateTime(functions.GetPageData(pagesModel.PageID, "CreatedAt")).ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\pages\\images\\" + DirectoryName + "\\";
                        functions.UploadImages(ImageGallery, 25, 1420, 820, null, SavePath, pagesModel.PageID); 
                    }

                    pagesModel.Author = AccountID;
                    //if title different, re-generate slug
                    string CurrentTitle = functions.GetTableData("Pages", "PageID", pagesModel.PageID, "Title");
                    if (CurrentTitle != pagesModel.Title)
                    {
                        pagesModel.Slug = functions.GenerateSlug(pagesModel.Title, "Page");
                    }

                    pagesModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    pagesModel.SEOTitle = (string.IsNullOrEmpty(HttpContext.Request.Form["SEOTitle"])) ? pagesModel.Title : HttpContext.Request.Form["SEOTitle"].ToString();
                    pagesModel.SEODescription = (string.IsNullOrEmpty(HttpContext.Request.Form["SEODescription"])) ? pagesModel.Title : HttpContext.Request.Form["SEODescription"].ToString();
                    pagesModel.SEOKeywords = (string.IsNullOrEmpty(HttpContext.Request.Form["SEOKeywords"])) ? pagesModel.Title : HttpContext.Request.Form["SEOKeywords"].ToString();
                    pagesModel.UpdatedAt = DateTime.Now;
                    pagesModel.CreatedAt = Convert.ToDateTime(functions.GetPageData(pagesModel.PageID, "CreatedAt"));

                    _context.Update(pagesModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                    return RedirectToAction("Pages", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Edit Page Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(pagesModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(pagesModel);
            }
        }




        //      ██╗ ██╗ ██╗     ███████╗███████╗██████╗ ██╗   ██╗██╗ ██████╗███████╗███████╗         ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔════╝██╔════╝██╔══██╗██║   ██║██║██╔════╝██╔════╝██╔════╝        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗███████╗█████╗  ██████╔╝██║   ██║██║██║     █████╗  ███████╗█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝╚════██║██╔══╝  ██╔══██╗╚██╗ ██╔╝██║██║     ██╔══╝  ╚════██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ███████║███████╗██║  ██║ ╚████╔╝ ██║╚██████╗███████╗███████║     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚══════╝╚══════╝╚═╝  ╚═╝  ╚═══╝  ╚═╝ ╚═════╝╚══════╝╚══════╝     ╚═╝ ╚═╝ ╚═╝    
        //                                                                                                      
        // GET: Services
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> Services()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            var servicesModel = _context.Services.OrderByDescending(s => s.CreatedAt);
            return View(await servicesModel.ToListAsync());
        }


        /// <summary>
        /// New service view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public IActionResult NewService()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            return View();
        }


        /// <summary>
        /// Creates new service
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewService(ServiceModel serviceModel)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    //Set default post values
                    serviceModel.ServiceID = functions.GetGuid();

                    serviceModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    serviceModel.UpdatedAt = DateTime.Now;
                    serviceModel.CreatedAt = DateTime.Now;

                    _context.Add(serviceModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                    return RedirectToAction("Services", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Create Services Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(serviceModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(serviceModel);
            }
        }


        /// <summary>
        /// Edit service view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> EditService(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var servicesModel = await _context.Services
               .FirstOrDefaultAsync(m => m.ServiceID == id);
            if (servicesModel == null)
            {
                return NotFound();
            }
            return View(servicesModel);
        }

        /// <summary>
        /// Updates service data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(ServiceModel serviceModel)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    serviceModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    serviceModel.UpdatedAt = DateTime.Now;
                    serviceModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("Services", "ServiceID", serviceModel.ServiceID, "CreatedAt"));

                    _context.Update(serviceModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                    return RedirectToAction("Services", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Edit Services Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(serviceModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(serviceModel);
            }
        }


        //      ██╗ ██╗ ██╗     ██████╗  ██████╗ ██████╗ ████████╗███████╗ ██████╗ ██╗     ██╗ ██████╗          ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔══██╗██╔═══██╗██╔══██╗╚══██╔══╝██╔════╝██╔═══██╗██║     ██║██╔═══██╗        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗██████╔╝██║   ██║██████╔╝   ██║   █████╗  ██║   ██║██║     ██║██║   ██║█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██╔═══╝ ██║   ██║██╔══██╗   ██║   ██╔══╝  ██║   ██║██║     ██║██║   ██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ██║     ╚██████╔╝██║  ██║   ██║   ██║     ╚██████╔╝███████╗██║╚██████╔╝     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚═╝      ╚═════╝ ╚═╝  ╚═╝   ╚═╝   ╚═╝      ╚═════╝ ╚══════╝╚═╝ ╚═════╝      ╚═╝ ╚═╝ ╚═╝    
        //                                                                                                                 
        // GET: Portfolio
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> Portfolio()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            var portfolioModel = _context.Portfolios.OrderByDescending(s => s.CreatedAt);
            return View(await portfolioModel.ToListAsync());
        }


        /// <summary>
        /// New portfolio view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public IActionResult NewPortfolio()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            return View();
        }


        /// <summary>
        /// Creates new portfolio
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPortfolio(PortfolioModel portfolioModel, List<IFormFile> Image, List<IFormFile> ImageGallery)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    //Set default post values
                    portfolioModel.PortfolioID = functions.GetGuid();


                    //if post image posted, upload image and set value
                    if (Image.Count > 0)
                    {
                        var DirectoryName = DateTime.Now.ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\portfolio\\images\\" + DirectoryName + "\\";
                        portfolioModel.Image = functions.UploadImage(Image, 1, 1585, 790, null, SavePath);
                    }

                    //if gallery images posted, upload gallery
                    if (ImageGallery.Count > 0)
                    {
                        var DirectoryName = DateTime.Now.ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\portfolio\\images\\" + DirectoryName + "\\";
                        functions.UploadImages(ImageGallery, 25, 1585, 790, null, SavePath, portfolioModel.PortfolioID); 
                    }

                    portfolioModel.Slug = functions.GenerateSlug(portfolioModel.PortfolioTitle, "Portfolio");
                    portfolioModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    portfolioModel.UpdatedAt = DateTime.Now;
                    portfolioModel.CreatedAt = DateTime.Now;

                    _context.Add(portfolioModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                    return RedirectToAction("Portfolio", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Create Portfolio Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(portfolioModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(portfolioModel);
            }
        }


        /// <summary>
        /// Edit portfolio view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> EditPortfolio(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var portfoliosModel = await _context.Portfolios
               .FirstOrDefaultAsync(m => m.PortfolioID == id);
            if (portfoliosModel == null)
            {
                return NotFound();
            }
            return View(portfoliosModel);
        }


        /// <summary>
        /// Updates portfolio data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPortfolio(PortfolioModel portfolioModel, List<IFormFile> Image, List<IFormFile> ImageGallery)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    //if post image posted, upload image and set value
                    if (Image.Count > 0)
                    {
                        var DirectoryName = Convert.ToDateTime(functions.GetTableData("Portfolio", "PortfolioID", portfolioModel.PortfolioID, "CreatedAt")).ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\portfolio\\images\\" + DirectoryName + "\\";
                        portfolioModel.Image = functions.UploadImage(Image, 1, 1585, 790, null, SavePath);
                    }
                    else
                    {
                        //set to current post image
                        portfolioModel.Image = functions.GetTableData("Portfolio", "PortfolioID", portfolioModel.PortfolioID, "Image");
                    }

                    //if gallery images posted, upload gallery
                    if (ImageGallery.Count > 0)
                    {
                        var DirectoryName = Convert.ToDateTime(functions.GetTableData("Portfolio", "PortfolioID", portfolioModel.PortfolioID, "CreatedAt")).ToString("MM-yyyy");
                        var SavePath = @"wwwroot\\portfolio\\images\\" + DirectoryName + "\\";
                        functions.UploadImages(ImageGallery, 25, 1585, 790, null, SavePath, portfolioModel.PortfolioID); 
                    }

                    //if title different, re-generate slug
                    string CurrentTitle = functions.GetTableData("Portfolio", "PortfolioID", portfolioModel.PortfolioID, "PortfolioTitle");
                    if (CurrentTitle != portfolioModel.PortfolioTitle)
                    {
                        portfolioModel.Slug = functions.GenerateSlug(portfolioModel.PortfolioTitle, "Portfolio");
                    }

                    portfolioModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    portfolioModel.UpdatedAt = DateTime.Now;
                    portfolioModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("Portfolio", "PortfolioID", portfolioModel.PortfolioID, "CreatedAt"));

                    _context.Update(portfolioModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                    return RedirectToAction("Portfolio", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Edit Portfolio Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(portfolioModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(portfolioModel);
            }
        }

        //      ██╗ ██╗ ██╗  ████████╗███████╗ █████╗ ███╗   ███╗         ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝  ╚══██╔══╝██╔════╝██╔══██╗████╗ ████║        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗██║   █████╗  ███████║██╔████╔██║█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██║   ██╔══╝  ██╔══██║██║╚██╔╝██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ██║   ███████╗██║  ██║██║ ╚═╝ ██║     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚═╝   ╚══════╝╚═╝  ╚═╝╚═╝     ╚═╝     ╚═╝ ╚═╝ ╚═╝    
        //                                                                           
        // GET: Team
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> Team()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            var teamsModel = _context.Teams.OrderByDescending(s => s.CreatedAt);
            return View(await teamsModel.ToListAsync());
        }


        /// <summary>
        /// New team member view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public IActionResult NewTeamMember()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            return View();
        }


        /// <summary>
        /// Creates new team member
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewTeamMember(TeamModel teamModel, List<IFormFile> ProfileImage)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    //Set default post values
                    teamModel.TeamID = functions.GetGuid();

                    //if post image posted, upload image and set value
                    if (ProfileImage.Count > 0)
                    {
                        var SavePath = @"wwwroot\\team\\images\\";
                        teamModel.ProfileImage = functions.UploadImage(ProfileImage, 1, 300, 300, null, SavePath);
                    }

                    teamModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    teamModel.UpdatedAt = DateTime.Now;
                    teamModel.CreatedAt = DateTime.Now;

                    _context.Add(teamModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                    return RedirectToAction("Team", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Create Team Member Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(teamModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(teamModel);
            }
        }


        /// <summary>
        /// Edit team member view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> EditTeamMember(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var teamModel = await _context.Teams
               .FirstOrDefaultAsync(m => m.TeamID == id);
            if (teamModel == null)
            {
                return NotFound();
            }
            return View(teamModel);
        }

        /// <summary>
        /// Updates team member data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeamMember(TeamModel teamModel, List<IFormFile> ProfileImage)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    //if post image posted, upload image and set value
                    if (ProfileImage.Count > 0)
                    {
                        var SavePath = @"wwwroot\\team\\images\\";
                        teamModel.ProfileImage = functions.UploadImage(ProfileImage, 1, 300, 300, null, SavePath);
                    }
                    else
                    {
                        //set to current post image
                        teamModel.ProfileImage = functions.GetTableData("Team", "TeamID", teamModel.TeamID, "ProfileImage");
                    }

                    teamModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    teamModel.UpdatedAt = DateTime.Now;
                    teamModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("Team", "TeamID", teamModel.TeamID, "CreatedAt"));

                    _context.Update(teamModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                    return RedirectToAction("Team", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Edit Team Member Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(teamModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(teamModel);
            }
        }


        //      ██╗ ██╗ ██╗     ██████╗  █████╗ ██████╗ ████████╗███╗   ██╗███████╗██████╗ ███████╗         ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔══██╗██╔══██╗██╔══██╗╚══██╔══╝████╗  ██║██╔════╝██╔══██╗██╔════╝        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗██████╔╝███████║██████╔╝   ██║   ██╔██╗ ██║█████╗  ██████╔╝███████╗█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██╔═══╝ ██╔══██║██╔══██╗   ██║   ██║╚██╗██║██╔══╝  ██╔══██╗╚════██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ██║     ██║  ██║██║  ██║   ██║   ██║ ╚████║███████╗██║  ██║███████║     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝╚══════╝     ╚═╝ ╚═╝ ╚═╝    
        //                                                                                                             
        // GET: Partners
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> Partners()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            var partnersModel = _context.Partners.OrderByDescending(s => s.CreatedAt);
            return View(await partnersModel.ToListAsync());
        }


        /// <summary>
        /// New partner view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public IActionResult NewPartner()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            return View();
        }


        /// <summary>
        /// Creates new partner
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPartner(PartnerModel partnerModel, List<IFormFile> Logo)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    //Set default post values
                    partnerModel.PartnerID = functions.GetGuid();


                    //if post image posted, upload image and set value
                    if (Logo.Count > 0)
                    {
                        var SavePath = @"wwwroot\\partners\\images\\";
                        partnerModel.Logo = functions.UploadImage(Logo, 1, 300, 300, null, SavePath);
                    }

                    partnerModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    partnerModel.UpdatedAt = DateTime.Now;
                    partnerModel.CreatedAt = DateTime.Now;

                    _context.Add(partnerModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                    return RedirectToAction("Partners", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Create Partner Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(partnerModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(partnerModel);
            }
        }


        /// <summary>
        /// Edit partner view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> EditPartner(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var partnersModel = await _context.Partners
               .FirstOrDefaultAsync(m => m.PartnerID == id);
            if (partnersModel == null)
            {
                return NotFound();
            }
            return View(partnersModel);
        }


        /// <summary>
        /// Updates partner data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPartner(PartnerModel partnerModel, List<IFormFile> Logo)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    //if post image posted, upload image and set value
                    if (Logo.Count > 0)
                    {
                        var SavePath = @"wwwroot\\partners\\images\\";
                        partnerModel.Logo = functions.UploadImage(Logo, 1, 300, 300, null, SavePath);
                    }
                    else
                    {
                        //set to current post image
                        partnerModel.Logo = functions.GetTableData("Partner", "PartnerID", partnerModel.PartnerID, "Logo");
                    }

                    partnerModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    partnerModel.UpdatedAt = DateTime.Now;
                    partnerModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("Partner", "PartnerID", partnerModel.PartnerID, "CreatedAt"));

                    _context.Update(partnerModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                    return RedirectToAction("Partners", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Edit Partner Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(partnerModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(partnerModel);
            }
        }



        //      ██╗ ██╗ ██╗      ██████╗ ██████╗ ███╗   ██╗████████╗ █████╗  ██████╗████████╗      ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔════╝██╔═══██╗████╗  ██║╚══██╔══╝██╔══██╗██╔════╝╚══██╔══╝     ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗██║     ██║   ██║██╔██╗ ██║   ██║   ███████║██║        ██║█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██║     ██║   ██║██║╚██╗██║   ██║   ██╔══██║██║        ██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ╚██████╗╚██████╔╝██║ ╚████║   ██║   ██║  ██║╚██████╗   ██║     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝          ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝   ╚═╝     ╚═╝ ╚═╝ ╚═╝    
        //                                                                                                    
        /// <summary>
        /// Contact view
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Admin Permissions")]
        public async Task<IActionResult> Contact(string id, [FromQuery(Name = "urid")] string urid, [FromQuery(Name = "p")] string p = "1", [FromQuery(Name = "s")] string s = "250")
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            ViewBag.PageNo = 1;
            ViewBag.PageSize = 250;
            if (!string.IsNullOrEmpty(p) && !string.IsNullOrEmpty(s))
            {
                ViewBag.PageNo = Int32.Parse(p);
                ViewBag.PageSize = Int32.Parse(s);
            }
            ViewBag.PageSkip = (ViewBag.PageNo - 1) * ViewBag.PageSize;
            int PageSkip = ViewBag.PageSkip;
            int PageSize = ViewBag.PageSize;
            ViewBag.TotalRecords = _context.Posts.Count(s => s.Author == AccountID);

            var contactsModel = _context.Contacts.OrderByDescending(s => s.ID).Skip(PageSkip).Take(PageSize);

            ViewBag.MessageID = id;
            //mark as read if not null
            if (!string.IsNullOrEmpty(ViewBag.MessageID))
            {
                functions.MarkMessageRead(ViewBag.MessageID, AccountID);
                var DBQuery = _context.MessageViews.Where(s => s.ContactID == id && s.AccountID == AccountID);
                ViewBag.ViewID = (DBQuery.Any()) ? DBQuery.FirstOrDefault().ViewID : "";
            }

            //mark as unread if not null
            if (!string.IsNullOrEmpty(urid))
            {
                functions.DeleteTableData("MessageViews", "ViewID", urid);
                return RedirectToAction("Contact", "Admin");
            }

            return View(await contactsModel.ToListAsync());
        }






        //      ██╗ ██╗ ██╗     ███████╗ █████╗  ██████╗          ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔════╝██╔══██╗██╔═══██╗        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗█████╗  ███████║██║   ██║█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██╔══╝  ██╔══██║██║▄▄ ██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ██║     ██║  ██║╚██████╔╝     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚═╝     ╚═╝  ╚═╝ ╚══▀▀═╝      ╚═╝ ╚═╝ ╚═╝    
        //                                                                   
        // GET: Faq
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> Faq()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            var faqsModel = _context.FAQs.OrderByDescending(s => s.CreatedAt);
            return View(await faqsModel.ToListAsync());
        }


        /// <summary>
        /// New faq view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public IActionResult NewFaq()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            return View();
        }


        /// <summary>
        /// Creates new faq
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewFaq(FAQModel faqModel)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    //Set default post values
                    faqModel.FaqID = functions.GetGuid();

                    faqModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    faqModel.UpdatedAt = DateTime.Now;
                    faqModel.CreatedAt = DateTime.Now;

                    _context.Add(faqModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                    return RedirectToAction("Faq", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Create Faq Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(faqModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(faqModel);
            }
        }


        /// <summary>
        /// Edit faq view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> EditFaq(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var faqsModel = await _context.FAQs
               .FirstOrDefaultAsync(m => m.FaqID == id);
            if (faqsModel == null)
            {
                return NotFound();
            }
            return View(faqsModel);
        }


        /// <summary>
        /// Updates faq data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFaq(FAQModel faqModel)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    faqModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    faqModel.UpdatedAt = DateTime.Now;
                    faqModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("FAQ", "FaqID", faqModel.FaqID, "CreatedAt"));

                    _context.Update(faqModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                    return RedirectToAction("Faq", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Edit Faq Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(faqModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(faqModel);
            }
        }


        //      ██╗ ██╗ ██╗  ████████╗███████╗███████╗████████╗██╗███╗   ███╗ ██████╗ ███╗   ██╗██╗ █████╗ ██╗     ███████╗         ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝  ╚══██╔══╝██╔════╝██╔════╝╚══██╔══╝██║████╗ ████║██╔═══██╗████╗  ██║██║██╔══██╗██║     ██╔════╝        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗██║   █████╗  ███████╗   ██║   ██║██╔████╔██║██║   ██║██╔██╗ ██║██║███████║██║     ███████╗█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██║   ██╔══╝  ╚════██║   ██║   ██║██║╚██╔╝██║██║   ██║██║╚██╗██║██║██╔══██║██║     ╚════██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ██║   ███████╗███████║   ██║   ██║██║ ╚═╝ ██║╚██████╔╝██║ ╚████║██║██║  ██║███████╗███████║     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚═╝   ╚══════╝╚══════╝   ╚═╝   ╚═╝╚═╝     ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝╚═╝  ╚═╝╚══════╝╚══════╝     ╚═╝ ╚═╝ ╚═╝    
        //                                                                                                                                     
        // GET: Testimonials
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> Testimonials()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            var testimonialsModel = _context.Testimonials.OrderByDescending(s => s.CreatedAt);
            return View(await testimonialsModel.ToListAsync());
        }


        /// <summary>
        /// New testimonial view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public IActionResult NewTestimonial()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            return View();
        }


        /// <summary>
        /// Creates testimonial
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewTestimonial(TestimonialModel testimonialModel, List<IFormFile> ProfileImage)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    //Set default post values
                    testimonialModel.TestimonialID = functions.GetGuid();

                    //if post image posted, upload image and set value
                    if (ProfileImage.Count > 0)
                    {
                        var SavePath = @"wwwroot\\testimonials\\images\\";
                        testimonialModel.ProfileImage = functions.UploadImage(ProfileImage, 1, 300, 300, null, SavePath);
                    }

                    testimonialModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    testimonialModel.UpdatedAt = DateTime.Now;
                    testimonialModel.CreatedAt = DateTime.Now;

                    _context.Add(testimonialModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                    return RedirectToAction("Testimonials", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Create Testimonial Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(testimonialModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(testimonialModel);
            }
        }


        /// <summary>
        /// Edit testimonial view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> EditTestimonial(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var testimonialModel = await _context.Testimonials
               .FirstOrDefaultAsync(m => m.TestimonialID == id);
            if (testimonialModel == null)
            {
                return NotFound();
            }
            return View(testimonialModel);
        }


        /// <summary>
        /// Updates testimonial data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTestimonial(TestimonialModel testimonialModel, List<IFormFile> ProfileImage)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (ModelState.IsValid)
            {
                try
                {
                    //if post image posted, upload image and set value
                    if (ProfileImage.Count > 0)
                    {
                        var SavePath = @"wwwroot\\testimonials\\images\\";
                        testimonialModel.ProfileImage = functions.UploadImage(ProfileImage, 1, 300, 300, null, SavePath);
                    }
                    else
                    {
                        //set to current post image
                        testimonialModel.ProfileImage = functions.GetTableData("Testimonials", "TestimonialID", testimonialModel.TestimonialID, "ProfileImage");
                    }

                    testimonialModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                    testimonialModel.UpdatedAt = DateTime.Now;
                    testimonialModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("Testimonials", "TestimonialID", testimonialModel.TestimonialID, "CreatedAt"));

                    _context.Update(testimonialModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                    return RedirectToAction("Testimonials", "Admin");
                }
                catch (Exception ex)
                {
                    //Log Error
                    _logger.LogInformation("Edit Testimonial Error: " + ex.ToString());
                    TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                    return View(testimonialModel);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return View(testimonialModel);
            }
        }





        //      ██╗ ██╗ ██╗      █████╗ ██████╗ ██████╗ ███████╗ █████╗ ██████╗  █████╗ ███╗   ██╗ ██████╗███████╗       ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗██╔══██╗██╔══██╗████╗  ██║██╔════╝██╔════╝      ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗███████║██████╔╝██████╔╝█████╗  ███████║██████╔╝███████║██╔██╗ ██║██║     █████╗█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██╔══██║██╔═══╝ ██╔═══╝ ██╔══╝  ██╔══██║██╔══██╗██╔══██║██║╚██╗██║██║     ██╔══╝╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ██║  ██║██║     ██║     ███████╗██║  ██║██║  ██║██║  ██║██║ ╚████║╚██████╗███████╗   ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚═╝  ╚═╝╚═╝     ╚═╝     ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝ ╚═════╝╚══════╝   ╚═╝ ╚═╝ ╚═╝    
        //       


        // GET: Appearance
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public IActionResult Appearance()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            ViewBag.SlidersData = _context.HomeSliders.OrderByDescending(s => s.ID);
            ViewBag.SlidersDataCount = _context.HomeSliders.Count();

            ViewBag.AvailableTheme = _systemConfiguration.availableThemes;

            return View();
        }


        /// <summary>
        /// Creates slider
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewSlider(HomeSliderModel sliderModel, List<IFormFile> SliderImage)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            try
            {
                //Set default post values
                sliderModel.SliderID = functions.GetGuid();

                //get post inputs
                string SliderTitle = HttpContext.Request.Form["SliderTitle"];
                string SliderSubText = HttpContext.Request.Form["SliderSubText"];
                string SliderLink = HttpContext.Request.Form["SliderLink"];
                string SliderStatus = HttpContext.Request.Form["Status"];

                //validate required inputs
                string[] ValidationInputs = { SliderTitle, SliderStatus };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                    return RedirectToAction("Appearance", "Admin");
                }

                //if post image posted, upload image and set value
                if (SliderImage.Count > 0)
                {
                    var SavePath = @"wwwroot\\sliders\\images\\";
                    sliderModel.SliderImage = functions.UploadImage(SliderImage, 1, 1200, 750, null, SavePath);
                }

                sliderModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                sliderModel.UpdatedAt = DateTime.Now;
                sliderModel.CreatedAt = DateTime.Now;

                _context.Add(sliderModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                return RedirectToAction("Appearance", "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Create Slider Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Appearance", "Admin");
            }
        }


        /// <summary>
        /// Edits slider
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSlider(List<IFormFile> SliderImage)
        {
            //get input values
            string SliderID = HttpContext.Request.Form["SliderID"];
            string SliderTitle = HttpContext.Request.Form["SliderTitle"];
            string SliderSubText = HttpContext.Request.Form["SliderSubText"];
            string SliderLink = HttpContext.Request.Form["SliderLink"];
            string SliderStatus = HttpContext.Request.Form["SliderStatus"];
            string ReturnView = HttpContext.Request.Form["ReturnView"];
            string ReturnViewID = HttpContext.Request.Form["ReturnViewID"];
            ReturnView = (!string.IsNullOrEmpty(ReturnView)) ? ReturnView : "Appearance";

            //validate required inputs
            string[] ValidationInputs = { SliderID, SliderTitle, SliderStatus };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return RedirectToAction(ReturnView, "Admin");
            }

            //validate unique category name
            if (_context.Categories.Any(s => s.CategoryName == SliderTitle && s.CategoryID != SliderID))
            {
                TempData["ErrorMessage"] = "Slider with the same title already exist.";
                return RedirectToAction(ReturnView, "Admin");
            }

            try
            {
                //set to current sloder image
                string UpdatedSliderImage = functions.GetTableData("HomeSliders", "SliderID", SliderID, "SliderImage");
                //if post image posted, upload image and set value
                if (SliderImage.Count > 0)
                {
                    var SavePath = @"wwwroot\\sliders\\images\\";
                    UpdatedSliderImage = functions.UploadImage(SliderImage, 1, 1200, 750, null, SavePath);
                }

                HomeSliderModel slidersModel = _context.HomeSliders.Single(u => u.SliderID == SliderID);
                slidersModel.SliderID = SliderID;
                slidersModel.SliderTitle = SliderTitle;
                slidersModel.SubText = SliderSubText;
                slidersModel.SliderLink = SliderLink;
                slidersModel.SliderImage = UpdatedSliderImage;
                slidersModel.Status = (SliderStatus == "1") ? 1 : 0;
                slidersModel.UpdatedAt = DateTime.Now;
                slidersModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("HomeSliders", "SliderID", slidersModel.SliderID, "CreatedAt"));

                _context.HomeSliders.Update(slidersModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction(ReturnView, "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Slider Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction(ReturnView, "Admin");
            }
        }


        /// <summary>
        /// Edits theme colors
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateThemeColors()
        {
            string ThemeColor = HttpContext.Request.Form["ThemeColor"];
            try
            {
                //validate required inputs
                string[] ValidationInputs = { ThemeColor };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                    return RedirectToAction("Appearance", "Admin", new { tab = "theme-colors" });
                }

                //Update settings
                functions.UpdateTableData("ThemeSettings", "SettingName", "ThemeColor", "SettingValue", ThemeColor);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Appearance", "Admin", new { tab = "theme-colors" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Theme Color Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Appearance", "Admin", new { tab = "theme-colors" });
            }
        }



        /// <summary>
        /// Updates selected theme
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTheme()
        {
            string Theme = HttpContext.Request.Form["Theme"];
            try
            {
                //validate required inputs
                string[] ValidationInputs = { Theme };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                    return RedirectToAction("Appearance", "Admin", new { tab = "theme" });
                }

                //Update settings
                functions.UpdateTableData("ThemeSettings", "SettingName", "SelectedTheme", "SettingValue", Theme);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Appearance", "Admin", new { tab = "theme" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Theme Color Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Appearance", "Admin", new { tab = "theme" });
            }
        }


        //      ██╗ ██╗      ██████╗ ██████╗ ███╗   ██╗████████╗███████╗███╗   ██╗████████╗    ███╗   ███╗ ██████╗████████╗             ██╗ ██╗
        //     ██╔╝██╔╝     ██╔════╝██╔═══██╗████╗  ██║╚══██╔══╝██╔════╝████╗  ██║╚══██╔══╝    ████╗ ████║██╔════╝╚══██╔══╝            ██╔╝██╔╝
        //    ██╔╝██╔╝█████╗██║     ██║   ██║██╔██╗ ██║   ██║   █████╗  ██╔██╗ ██║   ██║       ██╔████╔██║██║  ███╗  ██║       █████╗ ██╔╝██╔╝ 
        //   ██╔╝██╔╝ ╚════╝██║     ██║   ██║██║╚██╗██║   ██║   ██╔══╝  ██║╚██╗██║   ██║       ██║╚██╔╝██║██║   ██║  ██║       ╚════╝██╔╝██╔╝  
        //  ██╔╝██╔╝        ╚██████╗╚██████╔╝██║ ╚████║   ██║   ███████╗██║ ╚████║   ██║       ██║ ╚═╝ ██║╚██████╔╝  ██║██╗         ██╔╝██╔╝   
        //  ╚═╝ ╚═╝          ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝   ╚═╝       ╚═╝     ╚═╝ ╚═════╝   ╚═╝╚═╝         ╚═╝ ╚═╝    
        //                                                                                                                                     

        // GET: ContentManagement
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public IActionResult ContentManagement()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            //get service stats data
            ViewBag.ServiceStatsData = _context.ServiceStats.OrderBy(s => s.Order);
            ViewBag.ServiceStatsCount = _context.ServiceStats.Count();

            //get donation campaigns data
            ViewBag.DonationsData = _context.DonationCampaigns.OrderByDescending(s => s.ID);
            ViewBag.DonationsCount = _context.DonationCampaigns.Count();

            //get resources data
            ViewBag.ResourcesData = _context.DocumentResources.OrderByDescending(s => s.CreatedAt);
            ViewBag.ResourcesDataCount = _context.DocumentResources.Count();

            //get videos data
            ViewBag.VideosData = _context.Videos.OrderByDescending(s => s.CreatedAt);
            ViewBag.VideosDataCount = _context.Videos.Count();

            //get pages data
            ViewBag.PagesData = _context.Pages.OrderByDescending(s => s.CreatedAt);
            ViewBag.PagesDataCount = _context.Pages.Count();

            //get page sections data
            ViewBag.PageSectionsData = _context.PageSections.OrderBy(s => s.SectionOrder);
            ViewBag.PageSectionsCount = _context.PageSections.Count();

            return View();
        }


        /// <summary>
        /// Edits organization info
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrganizationInfo(List<IFormFile> Logo, List<IFormFile> Favicon)
        {
            string OrganizationName = HttpContext.Request.Form["OrganizationName"];
            string OrganizationAddress = HttpContext.Request.Form["OrganizationAddress"];
            string PhoneNumber = HttpContext.Request.Form["PhoneNumber"];
            string OptionalPhoneNumber = HttpContext.Request.Form["OptionalPhoneNumber"];
            string Email = HttpContext.Request.Form["Email"];
            string LogoFormat = HttpContext.Request.Form["LogoFormat"];
            string LogoWidth = HttpContext.Request.Form["LogoWidth"];
            string LogoFileName = functions.GetTableData("ContentManagement", "ContentName", "Logo", "ContentValue");
            string FaviconFileName = functions.GetTableData("ContentManagement", "ContentName", "Favicon", "ContentValue");

            try
            {
                //validate required inputs
                string[] ValidationInputs = { OrganizationName, OrganizationAddress, PhoneNumber, Email };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                    return RedirectToAction("ContentManagement", "Admin");
                }

                //if logo image posted, upload image and set value
                if (Logo.Count > 0)
                {
                    var SavePath = @"wwwroot\\images\\";
                    LogoFileName = functions.GetTableData("ContentManagement", "ContentName", "Logo", "ContentValue");
                    LogoFileName = functions.UploadImage(Logo, 1, SavePath);
                }

                //if favicon image posted, upload image and set value
                if (Favicon.Count > 0)
                {
                    var SavePath = @"wwwroot\\images\\";
                    FaviconFileName = functions.UploadImage(Favicon, 1, 60, 60, null, SavePath);
                }

                //Update settings
                functions.UpdateTableData("ContentManagement", "ContentName", "OrganizationName", "ContentValue", OrganizationName);
                functions.UpdateTableData("ContentManagement", "ContentName", "OrganizationAddress", "ContentValue", OrganizationAddress);
                functions.UpdateTableData("ContentManagement", "ContentName", "PhoneNumber", "ContentValue", PhoneNumber);
                functions.UpdateTableData("ContentManagement", "ContentName", "OptionalPhoneNumber", "ContentValue", OptionalPhoneNumber);
                functions.UpdateTableData("ContentManagement", "ContentName", "Email", "ContentValue", Email);
                functions.UpdateTableData("ContentManagement", "ContentName", "Logo", "ContentValue", LogoFileName);
                functions.UpdateTableData("ContentManagement", "ContentName", "LogoFormat", "ContentValue", LogoFormat);
                functions.UpdateTableData("ContentManagement", "ContentName", "LogoWidth", "ContentValue", LogoWidth);
                functions.UpdateTableData("ContentManagement", "ContentName", "Favicon", "ContentValue", FaviconFileName);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin");

            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Organization Info Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin");
            }
        }


        /// <summary>
        /// Edits organization social media links 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSocialInfo()
        {
            string FacebookLink = HttpContext.Request.Form["FacebookLink"];
            string TwitterLink = HttpContext.Request.Form["TwitterLink"];
            string InstagramLink = HttpContext.Request.Form["InstagramLink"];
            string YouTubeLink = HttpContext.Request.Form["YouTubeLink"];
            string LinkedInLink = HttpContext.Request.Form["LinkedInLink"];

            try
            {
                //Update settings
                functions.UpdateTableData("ContentManagement", "ContentName", "FacebookLink", "ContentValue", FacebookLink);
                functions.UpdateTableData("ContentManagement", "ContentName", "TwitterLink", "ContentValue", TwitterLink);
                functions.UpdateTableData("ContentManagement", "ContentName", "InstagramLink", "ContentValue", InstagramLink);
                functions.UpdateTableData("ContentManagement", "ContentName", "YouTubeLink", "ContentValue", YouTubeLink);
                functions.UpdateTableData("ContentManagement", "ContentName", "LinkedInLink", "ContentValue", LinkedInLink);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "social" });

            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Organization Social Info Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "social" });
            }
        }



        /// <summary>
        /// Creates service stat
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewServiceStat(ServiceStatModel serviceStatModel)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            try
            {
                //Set default post values
                serviceStatModel.ServiceStatID = functions.GetGuid();

                //get post inputs
                string Title = HttpContext.Request.Form["Title"];
                string StatValue = HttpContext.Request.Form["StatValue"];
                string Icon = HttpContext.Request.Form["Icon"];
                string Link = HttpContext.Request.Form["Link"];
                string Status = HttpContext.Request.Form["Status"];

                //validate required inputs
                string[] ValidationInputs = { Title, StatValue, Status };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                    return RedirectToAction("ContentManagement", "Admin", new { tab = "service-stat" });
                }

                serviceStatModel.Title = Title;
                serviceStatModel.StatValue = StatValue;
                serviceStatModel.Icon = Icon;
                serviceStatModel.Link = Link;

                serviceStatModel.Order = (string.IsNullOrEmpty(HttpContext.Request.Form["Order"])) ? 10 : functions.Int32Parse(HttpContext.Request.Form["Order"]);
                serviceStatModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                serviceStatModel.UpdatedAt = DateTime.Now;
                serviceStatModel.CreatedAt = DateTime.Now;

                _context.Add(serviceStatModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "service-stat" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Create Slider Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "service-stat" });
            }
        }



        /// <summary>
        /// Edits service stat
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditServiceStat()
        {
            //get input values
            string ServiceStatID = HttpContext.Request.Form["ServiceStatID"];
            string ServiceStatTitle = HttpContext.Request.Form["ServiceStatTitle"];
            string ServiceStatValue = HttpContext.Request.Form["ServiceStatValue"];
            string ServiceStatIcon = HttpContext.Request.Form["ServiceStatIcon"];
            string ServiceStatLink = HttpContext.Request.Form["ServiceStatLink"];
            string ServiceStatOrder = HttpContext.Request.Form["ServiceStatOrder"];
            string ServiceStatStatus = HttpContext.Request.Form["ServiceStatStatus"];
            string ReturnView = HttpContext.Request.Form["ReturnView"];
            string ReturnViewID = HttpContext.Request.Form["ReturnViewID"];
            ReturnView = (!string.IsNullOrEmpty(ReturnView)) ? ReturnView : "ContentManagement";

            //validate required inputs
            string[] ValidationInputs = { ServiceStatID, ServiceStatTitle };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return RedirectToAction(ReturnView, "Admin", new { tab = "service-stat" });
            }

            //validate unique service stat name
            if (_context.ServiceStats.Any(s => s.Title == ServiceStatTitle && s.ServiceStatID != ServiceStatID))
            {
                TempData["ErrorMessage"] = "Service stat with the same name already exist.";
                return RedirectToAction(ReturnView, "Admin", new { tab = "service-stat" });
            }

            try
            {
                ServiceStatModel serviceStatsModel = _context.ServiceStats.Single(u => u.ServiceStatID == ServiceStatID);
                serviceStatsModel.ServiceStatID = ServiceStatID;
                serviceStatsModel.Title = ServiceStatTitle;
                serviceStatsModel.StatValue = ServiceStatValue;
                serviceStatsModel.Icon = ServiceStatIcon;
                serviceStatsModel.Link = ServiceStatLink;
                serviceStatsModel.Order = (!string.IsNullOrEmpty(ServiceStatOrder)) ? functions.Int32Parse(ServiceStatOrder, 10) : 10; //set default 10 if empty
                serviceStatsModel.Status = (ServiceStatStatus == "1") ? 1 : 0;
                serviceStatsModel.UpdatedAt = DateTime.Now;
                serviceStatsModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("ServiceStats", "ServiceStatID", serviceStatsModel.ServiceStatID, "CreatedAt"));

                _context.ServiceStats.Update(serviceStatsModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction(ReturnView, "Admin", new { tab = "service-stat" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Service Stat Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction(ReturnView, "Admin", new { tab = "service-stat" });
            }
        }



        /// <summary>
        /// Edits about info
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAboutSummary()
        {
            //get input values
            string AboutSummary = HttpContext.Request.Form["AboutSummary"];
            string AboutSummaryLink = HttpContext.Request.Form["AboutSummaryLink"];

            try
            {
                //validate required inputs
                string[] ValidationInputs = { AboutSummary };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                    return RedirectToAction("ContentManagement", "Admin", new { tab = "service-stat" });
                }

                //Update settings
                functions.UpdateTableData("ContentManagement", "ContentName", "AboutSummary", "ContentValue", AboutSummary);
                functions.UpdateTableData("ContentManagement", "ContentName", "AboutSummaryLink", "ContentValue", AboutSummaryLink);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "about-info" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit About Summary Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "about-info" });
            }
        }


        /// <summary>
        /// Edits home video
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateHomeVideo()
        {
            //get input values
            string HomeVideoTitle = HttpContext.Request.Form["HomeVideoTitle"];
            string HomeVideo = HttpContext.Request.Form["HomeVideo"];
            string HomeVideoDescription = HttpContext.Request.Form["HomeVideoDescription"];

            try
            {
                //validate required inputs
                string[] ValidationInputs = { HomeVideo };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                    return RedirectToAction("ContentManagement", "Admin", new { tab = "service-stat" });
                }

                //Update settings
                functions.UpdateTableData("ContentManagement", "ContentName", "HomeVideoTitle", "ContentValue", HomeVideoTitle);
                functions.UpdateTableData("ContentManagement", "ContentName", "HomeVideo", "ContentValue", HomeVideo);
                functions.UpdateTableData("ContentManagement", "ContentName", "HomeVideoDescription", "ContentValue", HomeVideoDescription);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "about-info" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Home Video Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "about-info" });
            }
        }



        /// <summary>
        /// Creates donation campaign
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewDonationCampaign(DonationCampaignsModel donationModel, List<IFormFile> CampaignImage)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);
            try
            {
                //Set default post values
                donationModel.DonationID = functions.GetGuid();

                //if post image posted, upload image and set value
                if (CampaignImage.Count > 0)
                {
                    var SavePath = @"wwwroot\\campaigns\\images\\";
                    donationModel.CampaignImage = functions.UploadImage(CampaignImage, 1, 400, 500, null, SavePath);
                }

                //get post inputs
                string CampaignTitle = HttpContext.Request.Form["CampaignTitle"];
                string CampaignDescription = HttpContext.Request.Form["CampaignDescription"];
                string DonationType = HttpContext.Request.Form["DonationType"];
                string Link = HttpContext.Request.Form["Link"];
                string Status = HttpContext.Request.Form["Status"];

                //validate required inputs
                string[] ValidationInputs = { CampaignTitle, DonationType, Status };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                    return RedirectToAction("ContentManagement", "Admin", new { tab = "donations" });
                }

                donationModel.CampaignTitle = CampaignTitle;
                donationModel.CampaignDescription = CampaignDescription;
                donationModel.DonationType = DonationType;
                donationModel.Link = Link;

                donationModel.Status = (string.IsNullOrEmpty(HttpContext.Request.Form["Status"])) ? 0 : functions.Int32Parse(HttpContext.Request.Form["Status"]);
                donationModel.UpdatedAt = DateTime.Now;
                donationModel.CreatedAt = DateTime.Now;

                _context.Add(donationModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "donations" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Create Donation Campaign Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "donations" });
            }
        }




        /// <summary>
        /// Edits donation campaign 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDonationCampaign(List<IFormFile> CampaignImage)
        {
            //get input values
            string DonationID = HttpContext.Request.Form["DonationID"];
            string CampaignTitle = HttpContext.Request.Form["DonationTitle"];
            string CampaignDescription = HttpContext.Request.Form["DonationDescription"];
            string DonationType = HttpContext.Request.Form["DonationType"];
            string Link = HttpContext.Request.Form["DonationLink"];
            string Status = HttpContext.Request.Form["DonationCampaignStatus"];
            string ReturnView = HttpContext.Request.Form["ReturnView"];
            string ReturnViewID = HttpContext.Request.Form["ReturnViewID"];
            string UpdatedCampaignImage = "";
            ReturnView = (!string.IsNullOrEmpty(ReturnView)) ? ReturnView : "ContentManagement";

            //validate required inputs
            string[] ValidationInputs = { DonationID, CampaignTitle, DonationType };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return RedirectToAction(ReturnView, "Admin", new { tab = "donations" });
            }

            //if post image posted, upload image and set value
            if (CampaignImage.Count > 0)
            {
                var SavePath = @"wwwroot\\campaigns\\images\\";
                UpdatedCampaignImage = functions.UploadImage(CampaignImage, 1, 400, 500, null, SavePath);
            }
            else
            {
                //set to current post image
                UpdatedCampaignImage = functions.GetTableData("DonationCampaigns", "DonationID", DonationID, "CampaignImage");
            }

            try
            {
                DonationCampaignsModel donationModel = _context.DonationCampaigns.Single(u => u.DonationID == DonationID);
                donationModel.DonationID = DonationID;
                donationModel.CampaignTitle = CampaignTitle;
                donationModel.CampaignDescription = CampaignDescription;
                donationModel.CampaignImage = UpdatedCampaignImage;
                donationModel.DonationType = (!string.IsNullOrEmpty(DonationType)) ? DonationType : "Url";
                donationModel.Link = Link;
                donationModel.Status = (Status == "1") ? 1 : 0;
                donationModel.UpdatedAt = DateTime.Now;
                donationModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("DonationCampaigns", "DonationID", donationModel.DonationID, "CreatedAt"));

                _context.DonationCampaigns.Update(donationModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction(ReturnView, "Admin", new { tab = "donations" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Donation Campaign Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction(ReturnView, "Admin", new { tab = "donations" });
            }
        }



        /// <summary>
        /// Edits header texts
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateHeaderTexts()
        {
            //get input values
            string AboutUs = HttpContext.Request.Form["AboutUs"];
            string Services = HttpContext.Request.Form["Services"];
            string Testimonials = HttpContext.Request.Form["Testimonials"];
            string Posts = HttpContext.Request.Form["Posts"];
            string Donate = HttpContext.Request.Form["Donate"];
            string Portfolio = HttpContext.Request.Form["Portfolio"];
            string Team = HttpContext.Request.Form["Team"];
            string FAQ = HttpContext.Request.Form["FAQ"];
            string Partners = HttpContext.Request.Form["Partners"];
            string ContactUs = HttpContext.Request.Form["ContactUs"];

            try
            {
                //Update settings
                functions.UpdateTableData("ContentManagement", "ContentName", "AboutUs", "ContentValue", AboutUs);
                functions.UpdateTableData("ContentManagement", "ContentName", "Services", "ContentValue", Services);
                functions.UpdateTableData("ContentManagement", "ContentName", "Testimonials", "ContentValue", Testimonials);
                functions.UpdateTableData("ContentManagement", "ContentName", "Posts", "ContentValue", Posts);
                functions.UpdateTableData("ContentManagement", "ContentName", "Donate", "ContentValue", Donate);
                functions.UpdateTableData("ContentManagement", "ContentName", "Portfolio", "ContentValue", Portfolio);
                functions.UpdateTableData("ContentManagement", "ContentName", "Team", "ContentValue", Team);
                functions.UpdateTableData("ContentManagement", "ContentName", "FAQ", "ContentValue", FAQ);
                functions.UpdateTableData("ContentManagement", "ContentName", "Partners", "ContentValue", Partners);
                functions.UpdateTableData("ContentManagement", "ContentName", "ContactUs", "ContentValue", ContactUs);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "header-text" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Header Texts Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "header-text" });
            }
        }



        /// <summary>
        /// Edits display settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDisplaySettings()
        {
            //get input values
            string HomeSliders = functions.Int32Parse(HttpContext.Request.Form["HomeSliders"]).ToString();
            string SocialMedia = functions.Int32Parse(HttpContext.Request.Form["SocialMedia"]).ToString();
            string ServiceStatistics = functions.Int32Parse(HttpContext.Request.Form["ServiceStatistics"]).ToString();
            string AboutUs = functions.Int32Parse(HttpContext.Request.Form["AboutUs"]).ToString();
            string HomeVideo = functions.Int32Parse(HttpContext.Request.Form["HomeVideo"]).ToString();
            string Services = functions.Int32Parse(HttpContext.Request.Form["Services"]).ToString();
            string Testimonials = functions.Int32Parse(HttpContext.Request.Form["Testimonials"]).ToString();
            string Posts = functions.Int32Parse(HttpContext.Request.Form["Posts"]).ToString();
            string TwitterFeed = functions.Int32Parse(HttpContext.Request.Form["TwitterFeed"]).ToString();
            string Donate = functions.Int32Parse(HttpContext.Request.Form["Donate"]).ToString();
            string Portfolio = functions.Int32Parse(HttpContext.Request.Form["Portfolio"]).ToString();
            string Team = functions.Int32Parse(HttpContext.Request.Form["Team"]).ToString();
            string FAQ = functions.Int32Parse(HttpContext.Request.Form["FAQ"]).ToString();
            string Partners = functions.Int32Parse(HttpContext.Request.Form["Partners"]).ToString();
            string ContactUs = functions.Int32Parse(HttpContext.Request.Form["ContactUs"]).ToString();

            try
            {
                //Update settings
                functions.UpdateTableData("ContentDisplay", "ContentName", "HomeSliders", "DisplayStatus", HomeSliders);
                functions.UpdateTableData("ContentDisplay", "ContentName", "SocialMedia", "DisplayStatus", SocialMedia);
                functions.UpdateTableData("ContentDisplay", "ContentName", "ServiceStatistics", "DisplayStatus", ServiceStatistics);
                functions.UpdateTableData("ContentDisplay", "ContentName", "AboutUs", "DisplayStatus", AboutUs);
                functions.UpdateTableData("ContentDisplay", "ContentName", "HomeVideo", "DisplayStatus", HomeVideo);
                functions.UpdateTableData("ContentDisplay", "ContentName", "Services", "DisplayStatus", Services);
                functions.UpdateTableData("ContentDisplay", "ContentName", "Testimonials", "DisplayStatus", Testimonials);
                functions.UpdateTableData("ContentDisplay", "ContentName", "Posts", "DisplayStatus", Posts);
                functions.UpdateTableData("ContentDisplay", "ContentName", "TwitterFeed", "DisplayStatus", TwitterFeed);
                functions.UpdateTableData("ContentDisplay", "ContentName", "Donate", "DisplayStatus", Donate);
                functions.UpdateTableData("ContentDisplay", "ContentName", "Portfolio", "DisplayStatus", Portfolio);
                functions.UpdateTableData("ContentDisplay", "ContentName", "Team", "DisplayStatus", Team);
                functions.UpdateTableData("ContentDisplay", "ContentName", "FAQ", "DisplayStatus", FAQ);
                functions.UpdateTableData("ContentDisplay", "ContentName", "Partners", "DisplayStatus", Partners);
                functions.UpdateTableData("ContentDisplay", "ContentName", "ContactUs", "DisplayStatus", ContactUs);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "content-display" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Header Texts Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "content-display" });
            }
        }



        /// <summary>
        /// Edits seo data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSeoData()
        {
            //get input values
            string SEOAuthor = HttpContext.Request.Form["SEOAuthor"];
            string SEODescription = HttpContext.Request.Form["SEODescription"];
            string SEOKeywords = HttpContext.Request.Form["SEOKeywords"];

            try
            {
                //Update settings
                functions.UpdateTableData("ContentManagement", "ContentName", "SEOAuthor", "ContentValue", SEOAuthor);
                functions.UpdateTableData("ContentManagement", "ContentName", "SEODescription", "ContentValue", SEODescription);
                functions.UpdateTableData("ContentManagement", "ContentName", "SEOKeywords", "ContentValue", SEOKeywords);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "seo" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit SEO Data Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "seo" });
            }
        }



        /// <summary>
        /// My activities view
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Author Permissions")]
        public async Task<IActionResult> ManageTranslations([FromQuery(Name = "p")] string p = "1", [FromQuery(Name = "s")] string s = "100")
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            ViewBag.PageNo = 1;
            ViewBag.PageSize = 100;
            if (!string.IsNullOrEmpty(p) && !string.IsNullOrEmpty(s))
            {
                ViewBag.PageNo = Int32.Parse(p);
                ViewBag.PageSize = Int32.Parse(s);
            }
            ViewBag.PageSkip = (ViewBag.PageNo - 1) * ViewBag.PageSize;
            int PageSkip = ViewBag.PageSkip;
            int PageSize = ViewBag.PageSize;
            ViewBag.TotalRecords = _context.DataTranslations.Count();

            var dataTranslationsModel = _context.DataTranslations.OrderByDescending(s => s.ID).Skip(PageSkip).Take(PageSize);

            return View(await dataTranslationsModel.ToListAsync());
        }


        //      ██╗ ██╗ ██╗      █████╗ ██████╗ ███╗   ███╗██╗███╗   ██╗██╗███████╗████████╗██████╗  █████╗ ████████╗██╗ ██████╗ ███╗   ██╗         ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔══██╗██╔══██╗████╗ ████║██║████╗  ██║██║██╔════╝╚══██╔══╝██╔══██╗██╔══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗███████║██║  ██║██╔████╔██║██║██╔██╗ ██║██║███████╗   ██║   ██████╔╝███████║   ██║   ██║██║   ██║██╔██╗ ██║█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██╔══██║██║  ██║██║╚██╔╝██║██║██║╚██╗██║██║╚════██║   ██║   ██╔══██╗██╔══██║   ██║   ██║██║   ██║██║╚██╗██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ██║  ██║██████╔╝██║ ╚═╝ ██║██║██║ ╚████║██║███████║   ██║   ██║  ██║██║  ██║   ██║   ██║╚██████╔╝██║ ╚████║     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝         ╚═╝  ╚═╝╚═════╝ ╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝╚═╝╚══════╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝     ╚═╝ ╚═╝ ╚═╝    
        //                                                                                                                                                     
        // GET: Administration

        /// User access control
        [AccessControlFilter(PermissionName = "Admin Permissions")]
        public IActionResult Administration([FromQuery(Name = "p")] string p = "1", [FromQuery(Name = "s")] string s = "500")
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string AccountID = functions.GetSessionAccountData(_sessionManager.SessionKey, "AccountID");

            ViewBag.PageNo = 1;
            ViewBag.PageSize = 500;
            if (!string.IsNullOrEmpty(p) && !string.IsNullOrEmpty(s))
            {
                ViewBag.PageNo = Int32.Parse(p);
                ViewBag.PageSize = Int32.Parse(s);
            }
            ViewBag.PageSkip = (ViewBag.PageNo - 1) * ViewBag.PageSize;
            int PageSkip = ViewBag.PageSkip;
            int PageSize = ViewBag.PageSize;
            ViewBag.TotalRecords = _context.ActivityLogs.Count();

            ViewBag.ActivityLogsData = _context.ActivityLogs.OrderByDescending(s => s.ID).Skip(PageSkip).Take(PageSize);
            ViewBag.ActivityLogsCount = _context.ActivityLogs.Count();

            //TODO set subcribers page if more than 500
            ViewBag.SubscribersData = _context.Subscribers.OrderByDescending(s => s.ID).Take(500);
            ViewBag.SubscribersCount = _context.Subscribers.Count();

            ViewBag.AccountsData = _context.Accounts.OrderByDescending(s => s.CreatedAt);
            ViewBag.TotalAccounts = _context.Accounts.Count();

            return View();
        }



        /// <summary>
        /// Edits account roles
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAccountRoles()
        {
            //get input values
            string AccountID = HttpContext.Request.Form["AccountID"];
            string AuthorRole = HttpContext.Request.Form["AuthorRole"];
            string EditorRole = HttpContext.Request.Form["EditorRole"];
            string AdminRole = HttpContext.Request.Form["AdminRole"];

            try
            {
                //Update admin permissions
                functions.UpdatePermission("Author Permissions", AuthorRole, AccountID);

                //Update editor permissions
                functions.UpdatePermission("Editor Permissions", EditorRole, AccountID);

                //Update author permissions
                functions.UpdatePermission("Admin Permissions", AdminRole, AccountID);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Administration", "Admin", new { tab = "users" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Account Role Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Administration", "Admin", new { tab = "users" });
            }
        }


        /// <summary>
        /// Edit account view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Admin Permissions")]
        public async Task<IActionResult> EditAccount(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            //Get countries list
            ViewBag.CountriesList = functions.GetCountryList();

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var editModel = await _context.Accounts
               .FirstOrDefaultAsync(m => m.AccountID == id);
            if (editModel == null)
            {
                return NotFound();
            }
            return View(editModel);
        }



        /// <summary>
        /// User details view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Admin Permissions")]
        public async Task<IActionResult> UserDetails(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            //Get countries list
            ViewBag.CountriesList = functions.GetCountryList();

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var editModel = await _context.Accounts
               .FirstOrDefaultAsync(m => m.AccountID == id);
            if (editModel == null)
            {
                return NotFound();
            }
            return View(editModel);
        }


        /// <summary>
        /// updates account information
        /// </summary>
        /// <param name="accountModel"></param>
        /// <param name="ProfileImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(AccountModel accountModel, List<IFormFile> ProfileImage)
        {
            string AccountID = HttpContext.Request.Form["AccountID"];

            try
            {
                string UploadPicture = null;
                //check if image is posted
                if (Request.Form.Files.Count > 0)
                {
                    var DirectoryName = functions.GetSessionAccountData(_sessionManager.SessionKey, "DirectoryName");
                    var SavePath = @"wwwroot\\admin\\images\\accounts\\" + DirectoryName + "\\";
                    UploadPicture = functions.UploadImage(ProfileImage, 1, 150, 150, null, SavePath);
                    accountModel.ProfilePicture = UploadPicture;
                    //TODO reset profile image session link
                    //_sessionManager.LoginProfilePicture = AppSettings.BaseUrl() + "/admin/images/accounts/" + functions.GetSessionAccountData(_sessionManager.SessionKey, "DirectoryName") + "/" + UploadPicture;
                }
                else
                {
                    accountModel.ProfilePicture = functions.GetAccountData(AccountID, "ProfilePicture");
                }

                if (Request.Form.Any())
                {
                    //get account primary key
                    accountModel.ID = functions.Int32Parse(functions.GetAccountData(AccountID, "ID"), 0);

                    //set defaults
                    accountModel.AccountID = AccountID;
                    accountModel.Password = functions.GetAccountData(AccountID, "Password");
                    accountModel.DirectoryName = functions.GetAccountData(AccountID, "DirectoryName");
                    accountModel.Active = functions.BooleanParse(HttpContext.Request.Form["Active"]);

                    //set account data
                    accountModel.FirstName = HttpContext.Request.Form["FirstName"];
                    accountModel.LastName = HttpContext.Request.Form["LastName"];
                    accountModel.Description = HttpContext.Request.Form["Description"];
                    accountModel.Gender = HttpContext.Request.Form["Gender"];
                    accountModel.PhoneNumber = HttpContext.Request.Form["PhoneNumber"];
                    accountModel.DateOfBirth = HttpContext.Request.Form["DateOfBirth"];
                    accountModel.Address = HttpContext.Request.Form["Address"];
                    accountModel.Country = HttpContext.Request.Form["Country"];
                    accountModel.UpdatedAt = DateTime.Now;
                    accountModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("Accounts", "AccountID", accountModel.AccountID, "CreatedAt"));

                    _context.Update(accountModel);
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Administration", "Admin", new { tab = "users" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Admin Update Account Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("EditAccount", "Admin", new { id = AccountID });
            }
        }


        /// <summary>
        /// Email subscribers view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Admin Permissions")]
        public IActionResult EmailSubscribers([FromQuery(Name = "email")] string email)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            ViewBag.SubscriberEmail = email;

            return View();
        }



        /// <summary>
        /// Sends email to subscribers
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EmailSubscribers(SubscriberModel subscriberModel)
        {
            //get input values
            string Email = HttpContext.Request.Form["Email"];
            string Subject = HttpContext.Request.Form["Subject"];
            string Message = HttpContext.Request.Form["Message"];

            try
            {
                //validate required inputs
                string[] ValidationInputs = { Subject, Message };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                    return RedirectToAction("Administration", "Admin", new { tab = "subscriptions" });
                }

                //if single
                if (!string.IsNullOrEmpty(Email))
                {
                    //TODO send email
                }
                else
                {
                    var Subscribers = _context.Subscribers.OrderBy(s => s.ID);
                    foreach (var item in Subscribers)
                    {
                        //TODO send email
                    }
                }

                TempData["SuccessMessage"] = "Email(s) sent successfully";
                return RedirectToAction("Administration", "Admin", new { tab = "subscriptions" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Emailing Subscribers Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Administration", "Admin", new { tab = "subscriptions" });
            }
        }


        //      ██╗ ██╗ ██╗      ██████╗ ██████╗ ███╗   ██╗███████╗██╗ ██████╗ ██╗   ██╗██████╗  █████╗ ████████╗██╗ ██████╗ ███╗   ██╗         ██╗ ██╗ ██╗
        //     ██╔╝██╔╝██╔╝     ██╔════╝██╔═══██╗████╗  ██║██╔════╝██║██╔════╝ ██║   ██║██╔══██╗██╔══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║        ██╔╝██╔╝██╔╝
        //    ██╔╝██╔╝██╔╝█████╗██║     ██║   ██║██╔██╗ ██║█████╗  ██║██║  ███╗██║   ██║██████╔╝███████║   ██║   ██║██║   ██║██╔██╗ ██║█████╗ ██╔╝██╔╝██╔╝ 
        //   ██╔╝██╔╝██╔╝ ╚════╝██║     ██║   ██║██║╚██╗██║██╔══╝  ██║██║   ██║██║   ██║██╔══██╗██╔══██║   ██║   ██║██║   ██║██║╚██╗██║╚════╝██╔╝██╔╝██╔╝  
        //  ██╔╝██╔╝██╔╝        ╚██████╗╚██████╔╝██║ ╚████║██║     ██║╚██████╔╝╚██████╔╝██║  ██║██║  ██║   ██║   ██║╚██████╔╝██║ ╚████║     ██╔╝██╔╝██╔╝   
        //  ╚═╝ ╚═╝ ╚═╝          ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝     ╚═╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝     ╚═╝ ╚═╝ ╚═╝    
        //                                                                                                                                                 
        // GET: Configuration
        /// User access control
        [AccessControlFilter(PermissionName = "Admin Permissions")]
        public IActionResult Configuration()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            ViewBag.LanguagesDataCount = _context.SiteLanguages.Count();

            ViewBag.LanguagesData = _context.SiteLanguages.OrderByDescending(s => s.ID);

            return View();
        }



        /// <summary>
        /// Updates Google Settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateGoogleSettings()
        {
            //get input values
            string GoogleAnalyticsKey = HttpContext.Request.Form["GoogleAnalyticsKey"];
            string GoogleAdvertKey = HttpContext.Request.Form["GoogleAdvertKey"];

            try
            {
                //Update settings
                functions.UpdateTableData("Configurations", "ConfigurationKey", "GoogleAnalyticsKey", "ConfigurationValue", GoogleAnalyticsKey);
                functions.UpdateTableData("Configurations", "ConfigurationKey", "GoogleAdvertKey", "ConfigurationValue", GoogleAdvertKey);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Configuration", "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Google Settings Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Configuration", "Admin");
            }
        }



        /// <summary>
        /// Updates Facebook Settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateFacebookSettings()
        {
            //get input values
            string ShowFacebookComments = HttpContext.Request.Form["ShowFacebookComments"];
            string FacebookCommentId = HttpContext.Request.Form["FacebookCommentId"];

            try
            {
                //Update settings
                functions.UpdateTableData("Configurations", "ConfigurationKey", "ShowFacebookComments", "ConfigurationValue", ShowFacebookComments);
                functions.UpdateTableData("Configurations", "ConfigurationKey", "FacebookCommentId", "ConfigurationValue", FacebookCommentId);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "facebook" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Facebook Settings Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "facebook" });
            }
        }



        /// <summary>
        /// Updates Twitter Settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTwitterSettings()
        {
            //get input values
            string ShowTwitterFeeds = HttpContext.Request.Form["ShowTwitterFeeds"];
            string TwitterFeed = HttpContext.Request.Form["TwitterFeed"];

            try
            {
                //Update settings
                functions.UpdateTableData("Configurations", "ConfigurationKey", "ShowTwitterFeeds", "ConfigurationValue", ((!string.IsNullOrEmpty(ShowTwitterFeeds)) ? ShowTwitterFeeds : "0"));
                functions.UpdateConfigValue("TwitterFeed", TwitterFeed);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "twitter" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Twitter Settings Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "twitter" });
            }
        }


        /// <summary>
        /// Updates Email Config Settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateEmailConfigSettings()
        {
            //get input values
            string SmtpEmail = HttpContext.Request.Form["SmtpEmail"];
            string SmtpPassword = HttpContext.Request.Form["SmtpPassword"];
            string SmtpPort = HttpContext.Request.Form["SmtpPort"];
            string SmtpHost = HttpContext.Request.Form["SmtpHost"];
            string SenderEmail = HttpContext.Request.Form["SenderEmail"];
            string EmailDisplayName = HttpContext.Request.Form["EmailDisplayName"];
            string EmailClosureText = HttpContext.Request.Form["EmailClosureText"];
            string UseSendGrid = HttpContext.Request.Form["UseSendGrid"];
            string SendGridKey = HttpContext.Request.Form["SendGridKey"];

            try
            {
                //Update settings
                functions.UpdateTableData("Configurations", "ConfigurationKey", "SmtpEmail", "ConfigurationValue", SmtpEmail);
                functions.UpdateTableData("Configurations", "ConfigurationKey", "SmtpPassword", "ConfigurationValue", SmtpPassword);
                functions.UpdateTableData("Configurations", "ConfigurationKey", "SmtpPort", "ConfigurationValue", SmtpPort);
                functions.UpdateTableData("Configurations", "ConfigurationKey", "SmtpHost", "ConfigurationValue", SmtpHost);
                functions.UpdateTableData("Configurations", "ConfigurationKey", "SenderEmail", "ConfigurationValue", SenderEmail);
                functions.UpdateTableData("Configurations", "ConfigurationKey", "EmailDisplayName", "ConfigurationValue", EmailDisplayName);
                functions.UpdateTableData("Configurations", "ConfigurationKey", "EmailClosureText", "ConfigurationValue", EmailClosureText);
                functions.UpdateTableData("Configurations", "ConfigurationKey", "SendGridKey", "ConfigurationValue", SendGridKey);
                //if UseSendGrid, update EmailerType
                if (!string.IsNullOrEmpty(UseSendGrid) && UseSendGrid == "1")
                {
                    functions.UpdateTableData("Configurations", "ConfigurationKey", "EmailerType", "ConfigurationValue", "SendGrid");
                }

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "email-config" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Email Config Settings Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "email-config" });
            }
        }



        /// <summary>
        /// Updates Cookie Concent Settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCookieConcentSettings()
        {
            //get input values
            string ShowCookieConcent = HttpContext.Request.Form["ShowCookieConcent"];
            string CookieConcent = HttpContext.Request.Form["CookieConcent"];

            try
            {
                //Update settings
                functions.UpdateTableData("Configurations", "ConfigurationKey", "ShowCookieConcent", "ConfigurationValue", ((!string.IsNullOrEmpty(ShowCookieConcent)) ? ShowCookieConcent : "0"));
                functions.UpdateConfigValue("CookieConcent", CookieConcent);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "concent" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Cookie Concent Settings Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "concent" });
            }
        }



        /// <summary>
        /// Updates Java Scripts
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateJavaScripts()
        {
            //get input values
            string EnableJavaScripts = HttpContext.Request.Form["EnableJavaScripts"];
            string HeaderJavaScripts = HttpContext.Request.Form["HeaderJavaScripts"];
            string FooterJavaScripts = HttpContext.Request.Form["FooterJavaScripts"];

            try
            {
                //Update settings
                functions.UpdateTableData("Configurations", "ConfigurationKey", "EnableJavaScripts", "ConfigurationValue", ((!string.IsNullOrEmpty(EnableJavaScripts)) ? EnableJavaScripts : "0"));
                functions.UpdateConfigValue("HeaderJavaScripts", HeaderJavaScripts);
                functions.UpdateConfigValue("FooterJavaScripts", FooterJavaScripts);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "site-scripts" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Cookie Concent Settings Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "site-scripts" });
            }
        }




        /// <summary>
        /// Updates Sharethis keys
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateShareSettings()
        {
            //get input values
            string ShowShareThis = HttpContext.Request.Form["ShowShareThis"];
            string ShareThisUrl = HttpContext.Request.Form["ShareThisUrl"];
            try
            {
                //Update settings
                functions.UpdateTableData("Configurations", "ConfigurationKey", "ShowShareThis", "ConfigurationValue", ((!string.IsNullOrEmpty(ShowShareThis)) ? ShowShareThis : "0"));
                functions.UpdateTableData("Configurations", "ConfigurationKey", "ShareThisUrl", "ConfigurationValue", ShareThisUrl);

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "share" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Update Twitter Settings Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "share" });
            }
        }



        /// <summary>
        /// Adds new site language
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLanguage()
        {
            //get input values
            string Language = HttpContext.Request.Form["LanguageName"];
            //validate required inputs
            string[] ValidationInputs = { Language };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return RedirectToAction("Configuration", "Admin", new { tab = "languages" });
            }

            try
            {
                SiteLanguagesModel siteLangModel = new SiteLanguagesModel
                {
                    LanguageID = functions.GetGuid(),
                    Language = Language,
                    CreatedAt = DateTime.Now
                };
                _context.SiteLanguages.Add(siteLangModel);
                _context.SaveChanges();

                TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "languages" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Add Language Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("Configuration", "Admin", new { tab = "languages" });
            }
        }




        //  ██████╗  █████╗  ██████╗ ███████╗    ███████╗███████╗ ██████╗████████╗██╗ ██████╗ ███╗   ██╗███████╗
        //  ██╔══██╗██╔══██╗██╔════╝ ██╔════╝    ██╔════╝██╔════╝██╔════╝╚══██╔══╝██║██╔═══██╗████╗  ██║██╔════╝
        //  ██████╔╝███████║██║  ███╗█████╗      ███████╗█████╗  ██║        ██║   ██║██║   ██║██╔██╗ ██║███████╗
        //  ██╔═══╝ ██╔══██║██║   ██║██╔══╝      ╚════██║██╔══╝  ██║        ██║   ██║██║   ██║██║╚██╗██║╚════██║
        //  ██║     ██║  ██║╚██████╔╝███████╗    ███████║███████╗╚██████╗   ██║   ██║╚██████╔╝██║ ╚████║███████║
        //  ╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚══════╝    ╚══════╝╚══════╝ ╚═════╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝
        //  
        /// <summary>
        /// Creates page section data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPageSection(PageSectionsModel pageSectionsModel)
        {
            try
            {
                //get input values
                string SectionTitle = HttpContext.Request.Form["SectionTitle"];
                string SectionContent = HttpContext.Request.Form["SectionContent"];
                string SectionType = HttpContext.Request.Form["SectionType"];
                string SectionOrder = HttpContext.Request.Form["SectionOrder"];

                //validate required inputs
                string[] ValidationInputs = { SectionTitle, SectionContent };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                    return RedirectToAction("ContentManagement", "Admin", new { tab = "page-sections" });
                }

                pageSectionsModel.SectionID = functions.GetGuid();
                pageSectionsModel.SectionName = SectionTitle.Trim(' ');
                pageSectionsModel.SectionTitle = SectionTitle;
                pageSectionsModel.SectionContent = SectionContent;
                pageSectionsModel.SectionType = (!string.IsNullOrEmpty(SectionType)) ? SectionType : "Default";
                pageSectionsModel.SectionOrder = (!string.IsNullOrEmpty(SectionOrder)) ? functions.Int32Parse(SectionOrder) : 10;
                pageSectionsModel.UpdatedAt = DateTime.Now;
                pageSectionsModel.CreatedAt = DateTime.Now;

                _context.Add(pageSectionsModel);
                await _context.SaveChangesAsync();

                //reset page order TODO Not working
                //functions.PageSectionOrderReset(pageSectionsModel.SectionID, pageSectionsModel.SectionOrder);
                if (_context.PageSections.Any(s => s.SectionID != pageSectionsModel.SectionID && s.SectionOrder == pageSectionsModel.SectionOrder))
                {
                    var DBQuery = _context.PageSections.Where(s => s.SectionID != pageSectionsModel.SectionID && s.SectionOrder >= pageSectionsModel.SectionOrder);
                    foreach (var item in DBQuery)
                    {
                        //update order
                        string new_order = (item.SectionOrder + 1).ToString();
                        functions.UpdateTableData("PageSections", "SectionID", item.SectionID, "SectionOrder", new_order);
                    }
                }

                TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "page-sections" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Add Page Section Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "page-sections" });
            }
        }


        /// <summary>
        /// Edit page section view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Editor Permissions")]
        public async Task<IActionResult> EditPageSection(string id)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var pageSectionModel = await _context.PageSections
               .FirstOrDefaultAsync(m => m.SectionID == id);
            if (pageSectionModel == null)
            {
                return NotFound();
            }
            return View(pageSectionModel);
        }


        /// <summary>
        /// Edits Page Section
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePageSection()
        {
            //get input values
            string SectionID = HttpContext.Request.Form["SectionID"];
            string SectionTitle = HttpContext.Request.Form["SectionTitle"];
            string SectionContent = HttpContext.Request.Form["SectionContent"];
            string SectionOrder = HttpContext.Request.Form["SectionOrder"];
            try
            {
                //validate required inputs
                string[] ValidationInputs = { SectionTitle };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";

                    return RedirectToAction("ContentManagement", "Admin", new { tab = "page-sections" });
                }

                PageSectionsModel pageSectionsModel = _context.PageSections.Single(u => u.SectionID == SectionID);
                pageSectionsModel.SectionTitle = SectionTitle;
                pageSectionsModel.SectionContent = SectionContent;
                pageSectionsModel.SectionOrder = functions.Int32Parse(SectionOrder, 10); //set default order as 10
                pageSectionsModel.UpdatedAt = DateTime.Now;
                pageSectionsModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("PageSections", "SectionID", pageSectionsModel.SectionID, "CreatedAt"));

                _context.PageSections.Update(pageSectionsModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.createSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "page-sections" }); 
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Page Section Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "page-sections" });
            }
        }




        //  ██████╗ ███████╗███████╗ ██████╗ ██╗   ██╗██████╗  ██████╗███████╗███████╗
        //  ██╔══██╗██╔════╝██╔════╝██╔═══██╗██║   ██║██╔══██╗██╔════╝██╔════╝██╔════╝
        //  ██████╔╝█████╗  ███████╗██║   ██║██║   ██║██████╔╝██║     █████╗  ███████╗
        //  ██╔══██╗██╔══╝  ╚════██║██║   ██║██║   ██║██╔══██╗██║     ██╔══╝  ╚════██║
        //  ██║  ██║███████╗███████║╚██████╔╝╚██████╔╝██║  ██║╚██████╗███████╗███████║
        //  ╚═╝  ╚═╝╚══════╝╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝ ╚═════╝╚══════╝╚══════╝
        //                                                                            

        /// <summary>
        /// Creates New Resource Document
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewResourceDocument(DocumentResourcesModel docResourcesModel, List<IFormFile> DocumentFile)
        {
            //get input values
            string PageID = HttpContext.Request.Form["PageID"];
            string DocumentName = HttpContext.Request.Form["DocumentName"];
            string ShortDescription = HttpContext.Request.Form["ShortDescription"];
            string Status = HttpContext.Request.Form["Status"];
            try
            {
                //validate required inputs
                string[] ValidationInputs = { DocumentName };
                if (!functions.ValidateInputs(ValidationInputs) || DocumentFile.Count == 0)
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";

                    return RedirectToAction("ContentManagement", "Admin", new { tab = "resources" });
                }

                
                var DirectoryName = DateTime.Now.ToString("MM-yyyy");
                var SavePath = @"wwwroot\\resources\\documents\\" + DirectoryName + "\\";
                docResourcesModel.FileName = functions.UploadSingleDocument(DocumentFile, SavePath);
                docResourcesModel.FileExtension = docResourcesModel.FileName.ToString().Split(".")[1];

                docResourcesModel.DocumentID = functions.GetGuid();
                docResourcesModel.PageID = PageID;
                docResourcesModel.DocumentName = DocumentName;
                docResourcesModel.ShortDescription = ShortDescription;
                docResourcesModel.Status = functions.Int32Parse(Status, 0);
                docResourcesModel.UpdatedAt = DateTime.Now;
                docResourcesModel.CreatedAt = DateTime.Now;

                _context.Add(docResourcesModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "resources" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Create Document Resource Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "resources" });
            }
        }


        /// <summary>
        /// Edits Resource Document
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResourceDocument()
        {
            //get input values
            string DocumentID = HttpContext.Request.Form["DocumentID"]; 
            string DocumentName = HttpContext.Request.Form["DocumentName"];
            string ShortDescription = HttpContext.Request.Form["ShortDescription"];
            string Status = HttpContext.Request.Form["DocumentStatus"];

            try
            {
                //validate required inputs
                string[] ValidationInputs = { DocumentID, DocumentName };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";

                    return RedirectToAction("ContentManagement", "Admin", new { tab = "videos" });
                }

                DocumentResourcesModel documentResourcesModel = _context.DocumentResources.Single(u => u.DocumentID == DocumentID);
                documentResourcesModel.DocumentName = DocumentName;
                documentResourcesModel.ShortDescription = ShortDescription;
                documentResourcesModel.Status = functions.Int32Parse(Status, 0); //set default status as 0
                documentResourcesModel.UpdatedAt = DateTime.Now;
                documentResourcesModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("DocumentResources", "DocumentID", documentResourcesModel.DocumentID, "CreatedAt"));

                _context.DocumentResources.Update(documentResourcesModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.createSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "videos" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Resource Document Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "videos" });
            }
        }



        //  ██╗   ██╗██╗██████╗ ███████╗ ██████╗ ███████╗
        //  ██║   ██║██║██╔══██╗██╔════╝██╔═══██╗██╔════╝
        //  ██║   ██║██║██║  ██║█████╗  ██║   ██║███████╗
        //  ╚██╗ ██╔╝██║██║  ██║██╔══╝  ██║   ██║╚════██║
        //   ╚████╔╝ ██║██████╔╝███████╗╚██████╔╝███████║
        //    ╚═══╝  ╚═╝╚═════╝ ╚══════╝ ╚═════╝ ╚══════╝
        // 
        /// <summary>
        /// Creates New Video
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewVideo(VideosModel videosModel)
        {
            //get input values
            string VideoTitle = HttpContext.Request.Form["VideoTitle"];
            string VideoLink = HttpContext.Request.Form["VideoLink"];
            string Status = HttpContext.Request.Form["Status"];
            try
            {
                //validate required inputs
                string[] ValidationInputs = { VideoTitle, VideoLink };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";

                    return RedirectToAction("ContentManagement", "Admin", new { tab = "videos" });
                }

                videosModel.VideoID = functions.GetGuid();
                videosModel.VideoTitle = VideoTitle;
                videosModel.VideoLink = VideoLink;
                videosModel.Status = functions.Int32Parse(Status, 0);
                videosModel.UpdatedAt = DateTime.Now;
                videosModel.CreatedAt = DateTime.Now;

                _context.Add(videosModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "videos" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Create Video Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "videos" });
            }
        }



        /// <summary>
        /// Edits Video
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVideo()
        {
            //get input values
            string VideoID = HttpContext.Request.Form["VideoID"];
            string VideoTitle = HttpContext.Request.Form["VideoTitle"];
            string VideoLink = HttpContext.Request.Form["VideoLink"];
            string Status = HttpContext.Request.Form["VideoStatus"];

            try
            {
                //validate required inputs
                string[] ValidationInputs = { VideoTitle, VideoLink };
                if (!functions.ValidateInputs(ValidationInputs))
                {
                    TempData["ErrorMessage"] = "Validation error. Missing required field(s).";

                    return RedirectToAction("ContentManagement", "Admin", new { tab = "videos" });
                }

                VideosModel videosModel = _context.Videos.Single(u => u.VideoID == VideoID);
                videosModel.VideoTitle = VideoTitle;
                videosModel.VideoLink = VideoLink;
                videosModel.Status = functions.Int32Parse(Status, 0); //set default status as 0
                videosModel.UpdatedAt = DateTime.Now;
                videosModel.CreatedAt = Convert.ToDateTime(functions.GetTableData("Videos", "VideoID", videosModel.VideoID, "CreatedAt"));

                _context.Videos.Update(videosModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.createSuccessMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "videos" });
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Edit Video Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                return RedirectToAction("ContentManagement", "Admin", new { tab = "videos" });
            }
        }




        //   ██████╗ ██████╗ ███╗   ██╗████████╗██████╗  █████╗  ██████╗████████╗███████╗
        //  ██╔════╝██╔═══██╗████╗  ██║╚══██╔══╝██╔══██╗██╔══██╗██╔════╝╚══██╔══╝██╔════╝
        //  ██║     ██║   ██║██╔██╗ ██║   ██║   ██████╔╝███████║██║        ██║   ███████╗
        //  ██║     ██║   ██║██║╚██╗██║   ██║   ██╔══██╗██╔══██║██║        ██║   ╚════██║
        //  ╚██████╗╚██████╔╝██║ ╚████║   ██║   ██║  ██║██║  ██║╚██████╗   ██║   ███████║
        //   ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝   ╚═╝   ╚══════╝
        //          

        /// <summary>
        /// NewContract view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Admin Permissions")]
        public IActionResult NewContract()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            return View();
        }





        //      ██╗ ██╗     ██╗  ██╗███████╗██╗     ██████╗          ██╗ ██╗
        //     ██╔╝██╔╝     ██║  ██║██╔════╝██║     ██╔══██╗        ██╔╝██╔╝
        //    ██╔╝██╔╝█████╗███████║█████╗  ██║     ██████╔╝█████╗ ██╔╝██╔╝ 
        //   ██╔╝██╔╝ ╚════╝██╔══██║██╔══╝  ██║     ██╔═══╝ ╚════╝██╔╝██╔╝  
        //  ██╔╝██╔╝        ██║  ██║███████╗███████╗██║          ██╔╝██╔╝   
        //  ╚═╝ ╚═╝         ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝          ╚═╝ ╚═╝    
        //                                                                  
        /// <summary>
        /// Help center view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Author Permissions")]
        public IActionResult Help()
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            return View();
        }




        //      ██╗ ██╗     ███████╗██╗  ██╗ █████╗ ██████╗ ███████╗██████╗     ███████╗██╗   ██╗███╗   ██╗ ██████╗████████╗██╗ ██████╗ ███╗   ██╗███████╗         ██╗ ██╗
        //     ██╔╝██╔╝     ██╔════╝██║  ██║██╔══██╗██╔══██╗██╔════╝██╔══██╗    ██╔════╝██║   ██║████╗  ██║██╔════╝╚══██╔══╝██║██╔═══██╗████╗  ██║██╔════╝        ██╔╝██╔╝
        //    ██╔╝██╔╝█████╗███████╗███████║███████║██████╔╝█████╗  ██║  ██║    █████╗  ██║   ██║██╔██╗ ██║██║        ██║   ██║██║   ██║██╔██╗ ██║███████╗█████╗ ██╔╝██╔╝ 
        //   ██╔╝██╔╝ ╚════╝╚════██║██╔══██║██╔══██║██╔══██╗██╔══╝  ██║  ██║    ██╔══╝  ██║   ██║██║╚██╗██║██║        ██║   ██║██║   ██║██║╚██╗██║╚════██║╚════╝██╔╝██╔╝  
        //  ██╔╝██╔╝        ███████║██║  ██║██║  ██║██║  ██║███████╗██████╔╝    ██║     ╚██████╔╝██║ ╚████║╚██████╗   ██║   ██║╚██████╔╝██║ ╚████║███████║     ██╔╝██╔╝   
        //  ╚═╝ ╚═╝         ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚═════╝     ╚═╝      ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝     ╚═╝ ╚═╝    
        //         

        /// <summary>
        /// Delete specifed table record
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRecord()
        {
            //get delete post variables
            string DeleteKeyValue = HttpContext.Request.Form["DeleteKey"];
            string DeleteModel = HttpContext.Request.Form["DeleteModel"];
            string ReturnView = HttpContext.Request.Form["ReturnView"];
            string ReturnViewID = HttpContext.Request.Form["ReturnViewID"];

            string[] ValidationInputs = { DeleteKeyValue, DeleteModel, ReturnView };
            if (!functions.ValidateInputs(ValidationInputs))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                ReturnView = (!string.IsNullOrEmpty(ReturnView)) ? ReturnView : "Index";
                if (!string.IsNullOrEmpty(ReturnViewID))
                {
                    return RedirectToAction(ReturnView, "Admin", new { id = ReturnViewID });
                }
                return RedirectToAction(ReturnView, "Admin");
            }

            try
            {
                //get model primary key name
                string DeleteKey = AppSettings.ModelKey(DeleteModel);

                //Delete record
                functions.DeleteTableData(DeleteModel, DeleteKey, DeleteKeyValue);

                //if delete account
                if(DeleteModel == "Accounts")
                {
                    //Delete other account details
                    functions.DeleteTableData("AccountToPermission", "AccountID", DeleteKeyValue);
                }


                //Delete possible translation data
                functions.DeleteTableData("DataTranslations", "DocumentID", DeleteKeyValue);

                TempData["SuccessMessage"] = functions.GetAppMessage("DeleteSuccessMessage", _appMessages.deleteSuccessMessage);
                if (!string.IsNullOrEmpty(ReturnViewID))
                {
                    return RedirectToAction(ReturnView, "Admin", new { id = ReturnViewID });
                }
                return RedirectToAction(ReturnView, "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Delete "+ DeleteModel +" Error: "+ ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
            }

            if (!string.IsNullOrEmpty(ReturnViewID))
            {
                return RedirectToAction(ReturnView, "Admin", new { id = ReturnViewID });
            }
            return RedirectToAction(ReturnView, "Admin");
        }



        /// <summary>
        /// Adds data translation view
        /// </summary>
        /// <returns></returns>
        /// User access control
        [AccessControlFilter(PermissionName = "Author Permissions")]
        public IActionResult AddDataTranslation([FromQuery(Name = "key")] string doc_id, [FromQuery(Name = "key-name")] string key_name, [FromQuery(Name = "model")] string doc_model, 
            [FromQuery(Name = "title")] string title_name, [FromQuery(Name = "desc")] string description_name, [FromQuery(Name = "cont")] string content_name, 
            [FromQuery(Name = "view")] string return_view)
        {
            //Set Meta and Site Info Data
            ViewData["Organization"] = functions.GetCmsData("OrganizationName", _systemConfiguration.organizationName);

            string[] ValidationInputs = { doc_id, key_name, doc_model };
            if (!functions.ValidateInputs(ValidationInputs) || (string.IsNullOrEmpty(title_name) && string.IsNullOrEmpty(description_name) && string.IsNullOrEmpty(content_name)))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                return_view = (!string.IsNullOrEmpty(return_view)) ? return_view : "Index";
                if (!string.IsNullOrEmpty(return_view))
                {
                    return RedirectToAction(return_view, "Admin");
                }
                return RedirectToAction(return_view, "Admin");
            }

            //document model keys
            ViewBag.DocumentID = doc_id;
            ViewBag.DocumentKeyName = key_name;
            ViewBag.DocumentModel = doc_model;
            ViewBag.ReturnView = return_view;

            ViewBag.ModelTitleName = title_name;
            ViewBag.ModelDescriptionName = description_name;
            ViewBag.ModelContentName = content_name;

            ViewBag.InputType = AppSettings.GetTranslationContentType(doc_model);

            ViewBag.TitleDispalyClass = AppSettings.GetTranslationInputClass(doc_model, "Title");
            ViewBag.TitleInputValidation = (string.IsNullOrEmpty(ViewBag.TitleDispalyClass)) ? "required" : "";
            ViewBag.TitleLabelName = AppSettings.GetTranslationLabelName(doc_model, "Title");

            ViewBag.DescriptionDispalyClass = AppSettings.GetTranslationInputClass(doc_model, "Description");
            ViewBag.DescriptionInputValidation = (string.IsNullOrEmpty(ViewBag.DescriptionDispalyClass)) ? "required" : "";
            ViewBag.DescriptionLabelName = AppSettings.GetTranslationLabelName(doc_model, "Description");

            ViewBag.ContentDispalyClass = AppSettings.GetTranslationInputClass(doc_model, "Content");
            ViewBag.ContentInputValidation = (string.IsNullOrEmpty(ViewBag.ContentDispalyClass)) ? "required" : "";
            ViewBag.ContentLabelName = AppSettings.GetTranslationLabelName(doc_model, "Content");

            return View();
        }


        /// <summary>
        /// Adds/Updates translation of site data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDataTranslation()
        {
            //get delete post variables
            string DocumentID = HttpContext.Request.Form["DocumentID"];
            string DocumentModel = HttpContext.Request.Form["DocumentModel"];
            string Language = HttpContext.Request.Form["Language"];
            string ReturnView = HttpContext.Request.Form["ReturnView"];
            string TranslationTitle = HttpContext.Request.Form["TranslationTitle"];
            string TranslationDescription = HttpContext.Request.Form["TranslationDescription"];
            string TranslationContent = HttpContext.Request.Form["TranslationContent"];
            ReturnView = (!string.IsNullOrEmpty(ReturnView)) ? ReturnView : "Index";

            string[] ValidationInputs = { DocumentID, DocumentModel };
            if (!functions.ValidateInputs(ValidationInputs) || (string.IsNullOrEmpty(TranslationTitle) && string.IsNullOrEmpty(TranslationDescription) && string.IsNullOrEmpty(TranslationContent)))
            {
                TempData["ErrorMessage"] = "Validation error. Missing required field(s).";
                if (!string.IsNullOrEmpty(ReturnView))
                {
                    return RedirectToAction(ReturnView, "Admin");
                }
                return RedirectToAction(ReturnView, "Admin");
            }

            try
            {
                var DBQuery = _context.DataTranslations.Where(s => s.DocumentID == DocumentID && s.DocumentModel == DocumentModel && s.Language == Language);
                if (!DBQuery.Any())
                {
                    //add translation
                    DataTranslationsModel translationsModel = new DataTranslationsModel
                    {
                        DocumentID = DocumentID,
                        DocumentModel = DocumentModel,
                        Language = Language,
                        TranslationTitle = TranslationTitle,
                        TranslationDescription = TranslationDescription,
                        TranslationContent = TranslationContent,
                        UpdatedAt = DateTime.Now,
                        CreatedAt = DateTime.Now
                    };
                    _context.DataTranslations.Add(translationsModel);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = functions.GetAppMessage("CreateSuccessMessage", _appMessages.createSuccessMessage);
                }
                else
                {
                    //update translation
                    DataTranslationsModel translationsModel = _context.DataTranslations.Single(s => s.DocumentID == DocumentID && s.DocumentModel == DocumentModel && s.Language == Language);
                    translationsModel.TranslationTitle = TranslationTitle;
                    translationsModel.TranslationDescription = TranslationDescription;
                    translationsModel.TranslationContent = TranslationContent;
                    translationsModel.UpdatedAt = DateTime.Now;
                    _context.DataTranslations.Update(translationsModel);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = functions.GetAppMessage("EditSuccessMessage", _appMessages.editSuccessMessage);
                }


                if (!string.IsNullOrEmpty(ReturnView))
                {
                    return RedirectToAction(ReturnView, "Admin");
                }
                return RedirectToAction(ReturnView, "Admin");
            }
            catch (Exception ex)
            {
                //Log Error
                _logger.LogInformation("Add Data Translation Error: " + ex.ToString());
                TempData["ErrorMessage"] = functions.GetAppMessage("ExceptionMessage", _appMessages.exceptionMessage);
                if (!string.IsNullOrEmpty(ReturnView))
                {
                    return RedirectToAction(ReturnView, "Admin");
                }
                return RedirectToAction("Index", "Admin");
            }

        }


        //ovveride NotFound() to E404 error page
        public new IActionResult NotFound()
        {
            return RedirectToAction("E404", "Error");
        }
    }
}
