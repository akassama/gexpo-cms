using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoExpoApp.Models.Session
{
    public class SessionAuthorize : ActionFilterAttribute
    {
        //Injecting SessionManager in class
        private readonly SessionManager _sessionManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SystemConfiguration _systemConfiguration;

        public SessionAuthorize(SessionManager sessionManager, IHttpContextAccessor httpContextAccessor, IOptions<SystemConfiguration> systemConfiguration)
        {
            _sessionManager = sessionManager;
            this._httpContextAccessor = httpContextAccessor;
            _systemConfiguration = systemConfiguration.Value;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //get session and cookie keys
            var session_key = _sessionManager.SessionKey;
            var cookie_key = _httpContextAccessor.HttpContext.Request.Cookies[_systemConfiguration.authCookieKey];

            //if session key is null, set as cookie key 
            if (string.IsNullOrEmpty(session_key) && !string.IsNullOrEmpty(cookie_key))
            {
                //Set session key
                _sessionManager.SessionKey = cookie_key;
                session_key = cookie_key;
            }

            bool IsLoggedIn = false;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Accounts.Where(s => s.SessionKey == session_key);
                if (DBQuery.Any())
                {
                    IsLoggedIn = true;
                }
            }

            //check if logged in
            if (!IsLoggedIn)
            {

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                                { "Controller", "SignIn" },
                                { "Action", "Index" }
                                });
            }
        }
    }

    public class AccessControlFilter : ActionFilterAttribute
    {
        //Properties in Action Filter
        public string PermissionName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //get session key
            string session_key = filterContext.HttpContext.Session.GetString("_SessionKey");

            string AccountID = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Accounts.Where(s => s.SessionKey == session_key);
                if (DBQuery.Any())
                {
                    AccountID = DBQuery.FirstOrDefault().AccountID;
                }
            }

            if (string.IsNullOrEmpty(AccountID) || string.IsNullOrEmpty(PermissionName))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                                { "Controller", "Admin" },
                                { "Action", "Index" }
                                });
            }

            using (var db = new DBConnection())
            {
                AppFunctions functions = new AppFunctions();
                //Get permission booleans
                bool AuthorPermission = functions.UserHasPermission(AccountID, "Author Permissions");
                bool EditorPermission = functions.UserHasPermission(AccountID, "Editor Permissions");
                bool AdminPermission = functions.UserHasPermission(AccountID, "Admin Permissions");


                //get permission id from permission name
                int PermissionID = 0;
                if (db.Permissions.Any(s => s.PermissionName == PermissionName))
                {
                    PermissionID = db.Permissions.Where(s => s.PermissionName == PermissionName).FirstOrDefault().PermissionID;
                }

                //if author permission
                if (PermissionName == "Author Permissions")
                {
                    //check if not admin or editor 1st
                    if (!AdminPermission && !EditorPermission)
                    {
                        //check if user has permission
                        if (!db.AccountToPermissions.Any(s => s.AccountID == AccountID && s.PermissionID == PermissionID))
                        {

                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Admin" },
                                { "Action", "Index" }
                                        });
                        }
                    }
                }
                //if editor permission
                else if (PermissionName == "Editor Permissions")
                {
                    //check if not admin 1st
                    if (!AdminPermission)
                    {
                        //check if user has permission
                        if (!db.AccountToPermissions.Any(s => s.AccountID == AccountID && s.PermissionID == PermissionID))
                        {

                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Admin" },
                                { "Action", "Index" }
                                        });
                        }
                    }
                }
                //if author permission
                else if (PermissionName == "Admin Permissions")
                {
                    //check if not admin or editor
                    if (!AdminPermission)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Admin" },
                                { "Action", "Index" }
                                        });
                    }
                }
                else
                {
                    //Default: check if user has permission
                    if (!db.AccountToPermissions.Any(s => s.AccountID == AccountID && s.PermissionID == PermissionID))
                    {

                        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                { "Controller", "Admin" },
                                { "Action", "Index" }
                                    });
                    }
                }

            }

            base.OnActionExecuting(filterContext);
        }
    }

}
