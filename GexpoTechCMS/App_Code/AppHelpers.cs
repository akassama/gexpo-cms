using Microsoft.AspNetCore.Html;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NgoExpoApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NgoExpoApp.App_Code
{
    //Helpers for Profile
    public class ProfileHelpers
    {

        //Return account data
        /// <summary>
        /// Get specific account data. Takes account id and data to return
        /// </summary>
        /// <returns>account data</returns>
        public static string GetAccountData(string account_id, string return_data)
        {
            try
            {
                if (!string.IsNullOrEmpty(account_id))
                {
                    using (var db = new DBConnection())
                    {
                        switch (return_data)
                        {
                            case "ID":
                                return (db.Accounts.Any(s => s.AccountID == account_id)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().ID.ToString() : "0";
                            case "FirstName":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.FirstName != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().FirstName : "";
                            case "LastName":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.LastName != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().LastName : "";
                            case "FullName":
                                return db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().FirstName + " " + db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().LastName;
                            case "Email":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.Email != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().Email : "";
                            case "Description":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.Description != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().Description : "";
                            case "Gender":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.Gender != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().Gender : "";
                            case "Password":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.Password != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().Password : "";
                            case "ProfilePicture":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.ProfilePicture != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().ProfilePicture : "";
                            case "Active":
                                return (db.Accounts.Any(s => s.AccountID == account_id)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().Active.ToString() : "false";
                            case "Oauth":
                                return (db.Accounts.Any(s => s.AccountID == account_id)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().Oauth.ToString() : "false";
                            case "EmailVerification":
                                return (db.Accounts.Any(s => s.AccountID == account_id)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().EmailVerification.ToString() : "false";
                            case "EmailNotification":
                                return (db.Accounts.Any(s => s.AccountID == account_id)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().EmailNotification.ToString() : "false";
                            case "DirectoryName":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.DirectoryName != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().DirectoryName : "";
                            case "PhoneNumber":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.PhoneNumber != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().PhoneNumber : "";
                             case "Country":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.Country != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().Country : "";
                            case "Address":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.Address != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().Address : "";
                            case "UpdatedAt":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.UpdatedAt != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().UpdatedAt.ToString() : "";
                            case "CreatedAt":
                                return (db.Accounts.Any(s => s.AccountID == account_id && s.CreatedAt != null)) ? db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().CreatedAt.ToString() : "";
                            default:
                                return "NA";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return "NA";
        }


        //Return account profile picture
        /// <summary>
        /// Get profile picture
        /// </summary>
        /// <returns>account profile picture or default picture</returns>
        public static string GetAccountProfilePicture(string account_id)
        {
            string profile_picture = GetAccountData(account_id, "ProfilePicture");
            string base_url = AppSettings.BaseUrl();

            if (!string.IsNullOrEmpty(profile_picture))
            {
                string directory_name = GetAccountData(account_id, "DirectoryName");
                return base_url + "/admin/images/accounts/" + directory_name + "/" + profile_picture;
            }

            //return default
            return base_url + "/" + AppSettings.DefaultProfilePicture();
        }


        //Checks if a user has permission access
        public static bool UserHasPermission(string account_id, string permission_name)
        {
            if (string.IsNullOrEmpty(account_id) || string.IsNullOrEmpty(permission_name))
            {
                return false;
            }

            using (var db = new DBConnection())
            {
                //get permission id from permission name
                int PermissionID = 0;
                if (db.Permissions.Any(s => s.PermissionName == permission_name))
                {
                    PermissionID = db.Permissions.Where(s => s.PermissionName == permission_name).FirstOrDefault().PermissionID;
                }

                //check if user has permission
                if (db.AccountToPermissions.Any(s => s.AccountID == account_id && s.PermissionID == PermissionID))
                {
                    return true;
                }
            }
            return false;
        }

        public static HtmlString GetAccountRoles(string account_id)
        {
            string resut_data = "";

            if (ProfileHelpers.UserHasPermission(account_id, "Admin Permissions"))
            {
                resut_data += "<a href='#' class='badge bg-primary mr-1 my-1'>Admin</a>";
            }
            if (ProfileHelpers.UserHasPermission(account_id, "Editor Permissions"))
            {
                resut_data += "<a href='#' class='badge bg-primary mr-1 my-1'>Editor</a>";
            }
            if (ProfileHelpers.UserHasPermission(account_id, "Author Permissions"))
            {
                resut_data += "<a href='#' class= 'badge bg-primary mr-1 my-1' > Author</a>";
            }

            return new HtmlString(resut_data);
        }


        /// <summary>
        /// Checks for recent unread messages, returns total from last n message
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public static int GetTotalUnreadMessages(string account_id, int total_messages)
        {
            int total_unread = 0;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Contacts.OrderByDescending(s => s.ID).Take(total_messages);
                if (DBQuery.Any())
                {
                    foreach(var item in DBQuery)
                    {
                        if(!db.MessageViews.Any(x=> x.ContactID == item.ContactID && x.AccountID == account_id))
                        {
                            total_unread++;
                        }
                    }
                }
            }
            return total_messages;
        }


        /// <summary>
        /// Checks for recent unread messages, returns n messages list
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="total_messages"></param>
        /// <param name="return_total"></param>
        /// <returns></returns>
        public static HtmlString GetUnreadMessages(string account_id, int total_messages, int return_total)
        {
            string result_data = "";
            int total_count = 0;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Contacts.OrderByDescending(s => s.ID).Take(total_messages);
                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        if (!db.MessageViews.Any(x => x.ContactID == item.ContactID && x.AccountID == account_id))
                        {
                            //append if total not reached
                            if (total_count < return_total)
                            {
                                result_data += @$"<a href='/Admin/Contact/{item.ContactID}' class='list-group-item'>
	                                            <div class='row g-0 align-items-center'>
		                                            <div class='col-12 pl-2'>
			                                            <div class='text-dark'>{item.Name}</div>
			                                            <div class='text-muted small mt-1'>{Helpers.FormatLongText(item.Message, 100)}</div>
			                                            <div class='text-muted small mt-1'>{Helpers.GetTimeSince(item.CreatedAt)} ago</div>
		                                            </div>
	                                            </div>
                                            </a>";
                                total_count++;
                            }
                            else
                            {
                                return new HtmlString(result_data);
                            }
                        }
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Gets total notificattion for user
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public static int GetTotalNotifications(string account_id)
        {
            int total_notifications = 0;
            using (var db = new DBConnection())
            {
                //new account notifications
                int TotalPending = db.Accounts.Count(s => !s.Active);
                if (TotalPending > 0)
                {
                    total_notifications += TotalPending;
                }
            }
            return total_notifications;
        }


        /// <summary>
        /// Gets total notification list for user
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public static HtmlString GetNotifications(string account_id)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                //new account notifications
                int TotalPending = db.Accounts.Count(s => !s.Active);
                if (TotalPending > 0)
                {
                    result_data += @$"<a href='/Admin/Administration/?tab=users' class='list-group-item'>
	                                    <div class='row g-0 align-items-center'>
		                                    <div class='col-2'>
			                                    <i class='text-warning' data-feather='bell'></i>
		                                    </div>
		                                    <div class='col-10'>
			                                    <div class='text-dark'> {TotalPending} Account(s) pending action</div>
		                                    </div>
	                                    </div>
                                    </a>";
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Gets total users
        /// </summary>
        /// <returns></returns>
        public static int GetTotalUsers()
        {
            using (var db = new DBConnection())
            {
                return db.Accounts.Count();
            }
        }

    }


    //Helpers for Resources
    public class ResourcesHelpers
    {
        public static string GetDocIcon(string file_type)
        {
            switch (file_type)
            {
                case "doc":
                case "docx":
                    return "fas fa-file-word";
                case "ppt":
                case "pptx":
                    return "fas fa-file-powerpoint";
                case "pdf":
                    return "fas fa-file-pdf";
                case "txt":
                    return "fas fa-file-alt";
                case "xlsx":
                case "xls":
                    return "fas fa-file-excel";
                default:
                    return "fas fa-file-alt";
            }
        }
    }

    //Helpers for Posts
    public class PostsHelpers
    {
        /// <summary>
        /// Gets category name using category id
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        public static string GetCategoryName(string category_id)
        {
            using (var db = new DBConnection())
            {
                var DBQuery = db.Categories.Where(s => s.CategoryID == category_id);
                if (DBQuery.Any())
                {
                    return DBQuery.FirstOrDefault().CategoryName;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets category id using category name
        /// </summary>
        /// <param name="category_name"></param>
        /// <returns></returns>
        public static string GetCategoryID(string category_name)
        {
            category_name = Helpers.ConvertCase(category_name, "TitleTrim");
            using (var db = new DBConnection())
            {
                var DBQuery = db.Categories.Where(s => s.ShortCategoryName == category_name);
                if (DBQuery.Any())
                {
                    return DBQuery.FirstOrDefault().CategoryID;
                }
            }
            return null;
        }

        /// <summary>
        /// Formats category with badge tag
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static HtmlString FormatCategories(string categories)
        {
            string result_data = "";
            if (!string.IsNullOrEmpty(categories))
            {
                if (categories.Contains(","))
                {
                    string[] CategoryLists = categories.Split(",");
                    // Loop over strings and format
                    foreach (string category in CategoryLists)
                    {
                        result_data += $@"<span class='badge bg-secondary mr-1'>{category}</span>";
                    }
                }
                else
                {
                    result_data += $@"<span class='badge bg-secondary'>{categories}</span>";
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Formats category with badge tag
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static HtmlString FormatPostCategories(string categories)
        {
            string result_data = "";
            if (!string.IsNullOrEmpty(categories))
            {
                if (categories.Contains(","))
                {
                    string[] CategoryLists = categories.Split(",");
                    // Loop over strings and format
                    foreach (string category in CategoryLists)
                    {
                        result_data += $@"<span class='badge bg-secondary mr-2'><a href='/Category/{category}' class='text-white'>{category}</a></span> ";
                    }
                }
                else
                {
                    result_data += $@"<span class='badge bg-secondary'><a href='/Category/{categories}' class='text-white'>{categories}</a></span>";
                }
            }
            return new HtmlString(result_data);
        }



        /// <summary>
        /// Formats tags with badge tag
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static HtmlString FormatPostTags(string tags)
        {
            string result_data = "";
            if (!string.IsNullOrEmpty(tags))
            {
                if (tags.Contains(","))
                {
                    string[] TagsLists = tags.Split(",");
                    // Loop over strings and format
                    foreach (string tag in TagsLists)
                    {
                        result_data += $@"<a href='/Tags/{tag}' type='button' class='btn btn-outline-primary mr-1 mb-2'>{tag}</a> ";
                    }
                }
                else
                {
                    result_data += $@"<a href ='/Tag/{tags}' type='button' class='btn btn-outline-primary mr-1'>{tags}</a>";
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Gets total posts
        /// </summary>
        /// <returns></returns>
        public static int GetTotalPosts()
        {
            using (var db = new DBConnection())
            {
                return db.Posts.Count();
            }
        }


        /// <summary>
        /// Gets specific post data. Takes post id and data to return
        /// </summary>
        /// <param name="post_id"></param>
        /// <param name="return_data"></param>
        /// <returns></returns>
        public static string GetPostData(string post_id, string return_data)
        {
            try
            {
                if (!string.IsNullOrEmpty(post_id))
                {
                    using (var db = new DBConnection())
                    {
                        switch (return_data)
                        {
                            case "ID":
                                return (db.Posts.Any(s => s.PostID == post_id)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().ID.ToString() : "0";
                            case "PostType":
                                return (db.Posts.Any(s => s.PostID == post_id && s.PostType != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().PostType : "";
                            case "Slug":
                                return (db.Posts.Any(s => s.PostID == post_id && s.Slug != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().Slug : "";
                            case "Author":
                                return (db.Posts.Any(s => s.PostID == post_id && s.Author != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().Author : "";
                            case "Categories":
                                return (db.Posts.Any(s => s.PostID == post_id && s.Categories != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().Categories : "";
                            case "Title":
                                return (db.Posts.Any(s => s.PostID == post_id && s.Title != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().Title : "";
                            case "ShortDescription":
                                return (db.Posts.Any(s => s.PostID == post_id && s.ShortDescription != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().ShortDescription : "";
                            case "PostImage":
                                return (db.Posts.Any(s => s.PostID == post_id && s.PostImage != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().PostImage : "";
                            case "ImageCaption":
                                return (db.Posts.Any(s => s.PostID == post_id && s.ImageCaption != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().ImageCaption : "";
                            case "FeaturedPost":
                                return (db.Posts.Any(s => s.PostID == post_id)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().FeaturedPost.ToString() : "false";
                            case "Content":
                                return (db.Posts.Any(s => s.PostID == post_id && s.Content != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().Content : "";
                            case "PostTags":
                                return (db.Posts.Any(s => s.PostID == post_id && s.PostTags != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().PostTags : "";
                            case "Status":
                                return (db.Posts.Any(s => s.PostID == post_id)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().Status.ToString() : "0";
                            case "UpdatedAt":
                                return (db.Posts.Any(s => s.PostID == post_id && s.UpdatedAt != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().UpdatedAt.ToString() : "";
                            case "CreatedAt":
                                return (db.Posts.Any(s => s.PostID == post_id && s.CreatedAt != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().CreatedAt.ToString() : "";
                            default:
                                return "NA";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return "NA";
        }


        public static int GetCategoryCount(string category_name)
        {
            using (var db = new DBConnection())
            {
                return db.Posts.Count(s => s.Categories.Contains(category_name));
            }
        }


    }

    //Helpers for Navigation
    public class NavigationHelpers
    {
        public static HtmlString GetNavigationName(string navigation_id)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Navigations.Where(s => s.NavigationID == navigation_id);
                if (DBQuery.Any())
                {
                    result_data += $@"<span class='badge bg-secondary mr-1'>{DBQuery.FirstOrDefault().NavigationName}</span>";
                }
            }
            return new HtmlString(result_data);
        }

        /// <summary>
        /// Gets navigation select options
        /// </summary>
        /// <returns></returns>
        public static HtmlString GetNavigationOptions()
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Navigations.OrderBy(s => s.NavigationName);
                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        result_data += $@"<option value='{item.NavigationID}'>{item.NavigationName}</option>";
                    }
                }
            }
            return new HtmlString(result_data);
        }
    }

    //Helpers for Pages
    public class PagesHelpers
    {
        /// <summary>
        /// Gets total pages
        /// </summary>
        /// <returns></returns>
        public static int GetTotalPages()
        {
            using (var db = new DBConnection())
            {
                return db.Pages.Count();
            }
        }

    }

    //Helpers for Services
    public class ServicesHelpers
    {

    }

    //Helpers for Portfolio
    public class PortfolioHelpers
    {
        /// <summary>
        /// Gets portfolio preview image, returns default image place holder if not existing
        /// </summary>
        /// <param name="portfolio_id"></param>
        /// <returns></returns>
        public static string GetPreviewImage(string portfolio_id)
        {
            string portfolio_image = "https://via.placeholder.com/650x360/2828ed/FFFFFF/?text=No+Image"; //default post image
            using (var db = new DBConnection())
            {
                var DBQuery = db.Portfolios.Where(s => s.PortfolioID == portfolio_id);
                if (DBQuery.Any())
                {
                    var Directory = "/portfolio/images/" + GetPortfolioImageDirectory(portfolio_id) + "/";
                    string image_file = DBQuery.FirstOrDefault().Image;
                    portfolio_image = (!string.IsNullOrEmpty(image_file)) ? Directory + image_file : portfolio_image; //return directory + image, or default if empty
                }
            }
            return portfolio_image;
        }


        /// <summary>
        /// Get portfolio image folder name
        /// </summary>
        /// <param name="portfolio_id"></param>
        /// <returns></returns>
        public static string GetPortfolioImageDirectory(string portfolio_id) 
        {
            using (var db = new DBConnection())
            {
                var DBQuery = db.Portfolios.Where(s => s.PortfolioID == portfolio_id);
                if (DBQuery.Any())
                {
                    var DateString = DBQuery.FirstOrDefault().CreatedAt.ToString();
                    DateTime FormattedDate = DateTime.Parse(DateString);
                    return FormattedDate.ToString("MM-yyyy");
                }
            }
            return null;
        }



        /// <summary>
        /// Get portfolio images from image gallery
        /// </summary>
        /// <param name="portfolio_id"></param>
        /// <returns></returns>
        public static HtmlString GetPortfolioImages(string portfolio_id)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.GalleryImages.Where(s => s.ParentID == portfolio_id);
                if (DBQuery.Any())
                {
                    string Directory = GetPortfolioImageDirectory(portfolio_id);
                    
                    foreach(var item in DBQuery)
                    {
                        string portfolio_image = "/portfolio/images/" + Directory + "/" + item.Image;
                        result_data += $@"<div class='swiper-slide'>
	                                        <img src='{portfolio_image}' alt='Portfolio Image'>
                                        </div>";
                    }

                }
            }
            return new HtmlString(result_data);
        }
    }

    //Helpers for Team
    public class TeamHelpers
    {

    }

    //Helpers for Partners
    public class PartnersHelpers
    {

    }

    //Helpers for Contact
    public class ContactHelpers
    {
        /// <summary>
        /// Formats record status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static HtmlString FormatReadStatus(string contact_id, string account_id)
        {
            string result_data = "";

            using (var db = new DBConnection())
            {
                var DBQuery = db.MessageViews.Where(s => s.ContactID == contact_id && s.AccountID == account_id);
                if (!DBQuery.Any())
                {
                    result_data += "<i class='fas fa-circle fa-xs text-info mr-2'></i>";
                }
            }

            return new HtmlString(result_data);
        }
    }

    //Helpers for FAQ
    public class FAQHelpers
    {

    }

    //Helpers for Testimonials
    public class TestimonialsHelpers
    {

    }

    //Helpers for Apearance
    public class ApearanceHelpers
    {
        public static string GetColorName(string color_code)
        {
            try
            {
                color_code = color_code.Replace("#", "");
                string Url = @$"https://www.thecolorapi.com/id?hex={color_code}";

                var Request = WebRequest.Create(Url);
                using (WebResponse wrs = Request.GetResponse())
                using (Stream stream = wrs.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    var obj = JObject.Parse(json);

                    string ColorName = (string)obj["name"]["value"];
                    return ColorName;
                }
            }
            catch (Exception)
            {
                //TODO log error
                return "NA";
            }
        }
    }


    //Helpers for Content Management
    public class CMSHelpers
    {
        /// <summary>
        ///get specific config data. Takes key and data to return
        /// </summary>
        public static string GetCmsData(string unique_key)
        {
            try
            {
                if (!string.IsNullOrEmpty(unique_key))
                {
                    using (var db = new DBConnection())
                    {
                        var DBQuery = db.ContentManagement.Where(s => s.ContentName == unique_key);
                        if (DBQuery.Any())
                        {
                            return DBQuery.FirstOrDefault().ContentValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return "";
        }

        /// <summary>
        ///get specific config data. Takes key and data to return
        /// </summary>
        public static string GetCmsData(string unique_key, string default_value)
        {
            try
            {
                if (!string.IsNullOrEmpty(unique_key))
                {
                    using (var db = new DBConnection())
                    {
                        var DBQuery = db.ContentManagement.Where(s => s.ContentName == unique_key);
                        if (DBQuery.Any())
                        {
                            return DBQuery.FirstOrDefault().ContentValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return default_value;
        }

        /// <summary>
        ///get content display status and return checkbox format
        /// </summary>
        public static string GetContentDisplayCheck(string unique_key)
        {
            try
            {
                if (!string.IsNullOrEmpty(unique_key))
                {
                    using (var db = new DBConnection())
                    {
                        var DBQuery = db.ContentDisplay.Where(s => s.ContentName == unique_key);
                        if (DBQuery.Any())
                        {
                            return (DBQuery.FirstOrDefault().DisplayStatus) ? "checked" : "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return "";
        }


        /// <summary>
        ///get content display status 
        /// </summary>
        public static bool ContentDisplay(string unique_key)
        {
            try
            {
                if (!string.IsNullOrEmpty(unique_key))
                {
                    using (var db = new DBConnection())
                    {
                        var DBQuery = db.ContentDisplay.Where(s => s.ContentName == unique_key);
                        if (DBQuery.Any())
                        {
                            return (DBQuery.FirstOrDefault().DisplayStatus);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return false;
        }


        /// <summary>
        /// Get Home Slider Data
        /// </summary>
        /// <param name="return_data"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string GetHomeSliderData(string return_data , int position)
        {
            try
            {
                using (var db = new DBConnection())
                {
                    var DBQuery = db.HomeSliders.Where(s => s.Status == 1).OrderBy(x=> x.ID);
                    if (DBQuery.Any())
                    {
                        int Count = 1;
                        foreach (var item in DBQuery)
                        {
                            if(Count == position)
                            {
                                switch (return_data)
                                {
                                    case "SliderTitle":
                                        return DBQuery.FirstOrDefault().SliderTitle;
                                    case "SubText":
                                        return DBQuery.FirstOrDefault().SubText;
                                    case "SliderLink":
                                        return DBQuery.FirstOrDefault().SliderLink;
                                    case "SliderImage":
                                        return DBQuery.FirstOrDefault().SliderImage;
                                    default:
                                        return "";
                                }
                            }
                            Count++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return "";
        }

    }

    //Helpers for Administration
    public class AdministrationHelpers
    {

    }

    //Helpers for Configuration
    public class ConfigurationHelpers
    {
        /// <summary>
        ///get specific config data. Takes key and data to return
        /// </summary>
        public static string GetConfigurationsData(string unique_key)
        {
            try
            {
                if (!string.IsNullOrEmpty(unique_key))
                {
                    using (var db = new DBConnection())
                    {
                        var DBQuery = db.Configurations.Where(s => s.ConfigurationKey == unique_key);
                        if (DBQuery.Any())
                        {
                            return DBQuery.FirstOrDefault().ConfigurationValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return null;
        }


        /// <summary>
        ///get specific config data. Takes key and data to return
        /// </summary>
        public static string GetConfigurationsData(string unique_key, string default_value)
        {
            try
            {
                if (!string.IsNullOrEmpty(unique_key))
                {
                    using (var db = new DBConnection())
                    {
                        var DBQuery = db.Configurations.Where(s => s.ConfigurationKey == unique_key);
                        if (DBQuery.Any())
                        {
                            return DBQuery.FirstOrDefault().ConfigurationValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return default_value;
        }

        /// <summary>
        ///get specific config data. Takes key and data to return
        /// </summary>
        public static HtmlString GetConfigurationsHtmlData(string unique_key)
        {
            try
            {
                if (!string.IsNullOrEmpty(unique_key))
                {
                    using (var db = new DBConnection())
                    {
                        var DBQuery = db.Configurations.Where(s => s.ConfigurationKey == unique_key);
                        if (DBQuery.Any())
                        {
                            return new HtmlString(DBQuery.FirstOrDefault().ConfigurationValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return null;
        }


        /// <summary>
        ///get configuration status and return checkbox format
        /// </summary>
        public static string GetConfigurationCheck(string unique_key)
        {
            try
            {
                if (!string.IsNullOrEmpty(unique_key))
                {
                    using (var db = new DBConnection())
                    {
                        var DBQuery = db.Configurations.Where(s => s.ConfigurationKey == unique_key);
                        if (DBQuery.Any())
                        {
                            return (Helpers.BooleanParse(DBQuery.FirstOrDefault().ConfigurationValue)) ? "checked" : "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return "";
        }



        /// <summary>
        ///get configuration status and return checkbox format
        /// </summary>
        public static bool GetConfigurationStatus(string unique_key)
        {
            try
            {
                if (!string.IsNullOrEmpty(unique_key))
                {
                    using (var db = new DBConnection())
                    {
                        var DBQuery = db.Configurations.Where(s => s.ConfigurationKey == unique_key);
                        if (DBQuery.Any())
                        {
                            return (Helpers.BooleanParse(DBQuery.FirstOrDefault().ConfigurationValue));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
            }
            return false;
        }


        /// <summary>
        /// Get Language Options
        /// </summary>
        /// <returns></returns>
        public static HtmlString GetLanguageOptions() {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Languages.OrderBy(s=> s.Name);
                if (DBQuery.Any())
                {
                    foreach(var item in DBQuery)
                    {
                        result_data += $@"<option value='{item.ISO}'>{item.Name} ({item.ISO})</option>";
                    }
                }
            }
            return new HtmlString(result_data);
        }
    }

    //Helpers for SQL Queries
    public class SqlHelpers
    {
        /// <summary>
        /// Gets table column value base on the key passed  
        /// </summary>
        /// <param name="model_name"></param>
        /// <param name="pk_name"></param>
        /// <param name="pk_value"></param>
        /// <param name="return_data"></param>
        /// <returns></returns>
        public static string GetTablesData(string model_name, string pk_name, string pk_value, string return_data)
        {
            string[] ValidationInputs = { model_name, pk_name, pk_value, return_data };
            if (!Helpers.ValidateInputs(ValidationInputs))
            {
                return "";
            }

            try
            {
                using(var db = new DBConnection())
                {
                    switch (model_name)
                    {
                        case "ContentManagement":
                            if (return_data == "UpdatedAt")
                            {
                                return db.ContentManagement.Where(s => s.ContentName == pk_value).FirstOrDefault().UpdatedAt.ToString();
                            }
                            return db.ContentManagement.Where(s => s.ContentName == pk_value).FirstOrDefault().ContentValue;
                        case "Country":
                            return db.Countries.Where(s => s.ID == Helpers.Int32Parse(pk_value)).FirstOrDefault().Name;
                        case "Contact":
                            if (return_data == "Name")
                            {
                                return db.Contacts.Where(s => s.ContactID == pk_value).FirstOrDefault().Name;
                            }
                            else if (return_data == "Email")
                            {
                                return db.Contacts.Where(s => s.ContactID == pk_value).FirstOrDefault().Email;
                            }
                            else if (return_data == "Phone")
                            {
                                return db.Contacts.Where(s => s.ContactID == pk_value).FirstOrDefault().Phone;
                            }
                            else if (return_data == "Subject")
                            {
                                return db.Contacts.Where(s => s.ContactID == pk_value).FirstOrDefault().Subject;
                            }
                            else if (return_data == "Message")
                            {
                                return db.Contacts.Where(s => s.ContactID == pk_value).FirstOrDefault().Message;
                            }
                            return null;
                        case "Configurations":
                            return db.Configurations.Where(s => s.ConfigurationKey == pk_value).FirstOrDefault().ConfigurationValue;
                        case "ThemeSettings":
                            return db.ThemeSettings.Where(s => s.SettingName == pk_value).FirstOrDefault().SettingValue;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return null;
        }

        /// <summary>
        /// Gets table column value base on the key passed  
        /// </summary>
        /// <param name="model_name"></param>
        /// <param name="pk_name"></param>
        /// <param name="pk_value"></param>
        /// <param name="return_data"></param>
        /// <returns></returns>
        public static string GetTableData(string model_name, string pk_name, string pk_value, string return_data)
        {
            string[] ValidationInputs = { model_name, pk_name, pk_value, return_data };
            if (!Helpers.ValidateInputs(ValidationInputs))
            {
                return "";
            }

            string connection_string = AppSettings.DBConnection();

            string return_value = "";

            var DBQuery = @"SELECT [" + return_data + "] FROM [" + model_name + "] WHERE [" + pk_name + "]  = @key";
            try
            {
                using (var con = new SqlConnection(connection_string))
                {
                    con.Open();
                    var cmd = new SqlCommand(DBQuery, con);
                    cmd.Parameters.AddWithValue("@key", pk_value);
                    if (cmd.ExecuteScalar() != DBNull.Value && cmd.ExecuteScalar() != null)
                    {
                        return_value = cmd.ExecuteScalar().ToString();
                    }
                    else
                    {
                        return_value = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                //throw; TODO Log error
                return_value = null;
            }

            return return_value;
        }


        /// <summary>
        /// Gets table column value base on the key passed  
        /// </summary>
        /// <param name="model_name"></param>
        /// <param name="pk_name_1"></param>
        /// <param name="pk_value_1"></param>
        /// <param name="pk_name_2"></param>
        /// <param name="pk_value_2"></param>
        /// <param name="return_data"></param>
        /// <returns></returns>
        public static string GetTableData(string model_name, string pk_name_1, string pk_value_1, string pk_name_2, string pk_value_2, string return_data)
        {
            string[] ValidationInputs = { model_name, pk_name_1, pk_value_1, pk_name_2, pk_value_2, return_data };
            if (!Helpers.ValidateInputs(ValidationInputs))
            {
                return "";
            }

            string connection_string = AppSettings.DBConnection();

            string return_value = "";

            var DBQuery = @"SELECT [" + return_data + "] FROM [" + model_name + "] WHERE [" + pk_value_1 + "]  = @pk_value_1 AND  [" + pk_value_2 + "]  = @pk_value_2";
            try
            {
                using (var con = new SqlConnection(connection_string))
                {
                    con.Open();
                    var cmd = new SqlCommand(DBQuery, con);
                    cmd.Parameters.AddWithValue("@pk_value_1", pk_value_1);
                    if (cmd.ExecuteScalar() != DBNull.Value && cmd.ExecuteScalar() != null)
                    {
                        return_value = cmd.ExecuteScalar().ToString();
                    }
                    else
                    {
                        return_value = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                //throw; TODO Log error
                return_value = null;
            }

            return return_value;
        }
    }

    //App Helpers
    public class Helpers
    {
        //Converts string to integer
        /// <summary>
        /// Converts string to integer, returns zero if fails
        /// </summary>
        /// <returns>integer</returns>
        public static int Int32Parse(string string_number)
        {
            string_number = (!string.IsNullOrEmpty(string_number)) ? string_number : "0";
            try
            {
                return Int32.Parse(string_number);
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        //Converts string to integer
        /// <summary>
        /// Converts string to integer, returns default passed if fails
        /// </summary>
        /// <returns>integer</returns>
        public static int Int32Parse(string string_number, int return_default)
        {
            string_number = (!string.IsNullOrEmpty(string_number)) ? string_number : return_default.ToString();
            try
            {
                return Int32.Parse(string_number);
            }
            catch (FormatException)
            {
                return return_default;
            }
        }


        /// <summary>
        /// Converts string to integer, returns false if fails
        /// </summary>
        /// <param name="string_boolean"></param>
        /// <returns></returns>
        public static bool BooleanParse(string string_boolean)
        {
            try
            {
                switch (string_boolean)
                {
                    case "1":
                        return true;
                    case "0":
                        return false;
                    default:
                        return Convert.ToBoolean(string_boolean);
                }
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Converts string to integer, returns deafult if fails
        /// </summary>
        /// <param name="string_boolean"></param>
        /// <param name="return_default"></param>
        /// <returns></returns>
        public static bool BooleanParse(string string_boolean, bool return_default)
        {
            try
            {
                return Convert.ToBoolean(string_boolean);
            }
            catch (FormatException)
            {
                return return_default;
            }
        }


        //removes html tags from text
        public static string StripHTML(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = Regex.Replace(text, "<.*?>", "");
            }
            return text;
        }


        //trims text to the nearest desired lenght passed in the parameter
        public static string FormatLongText(string text, int max_length)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > max_length)
            {
                int iNextSpace = text.LastIndexOf(" ", max_length, StringComparison.Ordinal);
                text = $"{(text.Substring(0, (iNextSpace > 0) ? iNextSpace : max_length).Trim())}...";
            }
            return text;
        }

        //convert text case
        public static string ConvertCase(string text, string convert_to)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(convert_to))
            {
                return text;
            }
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            switch (convert_to)
            {
                case "Upper":
                    // convert to upper case
                    return textInfo.ToUpper(text);
                case "Lower":
                    // convert to lower case
                    return textInfo.ToLower(text);
                case "Title":
                    // convert to title case
                    return textInfo.ToTitleCase(text);
                case "TitleTrim":
                    // convert to title case and remove space
                    return Regex.Replace(textInfo.ToTitleCase(text), @"\s+", "");
                case "SplitUpper":
                    //split text by capital case
                    return Regex.Replace(text, "([A-Z])", " $1").Trim();
                default:
                    return text;
            }
        }

        //formats alert style based on total number
        public static HtmlString FormatAlert(int total_number)
        {
            string result = $"<span class='badge bg-secondary rounded-pill'>{total_number}</span>";
            if (total_number == 0)
            {
                result = $"<span class='badge bg-danger  rounded-pill'>{total_number}</span>";
            }
            else if (total_number > 0)
            {
                result = $"<span class='badge bg-success rounded-pill'>{total_number}</span>";
            }
            return new HtmlString(result);
        }



        //dispaly passed text ot default if empty
        public static string DefaultData(string text, string default_text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return text;
            }
            return default_text;
        }


        // return how much time passed since date object
        public static string GetTimeSince(DateTime? objDateTime)
        {
            DateTime newDateObject = (DateTime)((objDateTime == null) ? DateTime.Now : objDateTime);
            // here we are going to subtract the passed in DateTime from the current time converted to UTC
            TimeSpan ts = DateTime.Now.ToUniversalTime().Subtract(newDateObject);
            int intDays = ts.Days;
            int intHours = ts.Hours;
            int intMinutes = ts.Minutes;
            int intSeconds = ts.Seconds;

            if (intDays > 0)
                return (intDays == 1) ? string.Format("{0} day", intDays) : string.Format("{0} days", intDays);

            if (intHours > 0)
                return (intHours == 1) ? string.Format("{0} hour", intHours) : string.Format("{0} hours", intHours);

            if (intMinutes > 0)
                return (intMinutes == 1) ? string.Format("{0} minute", intMinutes) : string.Format("{0} minutes", intMinutes);

            if (intSeconds > 0)
                return (intSeconds == 1) ? string.Format("{0} second", intSeconds) : string.Format("{0} second", intSeconds);

            // let's handle future times..just in case
            if (intDays < 0)
                return string.Format("{0} days", Math.Abs(intDays));

            if (intHours < 0)
                return string.Format("{0} hours", Math.Abs(intHours));

            if (intMinutes < 0)
                return string.Format("{0} minutes", Math.Abs(intMinutes));

            if (intSeconds < 0)
                return string.Format("{0} seconds", Math.Abs(intSeconds));

            return "just now";
        }


        //Validate required inputs 
        /// <summary>
        /// Validates array of inputs for not null
        /// </summary>
        /// <returns>boolean</returns>
        public static bool ValidateInputs(string[] inputs)
        {
            // Loop over and check if empty.
            for (int i = 0; i < inputs.Length; i++)
            {
                if (string.IsNullOrEmpty(inputs[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Formats date to MMMM dd, yyyy. E.g: February 24, 2021
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatDate(string date)
        {
            try
            {
                if (!string.IsNullOrEmpty(date))
                {
                    DateTime newDate = DateTime.Parse(date);
                    return String.Format("{0:MMMM dd, yyyy}", newDate);
                }
            }
            catch (Exception)
            {
                //TODO log error
            }

            return date;
        }



        /// <summary>
        /// Formats date to MMMM dd, yyyy. E.g: February 24, 2021
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatDate(string date, string format)
        {
            try
            {
                if (!string.IsNullOrEmpty(date))
                {
                    DateTime newDate = DateTime.Parse(date);
                    return String.Format("{0:" + format + "}", newDate);
                }
            }
            catch (Exception)
            {
                //TODO log error
            }

            return date;
        }


        /// <summary>
        /// Formats record status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static HtmlString FormatStatus(string status)
        {
            if (status == "0")
            {
                return new HtmlString(@$"<button class='btn btn-danger'><i class='fas fa-times'></i> Pending</button>");
            }
            else if (status == "1")
            {
                return new HtmlString(@$"<button class='btn btn-success'><i class='fas fa-check'></i> Published</button>");
            }
            else
            {
                return new HtmlString(@$"<button class='btn btn-info'><i class='fas fa-question'></i> {status}</button>");
            }
        }


        /// <summary>
        /// Formats record status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static HtmlString FormatActiveStatus(string status)
        {
            if (status == "0")
            {
                return new HtmlString(@$"<button class='btn btn-danger'><i class='fas fa-times'></i> Pending</button>");
            }
            else if (status == "1")
            {
                return new HtmlString(@$"<button class='btn btn-success'><i class='fas fa-check'></i> Active</button>");
            }
            else
            {
                return new HtmlString(@$"<button class='btn btn-info'><i class='fas fa-question'></i> {status}</button>");
            }
        }

        /// <summary>
        /// Formats record status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static HtmlString FormatStatus(bool status)
        {
            if (!status)
            {
                return new HtmlString(@$"<button class='btn btn-danger'><i class='fas fa-times'></i> Pending</button>");
            }
            return new HtmlString(@$"<button class='btn btn-success'><i class='fas fa-check'></i> Published</button>");
        }


        /// <summary>
        /// Formats record status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static HtmlString FormatActiveStatus(bool status)
        {
            if (!status)
            {
                return new HtmlString(@$"<button class='btn btn-danger'><i class='fas fa-times'></i> Pending</button>");
            }
            return new HtmlString(@$"<button class='btn btn-success'><i class='fas fa-check'></i> Active</button>");
        }

        /// <summary>
        /// Gets post/page image, returns default image place holder if not existing
        /// </summary>
        /// <param name="post_id"></param>
        /// <param name="post_type"></param>
        /// <returns></returns>
        public static string GetPostImage(string post_id, string post_type)
        {
            string post_image = "https://via.placeholder.com/650x360/2828ed/FFFFFF/?text=No+Image"; //default post image
            using (var db = new DBConnection())
            {
                if (post_type == "Post")
                {
                    var DBQuery = db.Posts.Where(s => s.PostID == post_id);
                    if (DBQuery.Any())
                    {
                        var Directory = "/posts/images/" + GetPostImageDirectory(post_id, "Post") + "/";
                        string image_file = DBQuery.FirstOrDefault().PostImage;
                        post_image = (!string.IsNullOrEmpty(image_file)) ? Directory + image_file : post_image; //return directory + image, or default if empty
                    }
                }
                else if (post_type == "Page")
                {
                    var DBQuery = db.Pages.Where(s => s.PageID == post_id);
                    if (DBQuery.Any())
                    {
                        var Directory = "/pages/images/" + GetPostImageDirectory(post_id, "Page") + "/";
                        string image_file = DBQuery.FirstOrDefault().PageImage;
                        post_image = (!string.IsNullOrEmpty(image_file)) ? Directory + image_file : post_image; //return directory + image, or default if empty
                    }
                }
            }
            return post_image;
        }

        //get post image folder name
        public static string GetPostImageDirectory(string post_id, string post_type)
        {
            using (var db = new DBConnection())
            {
                if (post_type == "Page")
                {
                    var DBQuery = db.Pages.Where(s => s.PageID == post_id);
                    if (DBQuery.Any())
                    {
                        var DateString = DBQuery.FirstOrDefault().CreatedAt.ToString();
                        DateTime FormattedDate = DateTime.Parse(DateString);
                        return FormattedDate.ToString("MM-yyyy");
                    }
                }
                else
                {
                    var DBQuery = db.Posts.Where(s => s.PostID == post_id);
                    if (DBQuery.Any())
                    {
                        var DateString = DBQuery.FirstOrDefault().CreatedAt.ToString();
                        DateTime FormattedDate = DateTime.Parse(DateString);
                        return FormattedDate.ToString("MM-yyyy");
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Gets current post galleries for edit view
        /// </summary>
        /// <param name="post_id"></param>
        /// <returns>gallery images</returns>
        public static HtmlString GetEditGalleryImages(string post_id, string post_type)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.GalleryImages.Where(s => s.ParentID == post_id);
                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //get posts, pages, or portfolio link
                        var image_link = "";

                        switch (post_type)
                        {
                            case "Post":
                                image_link = "/posts/images/" + GetPostImageDirectory(post_id, post_type) + "/" + item.Image;
                                break;
                            case "Page":
                                image_link = "/pages/images/" + GetPostImageDirectory(post_id, post_type) + "/" + item.Image;
                                break;
                            case "Portfolio":
                                image_link = "/portfolio/images/" + PortfolioHelpers.GetPortfolioImageDirectory(post_id) + "/" + item.Image;
                                break;
                            default:
                                image_link = "/posts/images/" + GetPostImageDirectory(post_id, post_type) + "/" + item.Image;
                                break;
                        }

                        result_data += $@"<div class='col-sm-6 col-md-3'>
	                                        <a href='#' class='text-decoration-none text-danger float-right delete-record' data-title='Delete Image' data-label='Image' data-key='{item.ID}'
                                            data-action='/Admin/DeleteRecord' data-model='GalleryImages' data-view='Edit{post_type}' data-route-id='{item.ParentID}'>
                                                <i class='fas fa-times'></i>
                                            </a>
	                                        <img src='{image_link}' class='rounded w-100 current-gallery-image' alt='{item.Image}' height='200'> 
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Gets site logo with the format
        /// </summary>
        /// <param name="logo_format"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static HtmlString GetSiteLogo(string logo_format, int width)
        {
            width = (width < 50) ? 100 : width;
            string result_data = "";
            string logo_image = CMSHelpers.GetCmsData("Logo", "default-logo.jpg");
            switch (logo_format)
            {
                case "1":
                    result_data += $@"<img src='/images/{logo_image}' class='rounded logo-border' alt='Site Logo' width='{width}' height='{width*0.5}'>";
                    break;
                case "2":
                    result_data += $@"<img src='/images/{logo_image}' class='rounded logo-border' alt='Site Logo' width='{width}' height='{width}'>";
                    break;
                case "3":
                    result_data += $@"<img src='/images/{logo_image}' class='rounded-circle logo-border' alt='Site Logo' width='{width}' height='{width}'>";
                    break;
                default:
                    result_data += $@"<img src='/images/{logo_image}' class='rounded logo-border' alt='Site Logo' width='{width}' height='{width * 0.5}'>";
                    break;
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get country code from country name
        /// </summary>
        /// <param name="country_name"></param>
        /// <returns></returns>
        public static string GetCountryCode(string country_name)
        {
            using (var db = new DBConnection())
            {
                var DbQuery = db.Countries.Where(s => s.NiceName == country_name);
                if (DbQuery.Any())
                {
                    return DbQuery.FirstOrDefault().ISO;
                }
                return null;
            }
        }


        /// <summary>
        /// Get country name from country code
        /// </summary>
        /// <param name="country_code"></param>
        /// <returns></returns>
        public static string GetCountryName(int country_code)
        {
            using (var db = new DBConnection())
            {
                var DbQuery = db.Countries.Where(s => s.ID == country_code);
                if (DbQuery.Any())
                {
                    return DbQuery.FirstOrDefault().NiceName;
                }
                return null;
            }
        }


        /// <summary>
        /// Gets total visits
        /// </summary>
        /// <returns></returns>
        public static int GetTotalVisits()
        {
            using (var db = new DBConnection())
            {
                return db.SiteVisits.Count();
            }
        }



        /// <summary>
        /// Gets total browser visits
        /// </summary>
        /// <param name="browser_name"></param>
        /// <returns></returns>
        public static int GetTotalBrowserVisits(string browser_name)
        {
            using (var db = new DBConnection())
            {
                return db.SiteVisits.Count(s=> s.Browser.Contains(browser_name));
            }
        }



        /// <summary>
        /// Gets total post views by month
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetTotalViewsPerMonth(int month, int year)
        {
            string connection_string = AppSettings.DBConnection();
            int total_count = 0;
            var DBQuery = @$"SELECT    COUNT(*) 
                            FROM      [SiteVisits] 
                            WHERE     YEAR(CreatedAt) = '{year}' AND MONTH(CreatedAt) = '{month}' 
                            GROUP BY  MONTH(CreatedAt)";

            try
            {
                using (var con = new SqlConnection(connection_string))
                {
                    con.Open();
                    var cmd = new SqlCommand(DBQuery, con);
                    if (cmd.ExecuteScalar() != DBNull.Value && cmd.ExecuteScalar() != null)
                    {
                        total_count = Int32Parse(cmd.ExecuteScalar().ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                //throw; TODO Log error
            }

            return total_count;
        }


        /// <summary>
        /// Gets total post views by day for all
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetTotalViewsPerDay(int day, int month, int year)
        {
            string connection_string = AppSettings.DBConnection();
            int day_number = DateTime.Now.AddDays(day).Day;

            int total_count = 0;
            var DBQuery = @$"SELECT    COUNT(*) 
                            FROM      [SiteVisits] 
                            WHERE     YEAR(CreatedAt) = '{year}' AND MONTH(CreatedAt) = '{month}' AND DAY(CreatedAt) = '{day_number}'
                            GROUP BY  MONTH(CreatedAt)";

            try
            {
                using (var con = new SqlConnection(connection_string))
                {
                    con.Open();
                    var cmd = new SqlCommand(DBQuery, con);
                    if (cmd.ExecuteScalar() != DBNull.Value && cmd.ExecuteScalar() != null)
                    {
                        total_count = Int32Parse(cmd.ExecuteScalar().ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                //throw; TODO Log error
            }

            return total_count;
        }


        //Get country info
        /// <summary>
        /// Get current visitor country info based on ip address
        /// </summary>
        /// <returns>The the info for the second parameter passed</returns>
        public static string GetIpInfo(string ip_address, string return_data)
        {
            try
            {
                string url = "http://ip-api.com/json/" + ip_address;
                var request = WebRequest.Create(url);

                using (WebResponse wrs = request.GetResponse())
                using (Stream stream = wrs.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    var obj = JObject.Parse(json);
                    string country = (string)obj["country"];
                    string countryCode = (string)obj["countryCode"];
                    //string region = (string)obj["region"];
                    string regionName = (string)obj["regionName"];
                    string city = (string)obj["city"];
                    //string zip = (string)obj["zip"];
                    string lat = (string)obj["lat"];
                    string lon = (string)obj["lon"];
                    //string timezone = (string)obj["timezone"];
                    //string isp = (string)obj["isp"];

                    switch (return_data)
                    {
                        case "Country":
                            return country;
                        case "Code":
                            return countryCode;
                        case "Region":
                            return regionName;
                        case "City":
                            return city;
                        case "Latitude":
                            return lat;
                        case "Longitude":
                            return lon;
                        default:
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO log error
                Console.WriteLine(ex);
                return null;
            }
        }


        /// <summary>
        /// Gets recent visits data is array format
        /// </summary>
        /// <param name="minutes_back"></param>
        /// <returns></returns>
        public static string GetLiveVisitsData(int minutes_back)
        {
            string results_data = "";
            using(var db = new DBConnection())
            {
                DateTime DateRange = DateTime.Now.AddMinutes(minutes_back);

                var DBQuery = db.SiteVisits.Where(s => s.CreatedAt >= DateRange).OrderByDescending(x => x.ID).Take(20);

                foreach(var item in DBQuery)
                {
                    string Latitude = GetIpInfo(item.IpAddress, "Latitude");
                    string Longitude = GetIpInfo(item.IpAddress, "Longitude");
                    string City = GetIpInfo(item.IpAddress, "City");
                    if (!string.IsNullOrEmpty(Latitude) && !string.IsNullOrEmpty(Longitude) && !string.IsNullOrEmpty(City))
                    {
                        results_data += @"{coords: [" + Latitude + ", " + Longitude + "],name: '" + City + "'},";
                    }
                }

            }
            //remove last comma
            return results_data.TrimEnd(','); ;
        }


        /// <summary>
        /// Gets recent ip of visit
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="minutes_back"></param>
        /// <returns></returns>
        public static string GetRecentIP(int skip, int minutes_back)
        {
            string result_data = "";

            using (var db = new DBConnection())
            {
                DateTime DateRange = DateTime.Now.AddMinutes(minutes_back);

                var DBQuery = db.SiteVisits.Where(s => s.CreatedAt >= DateRange).OrderByDescending(x => x.ID).Skip(skip).Take(1);

                if (DBQuery.Any())
                {
                    result_data = DBQuery.FirstOrDefault().IpAddress;
                }
            }

            return result_data;
        }


        /// <summary>
        /// Gets language name from iso code
        /// </summary>
        /// <returns></returns>
        public static HtmlString GetSiteLanguageOptions()
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.SiteLanguages;
                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        result_data += $@"<option value='{item.Language}'>
                                            {Helpers.GetLanguageName(item.Language)}  ({item.Language})
                                          </option>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Gets language name from iso code, takes default language to diable
        /// </summary>
        /// <returns></returns>
        public static HtmlString GetSiteLanguageOptions(string disable_lang)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.SiteLanguages;
                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        string disable = (item.Language == disable_lang) ? "disabled" : "";
                        result_data += $@"<option value='{item.Language}' {disable}>
                                            {Helpers.GetLanguageName(item.Language)}  ({item.Language})
                                          </option>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Gets language name from iso code
        /// </summary>
        /// <param name="country_code"></param>
        /// <returns></returns>
        public static string GetLanguageName(string country_code)
        {
            using (var db = new DBConnection())
            {
                var DbQuery = db.Languages.Where(s => s.ISO == country_code);
                if (DbQuery.Any())
                {
                    return DbQuery.FirstOrDefault().Name;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets current added languages
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentLanguages()
        {
            using (var db = new DBConnection())
            {
                return db.SiteLanguages.Where(s=> s.Language != "en").Count();
            }
        }


        /// <summary>
        /// Gets list of current translated languages and lranslated data
        /// </summary>
        /// <param name="document_id"></param>
        /// <param name="model_name"></param>
        /// <param name="input_type"></param>
        /// <returns></returns>
        public static HtmlString GetAvailableLanguages(string document_id, string key_name, string model_name, string input_type, string model_title, string model_desc, string model_content)
        {
            if (string.IsNullOrEmpty(document_id) || string.IsNullOrEmpty(model_name) || string.IsNullOrEmpty(input_type))
            {
                return new HtmlString("");
            }

            string default_data = "";
            //get current default data (en)
            if (input_type == "Title")
            {
                default_data = SqlHelpers.GetTableData(model_name, key_name, document_id, model_title); //get current data
                default_data = $@"<div class='row'><div class='col-12'><h2>en</h2></div><div class='col-12 fixed-category-content'>{default_data}</div></div><hr/>"; //format data result
            }
            else if (input_type == "Description")
            {

                default_data = SqlHelpers.GetTableData(model_name, key_name, document_id, model_desc); //get current data
                default_data = $@"<div class='row'><div class='col-12'><h2>en</h2></div><div class='col-12 fixed-category-content'>{default_data}</div></div><hr/>"; //format data result
            }
            else if (input_type == "Content")
            {
                default_data = SqlHelpers.GetTableData(model_name, key_name, document_id, model_content); //get current data
                default_data = $@"<div class='row'><div class='col-12'><h2>en</h2></div><div class='col-12 fixed-category-content'>{default_data}</div></div><hr/>"; //format data result
            }

            using (var db = new DBConnection())
            {
                var DBQuery = db.DataTranslations.Where(s=> s.DocumentID == document_id && s.DocumentModel == model_name);
                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        string return_data = "";
                        switch (input_type)
                        {
                            case "Title":
                                return_data = item.TranslationTitle;
                                break;
                            case "Description":
                                return_data = item.TranslationDescription;
                                break;
                            case "Content":
                                return_data = item.TranslationContent; break;
                            default:
                                return_data = "";
                                break;

                        }
                        default_data += $@"<div class='row'>
                                            <div class='col-12'>
                                              <h2>
                                                {item.Language}
                                              </h2>
                                            </div>
                                            <div class='col-12 fixed-category-content'>
                                              {return_data}
                                            </div>
                                          </div>
                                          <hr/>";
                    }
                }
            }
            return new HtmlString(default_data);
        }


        /// <summary>
        /// Returns the appropriate flag class for the language
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string GetLanguageClass(string language)
        {
            if(language == "en")
            {
                return "gb";
            }
            else if (language == "ar")
            {
                return "sa";
            }

            return language;
        }


        /// <summary>
        /// Get translated data, return default if English
        /// </summary>
        /// <param name="default_data"></param>
        /// <param name="language"></param>
        /// <param name="document_id"></param>
        /// <param name="model"></param>
        /// <param name="return_data"></param>
        /// <returns></returns>
        public static string GetTranslatableData(string default_data, string language, string document_id, string model, string return_data)
        {
            if (language == "en")
            {
                return default_data;
            }

            string data = "";

            using (var db = new DBConnection())
            {
                var DBQuery = db.DataTranslations.Where(s => s.DocumentID == document_id && s.DocumentModel == model && s.Language == language);
                if (DBQuery.Any())
                {
                    switch (return_data)
                    {
                        case "Title":
                            data = DBQuery.FirstOrDefault().TranslationTitle;
                            break;
                        case "Description":
                            data = DBQuery.FirstOrDefault().TranslationDescription;
                            break;
                        case "Content":
                            data = DBQuery.FirstOrDefault().TranslationContent;
                            break;
                    }
                }
                else
                {
                    data = default_data;
                }
            }

            return data;
        }


        /// <summary>
        /// Get youtube video id from url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetYouTubeVideoID(string url)
        {
            //if empty return no video available
            if (string.IsNullOrEmpty(url))
            {
                return "05DqIGS_koU";
            }

            if (url.Contains("http") || url.Contains("www") || url.Contains("you"))
            {
                var uri = new Uri(url);

                // you can check host here => uri.Host <= "www.youtube.com"

                var query = HttpUtility.ParseQueryString(uri.Query);

                var videoId = string.Empty;

                if (query.AllKeys.Contains("v"))
                {
                    videoId = query["v"];
                }
                else
                {
                    videoId = uri.Segments.Last();
                }

                return videoId;
            }
            return url;
        }


        /// <summary>
        /// Checks if parent document has any gallery images
        /// </summary>
        /// <param name="document_id"></param>
        /// <returns></returns>
        public static bool DocumentHasGallery(string document_id)
        {
            using(var db = new DBConnection())
            {
                var DBQuery = db.GalleryImages.Where(s => s.ParentID == document_id);
                return (DBQuery.Any());
            }
        }


        //TODO
        /// <summary>
        /// Get Active Theme
        /// </summary>
        /// <returns></returns>
        public static string GetActiveTheme()
        {
            using (var db = new DBConnection()) {
                var DBQuery = db.ThemeSettings.Where(s => s.SettingType == "ThemeTemplate");

                if (DBQuery.Any())
                {
                    var Theme = DBQuery.FirstOrDefault().SettingValue;
                    return (!string.IsNullOrEmpty(Theme)) ? Theme : "Default";
                }
            }
            return "Default";
        }


        /// <summary>
        /// Gets current theme select options 
        /// </summary>
        /// <param name="available_theme"></param>
        /// <param name="current_theme"></param>
        /// <returns></returns>
        public static HtmlString GetAvailableTheme(string available_theme, string current_theme)
        {
            int count = 1;
            string results_data = "";

            if (!string.IsNullOrEmpty(available_theme))
            {
                //if it contains many themes
                if (available_theme.Contains(","))
                {
                    string[] ThemesArray = available_theme.Split(",");

                    foreach (string item in ThemesArray)
                    {
                        string theme_name = item;
                        string theme_image = "https://via.placeholder.com/150x100/000000/FFFFFF/?text=No+Preview";
                        if (item.Contains("#"))
                        {
                            theme_name = item.Split("#")[0];
                            theme_image = "/images/themes/" + item.Split("#")[1];
                        }
                        string checked_option = (theme_name == current_theme) ? "checked" : "";

                        results_data += @$"<div class='col-sm-6 col-md-4 mb-5'>
										  	<img src='{theme_image}' class='rounded' alt='Cinque Terre' width='150' height='100'> 
											  <label class='form-check form-check-inline mr-5'>
												<input class='form-check-input' type='radio' value='{theme_name}' id='Theme{count}' name='Theme' {checked_option}>
												<span class='form-check-label'>
												  {theme_name}
												</span>
											  </label>
										  </div>";
                        count++;
                    }
                }
                else
                {
                    //single theme
                    string theme_name = available_theme;
                    string theme_image = "https://via.placeholder.com/150x100/000000/FFFFFF/?text=No+Preview";
                    if (available_theme.Contains("#"))
                    {
                        theme_name = available_theme.Split("#")[0];
                        theme_image = "/images/themes/" + available_theme.Split("#")[1];
                    }
                    string checked_option = (theme_name == current_theme) ? "checked" : "";

                    results_data += @$"<div class='col-sm-6 col-md-4 mb-5'>
										  	<img src='{theme_image}' class='rounded' alt='Cinque Terre' width='150' height='100'> 
											  <label class='form-check form-check-inline mr-5'>
												<input class='form-check-input' type='radio' value='{theme_name}' id='Theme{count}' name='Theme' {checked_option}>
												<span class='form-check-label'>
												  {theme_name}
												</span>
											  </label>
										  </div>";
                }
            }

            return new HtmlString(results_data);
        }


        /// <summary>
        /// Get post images from image gallery
        /// </summary>
        /// <param name="document_id"></param>
        /// <returns></returns>
        public static HtmlString GetGalleryImages(string document_id, string document_type)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.GalleryImages.Where(s => s.ParentID == document_id);
                if (DBQuery.Any())
                {
                    int TotalImages = DBQuery.Count();
                    string Directory = Helpers.GetPostImageDirectory(document_id, document_type);

                    string Buttons = "";
                    for (int i = 0; i < TotalImages; i++)
                    {
                        string active = (i == 0) ? "active" : "";
                        string aria_current = (i == 0) ? "aria-current='true'" : "";
                        Buttons += $@"<button type='button' data-bs-target='#carouselGalleryIndicators' data-bs-slide-to='{i}' class='{active}' {aria_current} aria-label='Slide {i}'></button>";
                    }

                    string Images = "";
                    int Count = 0;
                    foreach (var item in DBQuery)
                    {
                        string active = (Count == 0) ? "active" : "";
                        string directory_type = (document_type == "Post") ? "posts" : "pages";
                        string gallery_image = $"/{directory_type}/images/{ Directory}/{ item.Image}";
                        Images += $@"<div class='carousel-item {active}'>
				                                <img src='{gallery_image}' class='d-block w-100' style='height: 25em;' alt=''>
			                                </div>";
                        Count++;
                    }

                    //append slider
                    result_data += $@"<div id='carouselGalleryIndicators' class='carousel slide' data-bs-ride='carousel'>
		                                <div class='carousel-indicators'>
			                                {Buttons}
		                                </div>
		                                <div class='carousel-inner'>
			                                {Images}
		                                </div>
		                                <button class='carousel-control-prev' type='button' data-bs-target='#carouselGalleryIndicators' data-bs-slide='prev'>
			                                <span class='carousel-control-prev-icon' aria-hidden='true'></span>
			                                <span class='visually-hidden'>Previous</span>
		                                </button>
		                                <button class='carousel-control-next' type='button' data-bs-target='#carouselGalleryIndicators' data-bs-slide='next'>
			                                <span class='carousel-control-next-icon' aria-hidden='true'></span>
			                                <span class='visually-hidden'>Next</span>
		                                </button>
	                                </div>";
                }
            }
            return new HtmlString(result_data);
        }


        public static string BaseUrl()
        {
            return AppSettings.BaseUrl();
        }

        public static string BaseUrl(string route)
        {
            return AppSettings.BaseUrl(route);
        }
    }

    public class AppSettings
    {

        public static string _connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBConnection"];
        public static string _baseUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("systemConfiguration")["baseUrl"];

        /// <summary>
        /// Gets appsetting BaseUrl
        /// </summary>
        /// <returns></returns>
        public static string BaseUrl(string route = null)
        {
            if (!string.IsNullOrEmpty(route))
            {
                return $"{_baseUrl}/{route}";
            }
            return _baseUrl;
        }

        /// <summary>
        /// Gets appsetting ConnectionString
        /// </summary>
        /// <returns></returns>
        public static string DBConnection()
        {
            return _connectionString;
        }

        public static string DefaultProfilePicture()
        {
            return "/admin/defaults/default-profile.jpg";
        }

    }



    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Template Helpers
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class TemplateHelpers {

        /// <summary>
        /// Get Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetNavagation(string language = "en",  string active_nav = "")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Navigations.Where(s => s.Status == 1 && (s.Parent == null || s.Parent == "")).OrderBy(x => x.Order);

                string set_active = "";

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string NavigationName = (language == "en") ? item.NavigationName : Helpers.GetTranslatableData(item.NavigationName, language, item.NavigationID, "Navigation", "Title");

                        //check if active nav
                        if (!string.IsNullOrEmpty(active_nav))
                        {
                            set_active = (item.NavigationName.ToLower() == active_nav.ToLower()) ? "active" : "";
                        }

                        string url = (item.NavigationLink.Contains("#")) ? $"{AppSettings.BaseUrl(item.NavigationLink)}" : $"{item.NavigationLink}"; 


                        result_data += $@"<li><a class='nav-link scrollto {set_active}' href='{url}'>{NavigationName}</a></li>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get Footer Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetFooterNavagation(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.FooterNavigations.Where(s => s.Status == 1).OrderBy(x => x.Order);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string NavigationName = (language == "en") ? item.NavigationName : Helpers.GetTranslatableData(item.NavigationName, language, item.NavigationID, "Navigation", "Title");

                        string url = (item.NavigationLink.Contains("#")) ? $"{AppSettings.BaseUrl(item.NavigationLink)}" : $"{item.NavigationLink}"; 


                        result_data += $@"<li><i class='bx bx-chevron-right'></i><a href='{url}'>{NavigationName}</a></li>";
                    }
                }
            }

            //add site map
            result_data += $@"<li><i class='bx bx-chevron-right'></i><a href='/Sitemap'>Sitemap</a></li>";

            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get language dropdown
        /// </summary>
        /// <param name="language_session"></param>
        /// <returns></returns>
        public static HtmlString LanguageDropDown(string language_session, string current_url = null)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.SiteLanguages.Where(x=> x.Language != language_session).OrderBy(x => x.Language);

                current_url = (!string.IsNullOrEmpty(current_url)) ? current_url : "/Home/SetLanguageSession/";

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        result_data += $@"<li>
	                                        <a class='nav-link scrollto' href='{current_url}?l={item.Language}'>
		                                        <span><span class='flag-icon flag-icon-{Helpers.GetLanguageClass(item.Language)} mr-2'></span>  {Helpers.GetLanguageName(item.Language)}</span>
	                                        </a>
                                        </li>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get home sliders with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeSliders(string language = "en")
        {
            string result_data = "";
            int count = 0;
            string active = "active";
            using (var db = new DBConnection())
            {
                var DBQuery = db.HomeSliders.Where(x => x.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        active = (count == 0) ? active : "";

                        //set title and sub text based on if language session passed
                        string SliderTitle = (language == "en") ? item.SliderTitle : Helpers.GetTranslatableData(item.SliderTitle, language, item.SliderID, "HomeSliders", "Title");
                        string SubText = (language == "en") ? item.SubText : Helpers.GetTranslatableData(Helpers.FormatLongText(item.SubText, 150), language, item.SliderID, "HomeSliders", "Description");

                        string read_more = (string.IsNullOrEmpty(item.SliderLink)) ? "" : $"<a href='{item.SliderLink}' class='btn-get-started animate__animated animate__fadeInUp scrollto theme-bg'>Read More</a>";
                        result_data += $@"<div class='carousel-item {active}' style='background-image: url({AppSettings.BaseUrl()}/sliders/images/{item.SliderImage})'>
                                          <div class='carousel-container'>
	                                        <div class='container'>
	                                          <h2 class='animate__animated animate__fadeInDown'>{SliderTitle}</h2>
	                                          <p class='animate__animated animate__fadeInUp'>{SubText}</p>
	                                          {read_more}
	                                        </div>
                                          </div>
                                        </div>";
                        count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get site statistics with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServiceStats(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.ServiceStats.Where(s => s.Status == 1).OrderBy(x => x.Order);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string StatTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.ServiceStatID, "ServiceStats", "Title");
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "" : $"<a href='{item.Link}'>Find out more &raquo;</a>";

                        result_data += $@"<div class='col-lg-3 col-md-6 d-md-flex align-items-md-stretch mb-2'>
	                                            <div class='count-box'>
		                                            <i class='{item.Icon}'></i>
		                                            <span data-purecounter-start='0' data-purecounter-end='{item.StatValue}' data-purecounter-duration='1' class='purecounter'></span>
		                                            <p><strong>{StatTitle}</strong></p>
		                                            {DetailsLink}
	                                            </div>
                                            </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get services with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServices(string language = "en")
        {
            string result_data = "";
            int delay = 100;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Services.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string ServiceTitle = (language == "en") ? item.ServiceTitle : Helpers.GetTranslatableData(item.ServiceTitle, language, item.ServiceID, "Services", "Title");
                        string ServiceLink = (string.IsNullOrEmpty(item.ServiceLink)) ? ServiceTitle : $"<a href='{item.ServiceLink}'>{ServiceTitle}</a>";
                        string ShortDescription = Helpers.GetTranslatableData(Helpers.FormatLongText(item.ShortDescription, 250), language, item.ServiceID, "Services", "Description");
                        string ReadMore = (string.IsNullOrEmpty(item.ServiceLink)) ? "" : $"<br/> <a class='btn btn-outline-danger text-center' href='{item.ServiceLink}'>Read More</a>";

                        result_data += $@"<div class='col-lg-4 col-md-6 d-flex align-items-stretch mb-3' data-aos='zoom-in' data-aos-delay='{delay}'>
	                                            <div class='icon-box w-100'>
	                                              <div class='icon '><i class='{item.ServiceIcon}'></i></div>
	                                              <h4>{ServiceLink}</h4>
	                                              <p>{ShortDescription}</p>
                                                  {ReadMore}
	                                            </div>
                                            </div>";
                        delay += 100;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get testimonials with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetTestimonials(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Testimonials.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Title = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.TestimonialID, "Testimonials", "Title");
                        string Testimonial = (language == "en") ? item.Testimonial : Helpers.GetTranslatableData(item.Testimonial, language, item.TestimonialID, "Testimonials", "Description");

                        result_data += $@"<div class='swiper-slide'>
	                                        <div class='testimonial-wrap'>
		                                        <div class='testimonial-item'>
			                                        <img src='/testimonials/images/{item.ProfileImage}' class='testimonial-img' alt='{Title}'>
			                                        <h3>{item.Name}</h3>
			                                        <h4>{Title} - {item.Organization}</h4>
			                                        <p>
				                                        <i class='bx bxs-quote-alt-left quote-icon-left'></i>
				                                        {Testimonial}
				                                        <i class='bx bxs-quote-alt-right quote-icon-right'></i>
			                                        </p>
		                                        </div>
	                                        </div>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get testimonials with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetOwlTestimonials(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Testimonials.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Title = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.TestimonialID, "Testimonials", "Title");
                        string Testimonial = (language == "en") ? item.Testimonial : Helpers.GetTranslatableData(item.Testimonial, language, item.TestimonialID, "Testimonials", "Description");

                        result_data += $@"<div>
	                                        <div class='card text-center'>
		                                        <img class='card-img-top' src='/testimonials/images/{item.ProfileImage}' alt='{item.Name} - {Title}'>
		                                        <div class='card-body'>
			                                        <h5>
				                                        {item.Name} <br />
				                                        <span> {Title} </span>
			                                        </h5>
			                                        <p class='card-text'>
				                                        <i class='bx bxs-quote-alt-left quote-icon-left'></i>
				                                        {Testimonial}
				                                        <i class='bx bxs-quote-alt-right quote-icon-right'></i>
			                                        </p>
		                                        </div>
	                                        </div>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePosts(string language = "en", int total = 3)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Posts.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PostTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.PostID, "Posts", "Title");
                        string PostImage = Helpers.GetPostImage(item.PostID, "Post");

                        result_data += $@"<div class='col-md-4 mb-2'>
	                                        <div class='card' style='min-height: 28rem;'>
                                                 <a href='Post/{item.Slug}'>
		                                          <img src='{PostImage}' class='card-img-top' style='max-height: 15rem;' alt='{Helpers.FormatLongText(PostTitle, 50)}'>
		                                        </a>
		                                        <div class='card-body'>
			                                        <h5 class='card-title'>
                                                        <a href='Post/{item.Slug}' class='text-dark text-decoration-none'>
					                                        {Helpers.FormatLongText(PostTitle, 50)}
                                                        </a>
			                                        </h5>
				                                        <p class='card-text'>
                                                            {Helpers.FormatLongText(item.ShortDescription, 125)}
				                                        </p>
				                                        <a href='Post/{item.Slug}' class='btn btn-light'>Read More</a>
		                                        </div>
	                                        </div>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }




        /// <summary>
        /// Get team with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeTeam(string language = "en")
        {
            string result_data = "";
            int count = 1;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Teams.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Title = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.TeamID, "Team", "Title");
                        string ProfileImage = $@"/team/images/{item.ProfileImage}";
                        string TwitterLink = (string.IsNullOrEmpty(item.Twitter)) ? "" : $"<a href='{item.Twitter}' target='_blank'><i class='bi bi-twitter'></i></a>";
                        string FacebookLink = (string.IsNullOrEmpty(item.Facebook)) ? "" : $"<a href='{item.Facebook}' target='_blank'><i class='bi bi-facebook'></i></a>";
                        string InstagramLink = (string.IsNullOrEmpty(item.Instagram)) ? "" : $"<a href='{item.Instagram}' target='_blank'><i class='bi bi-instagram'></i></a>";
                        string LinkedInLink = (string.IsNullOrEmpty(item.LinkedIn)) ? "" : $"<a href='{item.LinkedIn}' target='_blank'><i class='bi bi-linkedin'></i></a>";
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "#!" : $"{item.Link}";

                        result_data += $@"<div class='col-xl-3 col-lg-4 col-md-6' data-wow-delay='0.{count}s'>
	                                        <div class='member' data-aos='zoom-in' data-aos-delay='200'>
		                                        <img src='{ProfileImage}' class='img-fluid' alt=''>
		                                        <div class='member-info'>
			                                        <div class='member-info-content'>
				                                        <h4>
                                                            <a href='{DetailsLink}' class='text-decoration-none text-white'>
                                                                {item.FirstName} {item.LastName}
                                                            </a>
                                                        </h4>
				                                        <span>{Title}</span>
			                                        </div>
			                                        <div class='social'>
				                                        {TwitterLink}
				                                        {FacebookLink}
				                                        {InstagramLink}
				                                        {LinkedInLink}
			                                        </div>
		                                        </div>
	                                        </div>
                                        </div>";
                        count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }



        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePortfolio(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Portfolios.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PortfolioTitle = (language == "en") ? item.PortfolioTitle : Helpers.GetTranslatableData(item.PortfolioTitle, language, item.PortfolioID, "Portfolio", "Title");
                        string PortfolioImage = PortfolioHelpers.GetPreviewImage(item.PortfolioID);
                        string PortfolioLink = (string.IsNullOrEmpty(item.Slug)) ? "" : $"<a href='/Portfolio/{item.Slug}' class='details-link' title='More Details'><i class='bx bx-link'></i></a>";


                        result_data += $@"<div class='col-lg-4 col-md-6 portfolio-item'>
	                                        <img src='{PortfolioImage}' class='img-fluid' alt='{PortfolioTitle}'>
	                                        <div class='portfolio-info'>
		                                        <h4>{PortfolioTitle}</h4>
		                                        <p>{item.Category}</p>
		                                        <a href='{PortfolioImage}' data-gallery='portfolioGallery' class='portfolio-lightbox preview-link' title='{PortfolioTitle}'><i class='bx bx-plus'></i></a>
		                                        {PortfolioLink}
	                                        </div>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }



        /// <summary>
        /// Get FAQ with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeFAQ(string language = "en")
        {
            string result_data = "";
            int delay = 100;
            using (var db = new DBConnection())
            {
                var DBQuery = db.FAQs.Where(s => s.Status == 1).OrderByDescending(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Question = (language == "en") ? item.Question : Helpers.GetTranslatableData(item.Question, language, item.FaqID, "FAQ", "Title");
                        string Answer = (language == "en") ? item.Answer : Helpers.GetTranslatableData(item.Answer, language, item.FaqID, "FAQ", "Description");

                        result_data += $@"<div class='row faq-item d-flex align-items-stretch' data-aos='fade-up' data-aos-delay='{delay}'>
	                                        <div class='col-lg-5'>
		                                        <i class='bx bx-help-circle'></i>
		                                        <h4>{Question}</h4>
	                                        </div>
	                                        <div class='col-lg-7'>
		                                        <p>
			                                        {Answer}
		                                        </p>
	                                        </div>
                                        </div>";
                        delay +=100;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get partners with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetPartners(string language = "en") 
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Partners.Where(s => s.Status == 1).OrderBy(x => x.PartnerName);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Name = (language == "en") ? item.PartnerName : Helpers.GetTranslatableData(item.PartnerName, language, item.PartnerID, "Partner", "Title");
                        string NameLink = (string.IsNullOrEmpty(item.Link)) ?  $"<p class='mt-3 text-info'>{Name}</p>" : $"<a href='{item.Link}' target='_blank'><p class='mt-3 text-info'>{Name}</p></a>";
                        result_data += $@"<div class='item'>
                                            <img src='/partners/images/{item.Logo}' height='250' class='rounded w-100' alt='{Name}'>
                                            {NameLink}
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }



        /// <summary>
        /// Get footer services with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetFooterServices(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Services.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string ServiceTitle = (language == "en") ? item.ServiceTitle : Helpers.GetTranslatableData(item.ServiceTitle, language, item.ServiceID, "Services", "Title");
                        string ServiceLink = (!string.IsNullOrEmpty(item.ServiceLink)) ? $"href='{item.ServiceLink}'" : "href='#'";
                        string ShortDescription = Helpers.GetTranslatableData(Helpers.FormatLongText(item.ShortDescription, 250), language, item.ServiceID, "Services", "Description");



                        result_data += $@"<li><i class='bx bx-chevron-right'></i><a {ServiceLink} class=''>{ServiceTitle}</a></li>";
                    }
                }
            }
            return new HtmlString(result_data);
        }

        /// <summary>
        /// Get post images from image gallery
        /// </summary>
        /// <param name="post_id"></param>
        /// <returns></returns>
        public static HtmlString GetPostGalleryImages(string post_id)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.GalleryImages.Where(s => s.ParentID == post_id);
                if (DBQuery.Any())
                {
                    string Directory = Helpers.GetPostImageDirectory(post_id, "Post");

                    foreach (var item in DBQuery)
                    {
                        string portfolio_image = "/posts/images/" + Directory + "/" + item.Image;
                        result_data += $@"<div class='swiper-slide'>
	                                        <img src='{portfolio_image}' alt='Post Image'>
                                        </div>";
                    }

                }
            }
            return new HtmlString(result_data);
        }

        /// <summary>
        /// Get post images from image gallery
        /// </summary>
        /// <param name="page_id"></param>
        /// <returns></returns>
        public static HtmlString GetPageGalleryImages(string page_id)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.GalleryImages.Where(s => s.ParentID == page_id);
                if (DBQuery.Any())
                {
                    string Directory = Helpers.GetPostImageDirectory(page_id, "Page");

                    foreach (var item in DBQuery)
                    {
                        string portfolio_image = "/pages/images/" + Directory + "/" + item.Image;
                        result_data += $@"<div class='swiper-slide'>
	                                        <img src='{portfolio_image}' alt='Page Image'>
                                        </div>";
                    }

                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get post images from image gallery
        /// </summary>
        /// <param name="post_id"></param>
        /// <returns></returns>
        public static HtmlString GetOwlPostGalleryImages(string post_id)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.GalleryImages.Where(s => s.ParentID == post_id);
                if (DBQuery.Any())
                {
                    string Directory = Helpers.GetPostImageDirectory(post_id, "Post");

                    foreach (var item in DBQuery)
                    {
                        string portfolio_image = "/posts/images/" + Directory + "/" + item.Image;
                        result_data += $@"<div class='swiper-slide'>
	                                        <img src='{portfolio_image}' alt='Post Image'>
                                        </div>";
                    }

                }
            }
            return new HtmlString(result_data);
        }


    }




    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Tempo Template Helpers
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class TempoTemplateHelpers
    {
        /// <summary>
        /// Get Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetNavagation(string language = "en", string active_nav = "")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Navigations.Where(s => s.Status == 1 && (s.Parent == null || s.Parent == "")).OrderBy(x => x.Order);

                string set_active = "";

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string NavigationName = (language == "en") ? item.NavigationName : Helpers.GetTranslatableData(item.NavigationName, language, item.NavigationID, "Navigation", "Title");

                        //check if active nav
                        if (!string.IsNullOrEmpty(active_nav))
                        {
                            set_active = (item.NavigationName.ToLower() == active_nav.ToLower()) ? "active" : "";
                        }

                        string url = (item.NavigationLink.Contains("#")) ? $"{AppSettings.BaseUrl(item.NavigationLink)}" : $"{item.NavigationLink}";


                        result_data += $@"<li><a class='nav-link scrollto {set_active}' href='{url}'>{NavigationName}</a></li>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get home sliders with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeSliders(string language = "en")
        {
            string result_data = "";
            int count = 0;
            string active = "active";
            using (var db = new DBConnection())
            {
                var DBQuery = db.HomeSliders.Where(x => x.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        active = (count == 0) ? active : "";

                        //set title and sub text based on if language session passed
                        string SliderTitle = (language == "en") ? item.SliderTitle : Helpers.GetTranslatableData(item.SliderTitle, language, item.SliderID, "HomeSliders", "Title");
                        string SubText = (language == "en") ? item.SubText : Helpers.GetTranslatableData(Helpers.FormatLongText(item.SubText, 150), language, item.SliderID, "HomeSliders", "Description");

                        string read_more = (string.IsNullOrEmpty(item.SliderLink)) ? "" : $"<a href='{item.SliderLink}' class='btn-get-started animate__animated animate__fadeInUp scrollto theme-bg'>Read More</a>";
                        result_data += $@"<div class='carousel-item {active}' style='background-image: url({AppSettings.BaseUrl()}/sliders/images/{item.SliderImage})'>
                                          <div class='carousel-container'>
	                                        <div class='container'>
	                                          <h2 class='animate__animated animate__fadeInDown'>{SliderTitle}</h2>
	                                          <p class='animate__animated animate__fadeInUp'>{SubText}</p>
	                                          {read_more}
	                                        </div>
                                          </div>
                                        </div>";
                        count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get language dropdown
        /// </summary>
        /// <param name="language_session"></param>
        /// <returns></returns>
        public static HtmlString LanguageDropDown(string language_session, string current_url = null)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.SiteLanguages.Where(x => x.Language != language_session).OrderBy(x => x.Language);

                current_url = (!string.IsNullOrEmpty(current_url)) ? current_url : "/Home/SetLanguageSession/";

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        result_data += $@"<li>
	                                        <a class='nav-link scrollto' href='{current_url}?l={item.Language}'>
		                                        <span><span class='flag-icon flag-icon-{Helpers.GetLanguageClass(item.Language)} mr-2'></span>  {Helpers.GetLanguageName(item.Language)}</span>
	                                        </a>
                                        </li>";
                    }
                }
            }
            return new HtmlString(result_data);
        }

        /// <summary>
        /// Get site statistics with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServiceStats(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.ServiceStats.Where(s => s.Status == 1).OrderBy(x => x.Order);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string StatTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.ServiceStatID, "ServiceStats", "Title");
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "" : $"<a href='{item.Link}'>Find out more &raquo;</a>";

                        result_data += $@"<div class='col-lg-3 col-md-6 d-md-flex align-items-md-stretch mb-2'>
	                                            <div class='card w-100'>
                                                    <div class='card-body'>
		                                                <h2 class='text-lead'>
		                                                    <i class='{item.Icon}'></i> {item.StatValue}
                                                        </h2>
		                                                <p><strong>{StatTitle}</strong></p>
		                                                {DetailsLink}
                                                    </div>
	                                            </div>
                                            </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePortfolio(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Portfolios.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PortfolioTitle = (language == "en") ? item.PortfolioTitle : Helpers.GetTranslatableData(item.PortfolioTitle, language, item.PortfolioID, "Portfolio", "Title");
                        string PortfolioImage = PortfolioHelpers.GetPreviewImage(item.PortfolioID);
                        string PortfolioLink = (string.IsNullOrEmpty(item.Slug)) ? "" : $"<a href='/Portfolio/{item.Slug}' class='details-link' title='More Details'><i class='bx bx-link'></i></a>";


                        result_data += $@"<div class='col-lg-4 col-md-6 portfolio-item'>
	                                        <img src='{PortfolioImage}' class='img-fluid' alt='{PortfolioTitle}'>
	                                        <div class='portfolio-info'>
		                                        <h4>{PortfolioTitle}</h4>
		                                        <p>{item.Category}</p>
		                                        <a href='{PortfolioImage}' data-gallery='portfolioGallery' class='portfolio-lightbox preview-link' title='{PortfolioTitle}'><i class='bx bx-plus'></i></a>
		                                        {PortfolioLink}
	                                        </div>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get team with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeTeam(string language = "en")
        {
            string result_data = "";
            int count = 1;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Teams.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Title = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.TeamID, "Team", "Title");
                        string ProfileImage = $@"/team/images/{item.ProfileImage}";
                        string TwitterLink = (string.IsNullOrEmpty(item.Twitter)) ? "" : $"<a href='{item.Twitter}' target='_blank'><i class='bi bi-twitter'></i></a>";
                        string FacebookLink = (string.IsNullOrEmpty(item.Facebook)) ? "" : $"<a href='{item.Facebook}' target='_blank'><i class='bi bi-facebook'></i></a>";
                        string InstagramLink = (string.IsNullOrEmpty(item.Instagram)) ? "" : $"<a href='{item.Instagram}' target='_blank'><i class='bi bi-instagram'></i></a>";
                        string LinkedInLink = (string.IsNullOrEmpty(item.LinkedIn)) ? "" : $"<a href='{item.LinkedIn}' target='_blank'><i class='bi bi-linkedin'></i></a>";
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "#!" : $"{item.Link}";

                        result_data += $@"<div class='col-lg-3 col-md-6 d-flex align-items-stretch mb-2'>
	                                        <div class='member'>
		                                        <div class='member-img'>
			                                        <img src='{ProfileImage}' class='img-fluid' alt='{item.FirstName} {item.LastName}'>
			                                        <div class='social'>
				                                         {TwitterLink}
				                                        {FacebookLink}
				                                        {InstagramLink}
				                                        {LinkedInLink}
			                                        </div>
		                                        </div>
		                                        <div class='member-info'>
			                                        <h4>
				                                        <a href='{DetailsLink}' class='text-decoration-none text-dark'>
					                                        {item.FirstName} {item.LastName}
				                                        </a>
			                                        </h4>
			                                        <span>{Title}</span>
		                                        </div>
	                                        </div>
                                        </div>";
                        count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get FAQ with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeFAQ(string language = "en")
        {
            string result_data = "";
            int Count = 1;
            using (var db = new DBConnection())
            {
                var DBQuery = db.FAQs.Where(s => s.Status == 1).OrderByDescending(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Question = (language == "en") ? item.Question : Helpers.GetTranslatableData(item.Question, language, item.FaqID, "FAQ", "Title");
                        string Answer = (language == "en") ? item.Answer : Helpers.GetTranslatableData(item.Answer, language, item.FaqID, "FAQ", "Description");

                        result_data += $@"<li>
	                                        <div data-bs-toggle='collapse' class='collapsed question' href='#faq{Count}'>{Question} <i class='bi bi-chevron-down icon-show'></i><i class='bi bi-chevron-up icon-close'></i></div>
	                                        <div id='faq{Count}' class='collapse' data-bs-parent='.faq-list'>
		                                        <p>
			                                        {Answer}
		                                        </p>
	                                        </div>
                                        </li>";
                        Count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get partners with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetPartners(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Partners.Where(s => s.Status == 1).OrderBy(x => x.PartnerName);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Name = (language == "en") ? item.PartnerName : Helpers.GetTranslatableData(item.PartnerName, language, item.PartnerID, "Partner", "Title");
                        string NameLink = (string.IsNullOrEmpty(item.Link)) ? $"<p class='mt-3 text-info'>{Name}</p>" : $"<a href='{item.Link}' target='_blank'><p class='mt-3 text-info'>{Name}</p></a>";
                        result_data += $@"<div class='item'>
                                            <img src='/partners/images/{item.Logo}' height='250' class='rounded w-100' alt='{Name}'>
                                            {NameLink}
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get services with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServices(string language = "en")
        {
            string result_data = "";
            int delay = 100;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Services.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string ServiceTitle = (language == "en") ? item.ServiceTitle : Helpers.GetTranslatableData(item.ServiceTitle, language, item.ServiceID, "Services", "Title");
                        string ServiceLink = (string.IsNullOrEmpty(item.ServiceLink)) ? ServiceTitle : $"<a href='{item.ServiceLink}'>{ServiceTitle}</a>";
                        string ShortDescription = Helpers.GetTranslatableData(Helpers.FormatLongText(item.ShortDescription, 250), language, item.ServiceID, "Services", "Description");
                        string ReadMore = (string.IsNullOrEmpty(item.ServiceLink)) ? "" : $"<br/> <a class='btn btn-outline-primary text-center' href='{item.ServiceLink}'>Read More</a>";

                        result_data += $@"<div class='col-md-6 col-lg-3 d-flex align-items-stretch mb-5 mb-lg-0'>
	                                        <div class='icon-box'>
		                                        <div class='icon'><i class='{item.ServiceIcon}'></i></div>
		                                        <h4 class='title'>{ServiceLink}</h4>
		                                        <p class='description'>
			                                        {ShortDescription}
		                                        </p>
                                                {ReadMore}
	                                        </div>
                                        </div>";
                        delay += 100;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get testimonials with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetTestimonials(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Testimonials.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Title = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.TestimonialID, "Testimonials", "Title");
                        string Testimonial = (language == "en") ? item.Testimonial : Helpers.GetTranslatableData(item.Testimonial, language, item.TestimonialID, "Testimonials", "Description");

                        result_data += $@"<div class='col-lg-4'>
	                                        <div class='card mb-3' style='max-width: 600px; min-height: 28em;'>
	                                          <div class='row g-0'>
		                                        <div class='col-md-4'>
		                                          <img src='/testimonials/images/{item.ProfileImage}' style='width:8.5em;' alt='{item.Name}'>
		                                        </div>
		                                        <div class='col-md-8'>
		                                          <div class='card-body'>
			                                        <h3 class='card-title'>{item.Name}</h3>
                                                    <p class='fw-bold'>{Title} - {item.Organization}</p>
			                                        <p class='card-text'>
				                                        {Testimonial}
			                                        </p>
		                                          </div>
		                                        </div>
	                                          </div>
	                                        </div>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePosts(string language = "en", int total = 3)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Posts.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PostTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.PostID, "Posts", "Title");
                        string PostImage = Helpers.GetPostImage(item.PostID, "Post");

                        result_data += $@"<div class='col-md-4 mb-2'>
	                                        <article class='entry'>
	                                            <div class='entry-img'>
                                                    <a href='Post/{item.Slug}'>
		                                                <img src='{PostImage}' alt='{Helpers.FormatLongText(PostTitle, 50)}' class='img-fluid w-100' style='min-height: 16em;'>
		                                            </a>
	                                            </div>
	                                            <h2 class='entry-title'>
		                                            <a href='Post/{item.Slug}' class='text-dark'>
			                                            {Helpers.FormatLongText(PostTitle, 50)}
		                                            </a>
	                                            </h2>
	                                            <div class='entry-meta'>
	                                            </div>
	                                            <div class='entry-content'>
		                                            <p>
			                                            {Helpers.FormatLongText(item.ShortDescription, 125)}
		                                            </p>
		                                            <div class='read-more'>
			                                            <a href='Post/{item.Slug}'>Read More</a>
		                                            </div>
	                                            </div>
                                            </article>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetFooterNavagation(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.FooterNavigations.Where(s => s.Status == 1).OrderBy(x => x.Order);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string NavigationName = (language == "en") ? item.NavigationName : Helpers.GetTranslatableData(item.NavigationName, language, item.NavigationID, "FooterNavigation", "Title");

                        string url = (item.NavigationLink.Contains("#")) ? $"{AppSettings.BaseUrl(item.NavigationLink)}" : $"{item.NavigationLink}";

                        result_data += $@"<li><i class='bx bx-chevron-right'></i> <a href='{url}'>{NavigationName}</a></li>";
                    }
                }
            }

            //add site map
            result_data += $@"<li><i class='bx bx-chevron-right'></i><a href='/Sitemap'>Sitemap</a></li>";

            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get footer services with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetFooterServices(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Services.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string ServiceTitle = (language == "en") ? item.ServiceTitle : Helpers.GetTranslatableData(item.ServiceTitle, language, item.ServiceID, "Services", "Title");
                        string ServiceLink = (!string.IsNullOrEmpty(item.ServiceLink)) ? $"href='{item.ServiceLink}'" : "href='#'";
                        string ShortDescription = Helpers.GetTranslatableData(Helpers.FormatLongText(item.ShortDescription, 250), language, item.ServiceID, "Services", "Description");



                        result_data += $@"<li><i class='bx bx-chevron-right'></i><a {ServiceLink}>{ServiceTitle}</a></li>";
                    }
                }
            }
            return new HtmlString(result_data);
        }



        /// <summary>
        /// Get post images from image gallery
        /// </summary>
        /// <param name="post_id"></param>
        /// <returns></returns>
        public static HtmlString GetPostGalleryImages(string post_id)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.GalleryImages.Where(s => s.ParentID == post_id);
                if (DBQuery.Any())
                {
                    string Directory = Helpers.GetPostImageDirectory(post_id, "Post");

                    foreach (var item in DBQuery)
                    {
                        string portfolio_image = "/posts/images/" + Directory + "/" + item.Image;
                        result_data += $@"<div class='swiper-slide'>
	                                        <img src='{portfolio_image}' alt='Post Image'>
                                        </div>";
                    }

                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get post images from image gallery
        /// </summary>
        /// <param name="page_id"></param>
        /// <returns></returns>
        public static HtmlString GetPageGalleryImages(string page_id)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.GalleryImages.Where(s => s.ParentID == page_id);
                if (DBQuery.Any())
                {
                    string Directory = Helpers.GetPostImageDirectory(page_id, "Page");

                    foreach (var item in DBQuery)
                    {
                        string portfolio_image = "/pages/images/" + Directory + "/" + item.Image;
                        result_data += $@"<div class='swiper-slide'>
	                                        <img src='{portfolio_image}' alt='Page Image'>
                                        </div>";
                    }

                }
            }
            return new HtmlString(result_data);
        }
    }



    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// MyBiz Template Helpers
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class MyBizTemplateHelpers
    {
        /// <summary>
        /// Get Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetNavagation(string language = "en", string active_nav = "")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Navigations.Where(s => s.Status == 1 && (s.Parent == null || s.Parent == "")).OrderBy(x => x.Order);

                string set_active = "";

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string NavigationName = (language == "en") ? item.NavigationName : Helpers.GetTranslatableData(item.NavigationName, language, item.NavigationID, "Navigation", "Title");

                        //check if active nav
                        if (!string.IsNullOrEmpty(active_nav))
                        {
                            set_active = (item.NavigationName.ToLower() == active_nav.ToLower()) ? "active" : "";
                        }

                        string url = (item.NavigationLink.Contains("#")) ? $"{AppSettings.BaseUrl(item.NavigationLink)}" : $"{item.NavigationLink}";


                        result_data += $@"<li><a class='nav-link scrollto {set_active}' href='{url}'>{NavigationName}</a></li>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get home sliders with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeSliders(string language = "en")
        {
            string result_data = "";
            int count = 0;
            string active = "active";
            using (var db = new DBConnection())
            {
                var DBQuery = db.HomeSliders.Where(x => x.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        active = (count == 0) ? active : "";

                        //set title and sub text based on if language session passed
                        string SliderTitle = (language == "en") ? item.SliderTitle : Helpers.GetTranslatableData(item.SliderTitle, language, item.SliderID, "HomeSliders", "Title");
                        string SubText = (language == "en") ? item.SubText : Helpers.GetTranslatableData(Helpers.FormatLongText(item.SubText, 150), language, item.SliderID, "HomeSliders", "Description");

                        string read_more = (string.IsNullOrEmpty(item.SliderLink)) ? "" : $"<a href='{item.SliderLink}' class='btn-get-started animate__animated animate__fadeInUp scrollto'>Read More</a>";
                        result_data += $@"<div class='carousel-item {active}' style='background-image: url({AppSettings.BaseUrl()}/sliders/images/{item.SliderImage});'>
	                                        <div class='carousel-container'>
		                                        <div class='carousel-content'>
			                                        <h2 class='animate__animated animate__fadeInDown'><span>{SliderTitle}</span></h2>
			                                        <p class='animate__animated animate__fadeInUp'>
				                                        {SubText}
			                                        </p>
			                                        {read_more}
		                                        </div>
	                                        </div>
                                        </div>";
                        count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }



        /// <summary>
        /// Get site statistics with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServiceStats(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.ServiceStats.Where(s => s.Status == 1).OrderBy(x => x.Order);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string StatTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.ServiceStatID, "ServiceStats", "Title");
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "" : $"<a href='{item.Link}'>Find out more &raquo;</a>";

                        result_data += $@"<div class='col-lg-3 col-6 text-center mb-2'>
	                                            <i class='{item.Icon} fa-2x text-white'></i>
	                                            <span data-purecounter-start='0' data-purecounter-end='{item.StatValue}' data-purecounter-duration='1' class='purecounter'></span>
	                                            <p>{StatTitle}</p>
	                                            {DetailsLink}
                                            </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }



        /// <summary>
        /// Get Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetFooterNavagation(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.FooterNavigations.Where(s => s.Status == 1).OrderBy(x => x.Order);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string NavigationName = (language == "en") ? item.NavigationName : Helpers.GetTranslatableData(item.NavigationName, language, item.NavigationID, "FooterNavigation", "Title");

                        string url = (item.NavigationLink.Contains("#")) ? $"{AppSettings.BaseUrl(item.NavigationLink)}" : $"{item.NavigationLink}";

                        result_data += $@"<li><i class='bx bx-chevron-right'></i> <a href='{url}'>{NavigationName}</a></li>";
                    }
                }
            }

            //add site map
            result_data += $@"<li><i class='bx bx-chevron-right'></i><a href='/Sitemap'>Sitemap</a></li>";

            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get partners with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetPartners(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Partners.Where(s => s.Status == 1).OrderBy(x => x.PartnerName);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Name = (language == "en") ? item.PartnerName : Helpers.GetTranslatableData(item.PartnerName, language, item.PartnerID, "Partner", "Title");
                        string NameLink = (string.IsNullOrEmpty(item.Link)) ? $"<p class='mt-3 text-info'>{Name}</p>" : $"<a href='{item.Link}' target='_blank'><p class='mt-3 text-info'>{Name}</p></a>";
                        result_data += $@"<div class='item'>
                                            <img src='/partners/images/{item.Logo}' height='250' class='rounded w-100' alt='{Name}'>
                                            {NameLink}
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get services with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServices(string language = "en")
        {
            string result_data = "";
            int count = 0;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Services.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string ServiceTitle = (language == "en") ? item.ServiceTitle : Helpers.GetTranslatableData(item.ServiceTitle, language, item.ServiceID, "Services", "Title");
                        string ServiceLink = (string.IsNullOrEmpty(item.ServiceLink)) ? ServiceTitle : $"<a href='{item.ServiceLink}'>{ServiceTitle}</a>";
                        string ShortDescription = Helpers.GetTranslatableData(Helpers.FormatLongText(item.ShortDescription, 250), language, item.ServiceID, "Services", "Description");
                        string MarginClass = (count == 0) ? "mt-4 mt-md-0" : "";
                        string ReadMore = (string.IsNullOrEmpty(item.ServiceLink)) ? "" : $"<br/> <a class='btn btn-outline-primary text-center' href='{item.ServiceLink}'>Read More</a>";

                        result_data += $@"<div class='col-md-6 {MarginClass}'>
	                                        <div class='icon-box' style='min-height: 13em;'>
	                                          <i class='{item.ServiceIcon}'></i>
	                                          <h4>{ServiceLink}</h4>
	                                          <p>
		                                        {ShortDescription}
	                                          </p>
                                              {ReadMore}
	                                        </div>
                                        </div>";
                        count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }



        /// <summary>
        /// Get testimonials with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetTestimonials(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Testimonials.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Title = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.TestimonialID, "Testimonials", "Title");
                        string Testimonial = (language == "en") ? item.Testimonial : Helpers.GetTranslatableData(item.Testimonial, language, item.TestimonialID, "Testimonials", "Description");

                        result_data += $@"<div class='swiper-slide'>
                                              <div class='testimonial-item'>
	                                            <img src='assets/img/testimonials/testimonials-2.jpg' class='testimonial-img' alt=''>
	                                            <h3>{item.Name}</h3>
	                                            <h4>{Title} - {item.Organization}</h4>
	                                            <p>
	                                              <i class='bx bxs-quote-alt-left quote-icon-left'></i>
	                                              {Testimonial}
	                                              <i class='bx bxs-quote-alt-right quote-icon-right'></i>
	                                            </p>
                                              </div>
                                            </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get team with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeTeam(string language = "en")
        {
            string result_data = "";
            int count = 1;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Teams.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Title = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.TeamID, "Team", "Title");
                        string ProfileImage = $@"/team/images/{item.ProfileImage}";
                        string TwitterLink = (string.IsNullOrEmpty(item.Twitter)) ? "" : $"<a href='{item.Twitter}' target='_blank'><i class='bi bi-twitter'></i></a>";
                        string FacebookLink = (string.IsNullOrEmpty(item.Facebook)) ? "" : $"<a href='{item.Facebook}' target='_blank'><i class='bi bi-facebook'></i></a>";
                        string InstagramLink = (string.IsNullOrEmpty(item.Instagram)) ? "" : $"<a href='{item.Instagram}' target='_blank'><i class='bi bi-instagram'></i></a>";
                        string LinkedInLink = (string.IsNullOrEmpty(item.LinkedIn)) ? "" : $"<a href='{item.LinkedIn}' target='_blank'><i class='bi bi-linkedin'></i></a>";
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "#!" : $"{item.Link}";

                        result_data += $@"<div class='col-lg-6 mb-2'>
	                                        <div class='member d-flex align-items-start'>
		                                        <div class='pic'>
			                                        <img src='{ProfileImage}' class='img-fluid' alt='{item.FirstName} {item.LastName}'>
		                                        </div>
		                                        <div class='member-info'>
			                                        <h4>
				                                        <a href='{DetailsLink}' class='text-decoration-none text-dark'>
					                                        {item.FirstName} {item.LastName}
				                                        </a>
			                                        </h4>
			                                        <span>{Title}</span>
			                                        <div class='social'>
				                                        {TwitterLink}
				                                        {FacebookLink}
				                                        {InstagramLink}
				                                        {LinkedInLink}
			                                        </div>
		                                        </div>
	                                        </div>
                                        </div>";
                        count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePosts(string language = "en", int total = 3)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Posts.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PostTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.PostID, "Posts", "Title");
                        string PostImage = Helpers.GetPostImage(item.PostID, "Post");

                        result_data += $@"<div class='col-md-4 mb-2'>
	                                        <div class='card' style='min-height: 28rem;'>
                                                 <a href='Post/{item.Slug}'>
		                                          <img src='{PostImage}' class='card-img-top' style='max-height: 15rem;' alt='{Helpers.FormatLongText(PostTitle, 50)}'>
		                                        </a>
		                                        <div class='card-body'>
			                                        <h5 class='card-title'>
                                                        <a href='Post/{item.Slug}' class='text-dark text-decoration-none'>
					                                        {Helpers.FormatLongText(PostTitle, 50)}
                                                        </a>
			                                        </h5>
				                                        <p class='card-text'>
                                                            {Helpers.FormatLongText(item.ShortDescription, 125)}
				                                        </p>
				                                        <a href='Post/{item.Slug}' class='btn btn-light'>Read More</a>
		                                        </div>
	                                        </div>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePortfolio(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Portfolios.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PortfolioTitle = (language == "en") ? item.PortfolioTitle : Helpers.GetTranslatableData(item.PortfolioTitle, language, item.PortfolioID, "Portfolio", "Title");
                        string PortfolioImage = PortfolioHelpers.GetPreviewImage(item.PortfolioID);
                        string PortfolioLink = (string.IsNullOrEmpty(item.Slug)) ? "" : $"<a href='/Portfolio/{item.Slug}' title='More Details'><i class='ri-links-fill'></i></a>";


                        result_data += $@"<div class='col-lg-4 col-md-6 portfolio-item'>
	                                            <div class='portfolio-wrap'>
		                                            <img src='{PortfolioImage}' class='img-fluid' alt='{PortfolioTitle}'>
		                                            <div class='portfolio-info'>
			                                            <h4>{PortfolioTitle}</h4>
			                                            <p>{item.Category}</p>
			                                            <div class='portfolio-links'>
				                                            <a href='{PortfolioImage}' data-gallery='portfolioGallery' class='portfolio-lightbox' title='{PortfolioTitle}'><i class='ri-add-fill'></i></a>
				                                            {PortfolioLink}
			                                            </div>
		                                            </div>
	                                            </div>
                                            </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get FAQ with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeFAQ(string language = "en")
        {
            string result_data = "";
            int Count = 1;
            using (var db = new DBConnection())
            {
                var DBQuery = db.FAQs.Where(s => s.Status == 1).OrderByDescending(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Question = (language == "en") ? item.Question : Helpers.GetTranslatableData(item.Question, language, item.FaqID, "FAQ", "Title");
                        string Answer = (language == "en") ? item.Answer : Helpers.GetTranslatableData(item.Answer, language, item.FaqID, "FAQ", "Description");

                        result_data += $@"<div class='col-lg-6 mb-3'>
	                                            <h4>
		                                            <a class='text-decoration-none' data-bs-toggle='collapse' href='#faq{Count}' role='button' aria-expanded='false' aria-controls='faq{Count}'>
			                                            <i class='bi bi-caret-down-fill mr-1'></i>
			                                            <span class='text-dark'>
				                                            {Question}
			                                            </span>
		                                            </a>
	                                            </h4>
	                                            <div class='collapse' id='faq{Count}'>
		                                            <div class='card card-body'>
			                                            {Answer}
		                                            </div>
	                                            </div>
                                            </div>";
                        Count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }
    }


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Mamba Template Helpers
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class MambaTemplateHelpers
    {
        /// <summary>
        /// Get Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetNavagation(string language = "en", string active_nav = "")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Navigations.Where(s => s.Status == 1 && (s.Parent == null || s.Parent == "")).OrderBy(x => x.Order);

                string set_active = "";

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string NavigationName = (language == "en") ? item.NavigationName : Helpers.GetTranslatableData(item.NavigationName, language, item.NavigationID, "Navigation", "Title");

                        //check if active nav
                        if (!string.IsNullOrEmpty(active_nav))
                        {
                            set_active = (item.NavigationName.ToLower() == active_nav.ToLower()) ? "active" : "";
                        }

                        string url = (item.NavigationLink.Contains("#")) ? $"{AppSettings.BaseUrl(item.NavigationLink)}" : $"{item.NavigationLink}";


                        result_data += $@"<li><a class='nav-link scrollto {set_active}' href='{url}'>{NavigationName}</a></li>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get home sliders with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeSliders(string language = "en")
        {
            string result_data = "";
            int count = 0;
            string active = "active";
            using (var db = new DBConnection())
            {
                var DBQuery = db.HomeSliders.Where(x => x.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        active = (count == 0) ? active : "";

                        //set title and sub text based on if language session passed
                        string SliderTitle = (language == "en") ? item.SliderTitle : Helpers.GetTranslatableData(item.SliderTitle, language, item.SliderID, "HomeSliders", "Title");
                        string SubText = (language == "en") ? item.SubText : Helpers.GetTranslatableData(Helpers.FormatLongText(item.SubText, 150), language, item.SliderID, "HomeSliders", "Description");

                        string read_more = (string.IsNullOrEmpty(item.SliderLink)) ? "" : $"<a href='{item.SliderLink}' class='btn-get-started animate__animated animate__fadeInUp scrollto theme-bg'>Read More</a>";
                        result_data += $@"<div class='carousel-item {active}' style='background-image: url({AppSettings.BaseUrl()}/sliders/images/{item.SliderImage});'>
	                                        <div class='carousel-container'>
		                                        <div class='carousel-content container'>
		                                          <h2 class='animate__animated animate__fadeInDown'>{SliderTitle}</h2>
		                                          <p class='animate__animated animate__fadeInUp'>{SubText}</p>
		                                          {read_more}
		                                        </div>
	                                        </div>
                                        </div>";
                        count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get site statistics with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServiceStats(string language = "en")
        {
            string result_data = "";
            int data_delay = 0;
            using (var db = new DBConnection())
            {
                var DBQuery = db.ServiceStats.Where(s => s.Status == 1).OrderBy(x => x.Order);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string StatTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.ServiceStatID, "ServiceStats", "Title");
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "" : $"<a href='{item.Link}'>Find out more &raquo;</a>";

                        result_data += $@"<div class='col-lg-3 col-md-6 text-center mb-2' data-aos='fade-up' data-aos-delay='{data_delay}'>
	                                            <div class='count-box'>
		                                            <i class='{item.Icon}'></i>
		                                            <span data-purecounter-start='0' data-purecounter-end='{item.StatValue}' data-purecounter-duration='1' class='purecounter'></span>
		                                            <p>{StatTitle}</p>
		                                            {DetailsLink}
	                                            </div>
                                            </div>";
                        data_delay += 200;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetFooterNavagation(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.FooterNavigations.Where(s => s.Status == 1).OrderBy(x => x.Order);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string NavigationName = (language == "en") ? item.NavigationName : Helpers.GetTranslatableData(item.NavigationName, language, item.NavigationID, "FooterNavigation", "Title");

                        string url = (item.NavigationLink.Contains("#")) ? $"{AppSettings.BaseUrl(item.NavigationLink)}" : $"{item.NavigationLink}";

                        result_data += $@"<li><i class='bx bx-chevron-right'></i> <a href='{url}'>{NavigationName}</a></li>";
                    }
                }
            }

            //add site map
            result_data += $@"<li><i class='bx bx-chevron-right'></i><a href='/Sitemap'>Sitemap</a></li>";

            return new HtmlString(result_data);
        }



        /// <summary>
        /// Get partners with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetPartners(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get services with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServices(string language = "en")
        {
            string result_data = "";
            int delay = 0;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Services.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string ServiceTitle = (language == "en") ? item.ServiceTitle : Helpers.GetTranslatableData(item.ServiceTitle, language, item.ServiceID, "Services", "Title");
                        string ServiceLink = (string.IsNullOrEmpty(item.ServiceLink)) ? ServiceTitle : $"<a href='{item.ServiceLink}'>{ServiceTitle}</a>";
                        string ShortDescription = Helpers.GetTranslatableData(Helpers.FormatLongText(item.ShortDescription, 250), language, item.ServiceID, "Services", "Description");
                        string ReadMore = (string.IsNullOrEmpty(item.ServiceLink)) ? "" : $"<br/> <a class='btn btn-outline-primary text-center' href='{item.ServiceLink}'>Read More</a>";

                        result_data += $@"<div class='col-lg-4 col-md-6 icon-box mb-4' data-aos='fade-up' data-aos-delay='{delay}'>
	                                            <div class='icon'><i class='{item.ServiceIcon}'></i></div>
	                                            <h4 class='title'>{ServiceLink}</h4>
	                                            <p class='description'>{ShortDescription}</p>
                                                {ReadMore}
                                            </div>";
                        delay += 100;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get testimonials with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetTestimonials(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get team with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeTeam(string language = "en")
        {
            string result_data = "";
            int delay = 0;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Teams.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Title = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.TeamID, "Team", "Title");
                        string ProfileImage = $@"/team/images/{item.ProfileImage}";
                        string TwitterLink = (string.IsNullOrEmpty(item.Twitter)) ? "" : $"<a href='{item.Twitter}' target='_blank'><i class='bi bi-twitter'></i></a>";
                        string FacebookLink = (string.IsNullOrEmpty(item.Facebook)) ? "" : $"<a href='{item.Facebook}' target='_blank'><i class='bi bi-facebook'></i></a>";
                        string InstagramLink = (string.IsNullOrEmpty(item.Instagram)) ? "" : $"<a href='{item.Instagram}' target='_blank'><i class='bi bi-instagram'></i></a>";
                        string LinkedInLink = (string.IsNullOrEmpty(item.LinkedIn)) ? "" : $"<a href='{item.LinkedIn}' target='_blank'><i class='bi bi-linkedin'></i></a>";
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "#!" : $"{item.Link}";

                        result_data += $@"<div class='col-xl-3 col-lg-4 col-md-6 mb-2' data-aos='fade-up' data-aos-delay='{delay}'>
	                                            <div class='member'>
	                                              <div class='pic'><img src='{ProfileImage}' class='img-fluid' alt='{item.FirstName} {item.LastName}'></div>
	                                              <div class='member-info'>
		                                            <h4>
			                                            <a href='{DetailsLink}' class='text-decoration-none text-dark'>
				                                            {item.FirstName} {item.LastName}
			                                            </a>
		                                            </h4>
		                                            <span>{Title}</span>
		                                            <div class='social'>
			                                            {TwitterLink}
			                                            {FacebookLink}
			                                            {InstagramLink}
			                                            {LinkedInLink}
		                                            </div>
	                                              </div>
	                                            </div>
                                              </div>";
                        delay += 100;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePosts(string language = "en", int total = 3)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Posts.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PostTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.PostID, "Posts", "Title");
                        string PostImage = Helpers.GetPostImage(item.PostID, "Post");

                        result_data += $@"<div class='col-lg-4 col-md-6 content-item mb-2' data-aos='fade-up'>
	                                        <span>
		                                        <a href='Post/{item.Slug}'>
		                                            <img src='{PostImage}' class='img-fluid' alt='{Helpers.FormatLongText(PostTitle, 50)}'>
		                                        </a>
	                                        </span>
	                                        <h4>
		                                        <a href='Post/{item.Slug}' class='text-dark text-decoration-none'>
			                                        {Helpers.FormatLongText(PostTitle, 50)}
		                                        </a>
	                                        </h4>
	                                        <p>{Helpers.FormatLongText(item.ShortDescription, 125)}</p>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePortfolio(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Portfolios.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PortfolioTitle = (language == "en") ? item.PortfolioTitle : Helpers.GetTranslatableData(item.PortfolioTitle, language, item.PortfolioID, "Portfolio", "Title");
                        string PortfolioImage = PortfolioHelpers.GetPreviewImage(item.PortfolioID);
                        string PortfolioLink = (string.IsNullOrEmpty(item.Slug)) ? "" : $"<a href='/Portfolio/{item.Slug}' class='details-link' title='More Details'><i class='bx bx-link'></i></a>";


                        result_data += $@"<div class='col-lg-4 col-md-6 portfolio-item filter-card'>
	                                        <div class='portfolio-wrap'>
		                                        <img src='{PortfolioImage}' class='img-fluid' alt='{PortfolioTitle}'>
		                                        <div class='portfolio-info'>
			                                        <h4>{PortfolioTitle}</h4>
			                                        <p>{item.Category}</p>
			                                        <div class='portfolio-links'>
				                                        <a href='{PortfolioImage}' data-galleryery='portfolioGallery' class='portfolio-lightbox' title='{PortfolioTitle}'><i class='bi bi-plus'></i></a>
				                                        {PortfolioLink}
			                                        </div>
		                                        </div>
	                                        </div>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get FAQ with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeFAQ(string language = "en")
        {
            string result_data = "";
            int delay = 0;
            using (var db = new DBConnection())
            {
                var DBQuery = db.FAQs.Where(s => s.Status == 1).OrderByDescending(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Question = (language == "en") ? item.Question : Helpers.GetTranslatableData(item.Question, language, item.FaqID, "FAQ", "Title");
                        string Answer = (language == "en") ? item.Answer : Helpers.GetTranslatableData(item.Answer, language, item.FaqID, "FAQ", "Description");

                        result_data += $@"<div class='col-lg-6 faq-item' data-aos='fade-up' data-aos-delay='{delay}'>
	                                            <h4>{Question}</h4>
	                                            <p>
		                                            {Answer}
	                                            </p>
                                            </div>";
                        delay += 100;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get footer services with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetFooterServices(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Services.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string ServiceTitle = (language == "en") ? item.ServiceTitle : Helpers.GetTranslatableData(item.ServiceTitle, language, item.ServiceID, "Services", "Title");
                        string ServiceLink = (!string.IsNullOrEmpty(item.ServiceLink)) ? $"href='{item.ServiceLink}'" : "href='#'";
                        string ShortDescription = Helpers.GetTranslatableData(Helpers.FormatLongText(item.ShortDescription, 250), language, item.ServiceID, "Services", "Description");



                        result_data += $@"<li><i class='bx bx-chevron-right'></i><a {ServiceLink}>{ServiceTitle}</a></li>";
                    }
                }
            }
            return new HtmlString(result_data);
        }
    }


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Hidayah Template Helpers
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class HidayahTemplateHelpers
    {

        /// <summary>
        /// Get site statistics with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServiceStats(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.ServiceStats.Where(s => s.Status == 1).OrderBy(x => x.Order);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string StatTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.ServiceStatID, "ServiceStats", "Title");
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "" : $"<a href='{item.Link}'>Find out more &raquo;</a>";

                        result_data += $@"<div class='col-lg-3 col-6 text-center'>
	                                            <i class='{item.Icon} stat-icon'></i>
	                                            <span data-purecounter-start='0' data-purecounter-end='{item.StatValue}' data-purecounter-duration='1' class='purecounter'></span>
	                                            <p><strong>{StatTitle}</strong></p>
	                                            {DetailsLink}
                                            </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get services with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServices(string language = "en")
        {
            string result_data = "";
            int delay = 100;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Services.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string ServiceTitle = (language == "en") ? item.ServiceTitle : Helpers.GetTranslatableData(item.ServiceTitle, language, item.ServiceID, "Services", "Title");
                        string ServiceLink = (string.IsNullOrEmpty(item.ServiceLink)) ? ServiceTitle : $"<a href='{item.ServiceLink}'>{ServiceTitle}</a>";
                        string ShortDescription = Helpers.GetTranslatableData(Helpers.FormatLongText(item.ShortDescription, 250), language, item.ServiceID, "Services", "Description");
                        string ReadMore = (string.IsNullOrEmpty(item.ServiceLink)) ? "" : $"<br/> <a class='btn btn-outline-primary text-center' href='{item.ServiceLink}'>Read More</a>";

                        result_data += $@"<div class='col-lg-4 col-md-6 icon-box'>
	                                            <div class='icon'><i class='{item.ServiceIcon}'></i></div>
	                                            <h4 class='title'>{ServiceLink}</h4>
	                                            <p class='description'>
		                                            {ShortDescription}
	                                            </p>
	                                            {ReadMore}
                                            </div>";
                        delay += 100;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get testimonials with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetTestimonials(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Testimonials.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Title = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.TestimonialID, "Testimonials", "Title");
                        string Testimonial = (language == "en") ? item.Testimonial : Helpers.GetTranslatableData(item.Testimonial, language, item.TestimonialID, "Testimonials", "Description");

                        result_data += $@"<div class='col-lg-6 mb-3'>
	                                            <div class='testimonial-item' style='min-height: 13em;'>
		                                            <img src='/testimonials/images/{item.ProfileImage}' class='testimonial-img' alt='{item.Name}'>
		                                            <h3>{item.Name}</h3>
		                                            <h4>{Title} - {item.Organization}</h4>
		                                            <p>
			                                            <i class='bx bxs-quote-alt-left quote-icon-left'></i>
			                                            {Testimonial}
			                                            <i class='bx bxs-quote-alt-right quote-icon-right'></i>
		                                            </p>
	                                            </div>
                                            </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get team with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeTeam(string language = "en")
        {
            string result_data = "";
            int count = 1;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Teams.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Title = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.TeamID, "Team", "Title");
                        string ProfileImage = $@"/team/images/{item.ProfileImage}";
                        string TwitterLink = (string.IsNullOrEmpty(item.Twitter)) ? "" : $"<a href='{item.Twitter}' target='_blank'><i class='bi bi-twitter'></i></a>";
                        string FacebookLink = (string.IsNullOrEmpty(item.Facebook)) ? "" : $"<a href='{item.Facebook}' target='_blank'><i class='bi bi-facebook'></i></a>";
                        string InstagramLink = (string.IsNullOrEmpty(item.Instagram)) ? "" : $"<a href='{item.Instagram}' target='_blank'><i class='bi bi-instagram'></i></a>";
                        string LinkedInLink = (string.IsNullOrEmpty(item.LinkedIn)) ? "" : $"<a href='{item.LinkedIn}' target='_blank'><i class='bi bi-linkedin'></i></a>";
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "#!" : $"{item.Link}";

                        result_data += $@"<div class='col-xl-3 col-lg-4 col-md-6' data-wow-delay='0.{count}s'>
	                                            <div class='member'>
		                                            <img src='{ProfileImage}' class='img-fluid' alt='{item.FirstName} {item.LastName}'>
		                                            <div class='member-info'>
			                                            <div class='member-info-content'>
				                                            <h4>
					                                            <a href='{DetailsLink}' class='text-decoration-none text-white'>
						                                            {item.FirstName} {item.LastName}
					                                            </a>
				                                            </h4>
				                                            <span>{Title}</span>
			                                            </div>
			                                            <div class='social'>
				                                            {TwitterLink}
				                                            {FacebookLink}
				                                            {InstagramLink}
				                                            {LinkedInLink}
			                                            </div>
		                                            </div>
	                                            </div>
                                            </div>";
                        count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePosts(string language = "en", int total = 3)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Posts.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PostTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.PostID, "Posts", "Title");
                        string PostImage = Helpers.GetPostImage(item.PostID, "Post");

                        result_data += $@"<div class='col-sm-12 col-md-6 col-lg-4 mb-2'>
	                                            <div class='card'>
	                                            <a href='Post/{item.Slug}'>
	                                              <img src='{PostImage}' class='card-img-top' alt='{Helpers.FormatLongText(PostTitle, 50)}'>
	                                            </a>
		                                            <div class='card-body'>
			                                            <h5 class='card-title'>
				                                            <a href='Post/{item.Slug}' class='text-dark text-decoration-none'>
					                                            All children deserve a fair chance in life.
				                                            </a>
			                                            </h5>
			                                            <p class='card-text'>
				                                            {Helpers.FormatLongText(item.ShortDescription, 125)}
			                                            </p>
			                                            <a href='Post/{item.Slug}' class='btn btn-outline-primary'>
				                                            Read More
			                                            </a>
		                                            </div>
	                                            </div>
                                            </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePortfolio(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Portfolios.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PortfolioTitle = (language == "en") ? item.PortfolioTitle : Helpers.GetTranslatableData(item.PortfolioTitle, language, item.PortfolioID, "Portfolio", "Title");
                        string PortfolioImage = PortfolioHelpers.GetPreviewImage(item.PortfolioID);
                        string PortfolioLink = (string.IsNullOrEmpty(item.Slug)) ? "" : $"<a href='/Portfolio/{item.Slug}' class='details-link' title='More Details'><i class='bx bx-link'></i></a>";


                        result_data += $@"<div class='col-lg-4 col-md-6 portfolio-item'>
	                                            <div class='portfolio-wrap'>
		                                            <img src='{PortfolioImage}' class='img-fluid' alt=''>
		                                            <div class='portfolio-info'>
			                                            <h4>{PortfolioTitle}</h4>
		                                                <p>{item.Category}</p>
			                                            <div class='portfolio-links'>
				                                            <a href='{PortfolioImage}' data-gallery='portfolioGallery' class='portfolio-lightbox' title='{PortfolioTitle}'><i class='bx bx-plus'></i></a>
				                                            {PortfolioLink}
			                                            </div>
		                                            </div>
	                                            </div>
                                            </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get FAQ with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeFAQ(string language = "en")
        {
            string result_data = "";
            int Count = 1;
            using (var db = new DBConnection())
            {
                var DBQuery = db.FAQs.Where(s => s.Status == 1).OrderByDescending(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Question = (language == "en") ? item.Question : Helpers.GetTranslatableData(item.Question, language, item.FaqID, "FAQ", "Title");
                        string Answer = (language == "en") ? item.Answer : Helpers.GetTranslatableData(item.Answer, language, item.FaqID, "FAQ", "Description");

                        result_data += $@"<li class='list-group-item'>
	                                            <h4>
		                                            <a class='text-decoration-none text-dark' data-bs-toggle='collapse' href='#faq{Count}' role='button' aria-expanded='false' aria-controls='faq{Count}'>
			                                            <small><i class='bi bi-caret-down-fill mr-1'></i></small>
			                                            {Question}
		                                            </a>
	                                            </h4>
	                                            <div class='collapse mb-3' id='faq{Count}'>
		                                            {Answer}
	                                            </div>
                                            </li>";
                        Count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }
    }


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Shuffle Template Helpers
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class ShuffleTemplateHelpers
    {
        /// <summary>
        /// Get Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetNavagation(string language = "en", string active_nav = "")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Navigations.Where(s => s.Status == 1 && (s.Parent == null || s.Parent == "")).OrderBy(x => x.Order);

                string set_active = "";

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set name based on if language session passed
                        string NavigationName = (language == "en") ? item.NavigationName : Helpers.GetTranslatableData(item.NavigationName, language, item.NavigationID, "Navigation", "Title");

                        //check if active nav
                        if (!string.IsNullOrEmpty(active_nav))
                        {
                            set_active = (item.NavigationName.ToLower() == active_nav.ToLower()) ? "active" : "";
                        }

                        string url = (item.NavigationLink.Contains("#")) ? $"{AppSettings.BaseUrl(item.NavigationLink)}" : $"{item.NavigationLink}";


                        result_data += $@"<li><a class='nav-link scrollto {set_active}' href='{url}'>{NavigationName}</a></li>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get home sliders with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeSliders(string language = "en")
        {
            string result_data = "";
            int count = 0;
            string active = "active";
            using (var db = new DBConnection())
            {
                var DBQuery = db.HomeSliders.Where(x => x.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        active = (count == 0) ? active : "";

                        //set title and sub text based on if language session passed
                        string SliderTitle = (language == "en") ? item.SliderTitle : Helpers.GetTranslatableData(item.SliderTitle, language, item.SliderID, "HomeSliders", "Title");
                        string SubText = (language == "en") ? item.SubText : Helpers.GetTranslatableData(Helpers.FormatLongText(item.SubText, 150), language, item.SliderID, "HomeSliders", "Description");

                        string read_more = (string.IsNullOrEmpty(item.SliderLink)) ? "" : $"<a href='{item.SliderLink}' class='btn-get-started animate__animated animate__fadeInUp scrollto theme-bg'>Read More</a>";
                        result_data += $@"<div class='carousel-item {active}'>
	                                            <div class='carousel-background'><img src='{AppSettings.BaseUrl()}/sliders/images/{item.SliderImage}' class='w-100' alt='{SliderTitle}'></div>
	                                            <div class='carousel-container'>
	                                              <div class='carousel-content'>
		                                            <h2 class='animate__animated animate__fadeInDown'>{SliderTitle}</h2>
		                                            <p class='animate__animated animate__fadeInUp'>{SubText}</p>
		                                            {read_more}
	                                              </div>
	                                            </div>
                                              </div>";
                        count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get site statistics with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServiceStats(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.ServiceStats.Where(s => s.Status == 1).OrderBy(x => x.Order);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string StatTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.ServiceStatID, "ServiceStats", "Title");
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "" : $"<a href='{item.Link}'>Find out more &raquo;</a>";

                        result_data += $@"<div class='col-lg-3 col-md-6 d-md-flex align-items-md-stretch'>
	                                        <div class='count-box'>
	                                            <i class='{item.Icon}'></i>
	                                            <span data-purecounter-start='0' data-purecounter-end='{item.StatValue}' data-purecounter-duration='1' class='purecounter'></span>
	                                            <p>{StatTitle}</p>
	                                            {DetailsLink}
	                                        </div>
                                            </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetFooterNavagation(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }

            //add site map
            result_data += $@"<li><i class='bx bx-chevron-right'></i><a href='/Sitemap'>Sitemap</a></li>";

            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get partners with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetPartners(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get services with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServices(string language = "en")
        {
            string result_data = "";
            int delay = 100;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Services.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string ServiceTitle = (language == "en") ? item.ServiceTitle : Helpers.GetTranslatableData(item.ServiceTitle, language, item.ServiceID, "Services", "Title");
                        string ServiceLink = (string.IsNullOrEmpty(item.ServiceLink)) ? ServiceTitle : $"<a href='{item.ServiceLink}'>{ServiceTitle}</a>";
                        string ShortDescription = Helpers.GetTranslatableData(Helpers.FormatLongText(item.ShortDescription, 250), language, item.ServiceID, "Services", "Description");
                        string ReadMore = (string.IsNullOrEmpty(item.ServiceLink)) ? "" : $"<br/> <a class='btn btn-outline-primary text-center' href='{item.ServiceLink}'>Read More</a>";

                        result_data += $@"<div class='col-md-6 col-lg-3 d-flex align-items-stretch mb-5 mb-lg-2'>
	                                        <div class='icon-box w-100'>
	                                          <div class='icon'><i class='{item.ServiceIcon}'></i></div>
	                                          <h4 class='title'>{ServiceLink}</h4>
	                                          <p class='description'>{ShortDescription}</p>
	                                          {ReadMore}
	                                        </div>
                                          </div>";
                        delay += 100;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get testimonials with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetTestimonials(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get team with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeTeam(string language = "en")
        {
            string result_data = "";
            int count = 1;
            using (var db = new DBConnection())
            {
                var DBQuery = db.Teams.Where(s => s.Status == 1).OrderBy(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Title = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.TeamID, "Team", "Title");
                        string ProfileImage = $@"/team/images/{item.ProfileImage}";
                        string TwitterLink = (string.IsNullOrEmpty(item.Twitter)) ? "" : $"<a href='{item.Twitter}' target='_blank'><i class='bi bi-twitter'></i></a>";
                        string FacebookLink = (string.IsNullOrEmpty(item.Facebook)) ? "" : $"<a href='{item.Facebook}' target='_blank'><i class='bi bi-facebook'></i></a>";
                        string InstagramLink = (string.IsNullOrEmpty(item.Instagram)) ? "" : $"<a href='{item.Instagram}' target='_blank'><i class='bi bi-instagram'></i></a>";
                        string LinkedInLink = (string.IsNullOrEmpty(item.LinkedIn)) ? "" : $"<a href='{item.LinkedIn}' target='_blank'><i class='bi bi-linkedin'></i></a>";
                        string DetailsLink = (string.IsNullOrEmpty(item.Link)) ? "#!" : $"{item.Link}";

                        result_data += $@"<div class='col-xl-3 col-lg-4 col-md-6'>
	                                            <div class='member'>
		                                            <img src='{ProfileImage}' class='img-fluid' alt='{item.FirstName} {item.LastName}'>
		                                            <div class='member-info'>
			                                            <div class='member-info-content'>
				                                            <h4>
					                                            <a href='{DetailsLink}' class='text-decoration-none text-white'>
						                                            {item.FirstName} {item.LastName}
					                                            </a>
				                                            </h4>
				                                            <span>{Title}</span>
			                                            </div>
			                                            <div class='social'>
				                                            {TwitterLink}
				                                            {FacebookLink}
				                                            {InstagramLink}
				                                            {LinkedInLink}
			                                            </div>
		                                            </div>
	                                            </div>
                                            </div>";
                        count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePosts(string language = "en", int total = 3)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Posts.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PostTitle = (language == "en") ? item.Title : Helpers.GetTranslatableData(item.Title, language, item.PostID, "Posts", "Title");
                        string PostImage = Helpers.GetPostImage(item.PostID, "Post");

                        result_data += $@"<div class='col-lg-4 col-md-6 d-flex align-items-stretch mb-5 mb-lg-2'>
                                            <div class='card'>
						                        <a href='Post/{item.Slug}'>
							                        <img src='{PostImage}' class='card-img-top' alt='{Helpers.FormatLongText(PostTitle, 50)}'>
						                        </a>
                                                <div class='card-body'>
                                                    <h5 class='card-title'>
								                        <a href='Post/{item.Slug}'>
									                        {Helpers.FormatLongText(PostTitle, 50)}
								                        </a>
							                        </h5>
                                                    <p class='card-text'>
								                        {Helpers.FormatLongText(item.ShortDescription, 125)}
							                        </p>
                                                    <a href='Post/{item.Slug}' class='btn'>Read more</a>
                                                </div>
                                            </div>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePortfolio(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.Portfolios.Where(s => s.Status == 1).OrderByDescending(x => x.ID).Take(total);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string PortfolioTitle = (language == "en") ? item.PortfolioTitle : Helpers.GetTranslatableData(item.PortfolioTitle, language, item.PortfolioID, "Portfolio", "Title");
                        string PortfolioImage = PortfolioHelpers.GetPreviewImage(item.PortfolioID);
                        string PortfolioLink = (string.IsNullOrEmpty(item.Slug)) ? "" : $"<a href='/Portfolio/{item.Slug}' class='details-link' title='More Details'><i class='bx bx-link'></i></a>";


                        result_data += $@"<div class='col-lg-4 col-md-6 portfolio-item filter-app'>
	                                        <div class='portfolio-wrap'>
	                                          <img src='{PortfolioImage}' class='img-fluid' alt='{PortfolioTitle}'>
	                                          <div class='portfolio-info'>
		                                        <h4>{PortfolioTitle}</h4>
		                                        <p>{item.Category}</p>
	                                          </div>
	                                          <div class='portfolio-links'>
		                                        <a href='{PortfolioImage}' data-gallery='portfolioGallery' class='portfolio-lightbox' title='{PortfolioTitle}'><i class='bx bx-plus'></i></a>
		                                        {PortfolioLink}
	                                          </div>
	                                        </div>
                                        </div>";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get FAQ with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeFAQ(string language = "en")
        {
            string result_data = "";
            int Count = 1;
            using (var db = new DBConnection())
            {
                var DBQuery = db.FAQs.Where(s => s.Status == 1).OrderByDescending(x => x.ID);

                if (DBQuery.Any())
                {
                    foreach (var item in DBQuery)
                    {
                        //set data based on if language session passed
                        string Question = (language == "en") ? item.Question : Helpers.GetTranslatableData(item.Question, language, item.FaqID, "FAQ", "Title");
                        string Answer = (language == "en") ? item.Answer : Helpers.GetTranslatableData(item.Answer, language, item.FaqID, "FAQ", "Description");

                        result_data += $@"<li>
                                              <a data-bs-toggle='collapse' class='collapsed' data-bs-target='#accordion-list-{Count}'><span>{Count}</span> {Question} <i class='bx bx-chevron-down icon-show'></i><i class='bx bx-chevron-up icon-close'></i></a>
                                              <div id='accordion-list-{Count}' class='collapse' data-bs-parent='.accordion-list'>
	                                            <p>
	                                              {Answer}
	                                            </p>
                                              </div>
                                            </li>";
                        Count++;
                    }
                }
            }
            return new HtmlString(result_data);
        }
    }


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Groovin Template Helpers
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class GroovinTemplateHelpers
    {
        /// <summary>
        /// Get Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetNavagation(string language = "en", string active_nav = "")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get home sliders with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeSliders(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get site statistics with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServiceStats(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get Navigations with translatable names
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetFooterNavagation(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }

            //add site map
            result_data += $@"<li><i class='bx bx-chevron-right'></i><a href='/Sitemap'>Sitemap</a></li>";

            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get partners with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetPartners(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get services with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetServices(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get testimonials with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetTestimonials(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get team with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeTeam(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePosts(string language = "en", int total = 3)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get posts with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomePortfolio(string language = "en", int total = 9)
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }


        /// <summary>
        /// Get FAQ with translatable data
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static HtmlString GetHomeFAQ(string language = "en")
        {
            string result_data = "";
            using (var db = new DBConnection())
            {

            }
            return new HtmlString(result_data);
        }
    }




}
