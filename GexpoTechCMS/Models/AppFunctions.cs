using GexpoTechCMS.Models.AppModels;
using LazZiya.ImageResize;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NgoExpoApp.Models
{
    public class AppFunctions
    {
        public static string _connectionString = string.Empty;
        public static string _baseUrl = string.Empty;
        public AppFunctions()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();

            _connectionString = root.GetSection("ConnectionStrings").GetSection("DBConnection").Value;
            _baseUrl = root.GetSection("systemConfiguration").GetSection("baseUrl").Value;
        }


        /// <summary>
        /// Gets base url
        /// </summary>
        /// <returns></returns>
        public static string BaseUrl()
        {
            return _baseUrl;
        }

        /// <summary>
        /// Gets db connection string
        /// </summary>
        /// <returns></returns>
        public static string DBConnection()
        {
            return _connectionString;
        }


        /// <summary>
        /// checks if user is logged in
        /// </summary>
        public bool IsLoggedIn(string session_key)
        {
            if (!string.IsNullOrEmpty(session_key))
            {
                using (var db = new DBConnection())
                {
                    var DBQuery = db.Accounts.Where(s => s.SessionKey == session_key);

                    if (DBQuery.Any())
                    {
                        //is logged in
                        return true;
                    }
                }
            }

            return false;
        }


        //Generate unique id
        /// <summary>
        /// Generate unique directory name for new account created, takes in user email
        /// </summary>
        /// <returns>unique directory name</returns>
        public string GenerateDirectoryName(string user_email)
        {
            if (user_email.Contains("@"))
            {
                user_email = user_email.Split('@')[0];
            }
            return (user_email + RandomString(8)).ToLower();
        }


        /// <summary>
        ///  Converts string to integer, returns zero if fails
        /// </summary>
        /// <param name="string_number"></param>
        /// <returns>integer</returns>
        public int Int32Parse(string string_number)
        {
            if (!string.IsNullOrEmpty(string_number))
            {
                try
                {
                    return Int32.Parse(string_number); ;
                }
                catch (FormatException)
                {
                    return 0;
                }
            }
            return 0;
        }

        /// <summary>
        /// Converts string to integer, returns deafult if fails
        /// </summary>
        /// <param name="string_number"></param>
        /// <param name="return_default"></param>
        /// <returns>integer</returns>
        public int Int32Parse(string string_number, int return_default)
        {
            if (!string.IsNullOrEmpty(string_number))
            {
                try
                {
                    return Int32.Parse(string_number); ;
                }
                catch (FormatException)
                {
                    return return_default;
                }
            }
            return return_default;
        }


        /// <summary>
        /// Converts string to integer, returns false if fails
        /// </summary>
        /// <param name="string_boolean"></param>
        /// <returns></returns>
        public bool BooleanParse(string string_boolean)
        {
            if (!string.IsNullOrEmpty(string_boolean))
            {
                try
                {
                    return Convert.ToBoolean(string_boolean);
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Converts string to integer, returns deafult if fails
        /// </summary>
        /// <param name="string_boolean"></param>
        /// <param name="return_default"></param>
        /// <returns></returns>
        public bool BooleanParse(string string_boolean, bool return_default)
        {
            if (!string.IsNullOrEmpty(string_boolean))
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
            return false;
        }


        //Generates unique alphanumeric strings
        /// <summary>
        /// Generate unique alphanumeric strings
        /// </summary>
        /// <returns>unique string</returns>
        public string GetUinqueId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            var FormNumber = BitConverter.ToUInt32(buffer, 0) ^ BitConverter.ToUInt32(buffer, 4) ^ BitConverter.ToUInt32(buffer, 8) ^ BitConverter.ToUInt32(buffer, 12);
            return FormNumber.ToString("X");
        }


        //Generates GUID
        /// <summary>
        /// Generating GUID using the Guid.NewGuid() method
        /// </summary>
        /// <returns>GUID as string</returns>
        public string GetGuid()
        {
            Guid obj = Guid.NewGuid();
            return obj.ToString();
        }

        //Generates random alphanumeric strings
        /// <summary>
        /// Take in the length, and type of random text then generates random string. Default is alphanumeric string.
        /// </summary>
        /// <returns>alphanumeric string</returns>
        private static Random Rand = new Random();
        public string RandomString(int length, string format)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            if (!string.IsNullOrEmpty(format))
            {
                switch (format)
                {
                    case "Text":
                        chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                        break;
                    case "Numbers":
                        chars = "0123456789";
                        break;
                    default:
                        chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                        break;
                }
            }
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Rand.Next(s.Length)]).ToArray());
        }

        //Generates random alphanumeric strings
        /// <summary>
        /// Take in the length and generates random alphanumeric string
        /// </summary>
        /// <returns>alphanumeric string</returns>
        private static Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //Password match check
        /// <summary>
        /// Checks if the passwords passed are the same
        /// </summary>
        /// <returns>boolean</returns>
        public bool PasswordsMatch(string password_one, string password_two)
        {
            if (password_one.Equals(password_two))
            {
                return true;
            }
            return false;
        }


        //Generate unique post permalink
        /// <summary>
        /// Generate unique permalink for new post created, takes in post title text
        /// </summary>
        /// <returns>unique permalink</returns>
        public string GenerateSlug(string title, string post_type)
        {
            //First to lower case
            title = title.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(title);
            title = Encoding.ASCII.GetString(bytes);

            //Replace spaces
            title = Regex.Replace(title, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars
            title = Regex.Replace(title, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            //Trim dashes from end
            title = title.Trim('-', '_');

            //Replace double occurences of - or _
            title = Regex.Replace(title, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            //check if permalink too long
            if (title.Length > 60)
            {
                title = title.Substring(0, 50) +"-"+ GetUinqueId().ToLower();
            }

            //check if post permalink exists
            using (var db = new DBConnection())
            {

                if (post_type == "Post")
                {
                    if (db.Posts.Any(s => s.Slug == title))
                    {
                        //add randon text to permalink
                        title = (title.Replace(' ', '-') + "-" + GetUinqueId()).ToLower();
                    }
                }
                else if (post_type == "Page")
                {
                    if (db.Pages.Any(s => s.Slug == title))
                    {
                        //add randon text to permalink
                        title = (title.Replace(' ', '-') + "-" + GetUinqueId()).ToLower();
                    }
                }
                else if (post_type == "Portfolio")
                {
                    if (db.Portfolios.Any(s => s.Slug == title))
                    {
                        //add randon text to permalink
                        title = (title.Replace(' ', '-') + "-" + GetUinqueId()).ToLower();
                    }
                }
                else
                {
                    if (db.Posts.Any(s => s.Slug == title))
                    {
                        //add randon text to permalink
                        title = (title.Replace(' ', '-') + "-" + GetUinqueId()).ToLower();
                    }
                }
            }

            return title;
        }


        //Return post data
        /// <summary>
        /// Get specific post data. Takes post id and data to return
        /// </summary>
        /// <returns>post data</returns>
        public string GetPostData(string post_id, string return_data)
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
                                return (db.Posts.Any(s => s.PostID == post_id && s.ImageCaption != null)) ? db.Posts.Where(s => s.ImageCaption == post_id).FirstOrDefault().ImageCaption : "";
                            case "FeaturedPost":
                                return (db.Posts.Any(s => s.PostID == post_id)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().FeaturedPost.ToString() : "false";
                            case "Content":
                                return (db.Posts.Any(s => s.PostID == post_id && s.Content != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().Content : "";
                            case "PostTags":
                                return (db.Posts.Any(s => s.PostID == post_id && s.PostTags != null)) ? db.Posts.Where(s => s.PostID == post_id).FirstOrDefault().PostTags : "";
                            case "Status":
                                return (db.Accounts.Any(s => s.AccountID == post_id)) ? db.Accounts.Where(s => s.AccountID == post_id).FirstOrDefault().ID.ToString() : "0";
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



        //Return page data
        /// <summary>
        /// Get specific page data. Takes page id and data to return
        /// </summary>
        /// <returns>page data</returns>
        public string GetPageData(string page_id, string return_data)
        {
            try
            {
                if (!string.IsNullOrEmpty(page_id))
                {
                    using (var db = new DBConnection())
                    {
                        switch (return_data)
                        {
                            case "ID":
                                return (db.Pages.Any(s => s.PageID == page_id)) ? db.Pages.Where(s => s.PageID == page_id).FirstOrDefault().ID.ToString() : "0";
                            case "Slug":
                                return (db.Pages.Any(s => s.PageID == page_id && s.Slug != null)) ? db.Pages.Where(s => s.PageID == page_id).FirstOrDefault().Slug : "";
                            case "Author":
                                return (db.Pages.Any(s => s.PageID == page_id && s.Author != null)) ? db.Pages.Where(s => s.PageID == page_id).FirstOrDefault().Author : "";
                            case "Title":
                                return (db.Pages.Any(s => s.PageID == page_id && s.Title != null)) ? db.Pages.Where(s => s.PageID == page_id).FirstOrDefault().Title : "";
                            case "ShortDescription":
                                return (db.Pages.Any(s => s.PageID == page_id && s.ShortDescription != null)) ? db.Pages.Where(s => s.PageID == page_id).FirstOrDefault().ShortDescription : "";
                            case "PageImage":
                                return (db.Pages.Any(s => s.PageID == page_id && s.PageImage != null)) ? db.Pages.Where(s => s.PageID == page_id).FirstOrDefault().PageImage : "";
                            case "ImageCaption":
                                return (db.Pages.Any(s => s.PageID == page_id && s.ImageCaption != null)) ? db.Pages.Where(s => s.ImageCaption == page_id).FirstOrDefault().ImageCaption : "";
                           case "Content":
                                return (db.Pages.Any(s => s.PageID == page_id && s.Content != null)) ? db.Pages.Where(s => s.PageID == page_id).FirstOrDefault().Content : "";
                            case "Status":
                                return (db.Accounts.Any(s => s.AccountID == page_id)) ? db.Accounts.Where(s => s.AccountID == page_id).FirstOrDefault().ID.ToString() : "0";
                            case "UpdatedAt":
                                return (db.Pages.Any(s => s.PageID == page_id && s.UpdatedAt != null)) ? db.Pages.Where(s => s.PageID == page_id).FirstOrDefault().UpdatedAt.ToString() : "";
                            case "CreatedAt":
                                return (db.Pages.Any(s => s.PageID == page_id && s.CreatedAt != null)) ? db.Pages.Where(s => s.PageID == page_id).FirstOrDefault().CreatedAt.ToString() : "";
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




        /// <summary>
        ///get specific config data. Takes key and data to return
        /// </summary>
        public string GetConfigurationsData(string unique_key)
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
        public string GetConfigurationsData(string unique_key, string default_value)
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
        public HtmlString GetConfigurationsHtmlData(string unique_key)
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
        ///get specific app message data. Takes key and data to return
        /// </summary>
        public string GetAppMessage(string unique_key, string default_value)
        {
            try
            {
                if (!string.IsNullOrEmpty(unique_key))
                {
                    using (var db = new DBConnection())
                    {
                        var DBQuery = db.AppMessages.Where(s => s.MessageKey == unique_key);
                        if (DBQuery.Any())
                        {
                            return DBQuery.FirstOrDefault().MessageValue;
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
        /// Logs activity
        /// </summary>
        public bool LogActivity(string activity_by, string activity_type, string activity)
        {
            using (var db = new DBConnection())
            {
                ActivityLogModel activity_data = new ActivityLogModel
                {
                    ActivityID = GetGuid(),
                    ActivityBy = activity_by,
                    ActivityType = activity_type,
                    Activity = activity,
                    CreatedAt = DateTime.Now
                };

                // Add the new object to the collection.
                db.ActivityLogs.Add(activity_data);

                // Submit the change to the database.
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return false;
        }


        /// <summary>
        ///removes html tags from text
        /// </summary>
        public string StripHTML(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = Regex.Replace(text, "<.*?>", "");
            }
            return text;
        }


        //Return account data
        /// <summary>
        /// Get specific account data from session key. Takes session key and data to return
        /// </summary>
        /// <returns>account data</returns>
        public string GetSessionAccountData(string session_key, string return_data)
        {
            try
            {
                if (!string.IsNullOrEmpty(session_key))
                {
                    using (var db = new DBConnection())
                    {
                        switch (return_data)
                        {
                            case "ID":
                                return (db.Accounts.Any(s => s.SessionKey == session_key)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().ID.ToString() : "0";
                            case "AccountID":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.AccountID != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().AccountID : "";
                            case "FirstName":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.FirstName != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().FirstName : "";
                            case "LastName":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.LastName != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().LastName : "";
                            case "FullName":
                                return db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().FirstName + " " + db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().LastName;
                            case "Email":
                                return db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().FirstName + " " + db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().LastName;
                            case "Description":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.Description != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().Description : "";
                            case "Gender":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.Gender != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().Gender : "";
                            case "Password":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.Password != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().Password : "";
                            case "ProfilePicture":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.ProfilePicture != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().ProfilePicture : "";
                            case "Active":
                                return (db.Accounts.Any(s => s.SessionKey == session_key)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().Active.ToString() : "false";
                            case "Oauth":
                                return (db.Accounts.Any(s => s.SessionKey == session_key)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().Oauth.ToString() : "false";
                            case "EmailVerification":
                                return (db.Accounts.Any(s => s.SessionKey == session_key)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().EmailVerification.ToString() : "false";
                            case "EmailNotification":
                                return (db.Accounts.Any(s => s.SessionKey == session_key)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().EmailNotification.ToString() : "false";
                            case "DirectoryName":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.DirectoryName != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().DirectoryName : "";
                            case "Country":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.Country != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().Country : "";
                            case "Address":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.Address != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().Address : "";
                            case "UpdatedAt":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.UpdatedAt != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().UpdatedAt.ToString() : "";
                            case "CreatedAt":
                                return (db.Accounts.Any(s => s.SessionKey == session_key && s.CreatedAt != null)) ? db.Accounts.Where(s => s.SessionKey == session_key).FirstOrDefault().CreatedAt.ToString() : "";
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


        //Return account data
        /// <summary>
        /// Get specific account data. Takes account id and data to return
        /// </summary>
        /// <returns>account data</returns>
        public string GetAccountData(string account_id, string return_data)
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
                                return db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().FirstName + " " + db.Accounts.Where(s => s.AccountID == account_id).FirstOrDefault().LastName;
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
            AppFunctions functions = new AppFunctions();

            string profile_picture = functions.GetAccountData(account_id, "ProfilePicture");
            string base_url = BaseUrl();

            if (!string.IsNullOrEmpty(profile_picture))
            {
                string directory_name = functions.GetAccountData(account_id, "DirectoryName");
                return base_url + "/admin/images/accounts/" + directory_name + "/" + profile_picture;
            }

            //return default
            return base_url + "/" + AppSettings.DefaultProfilePicture();
        }


        public HtmlString GetRecentActivities(string account_id, int skip, int total)
        {
            var result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.ActivityLogs.Where(s => s.ActivityBy == account_id).OrderByDescending(x => x.ID).Skip(skip).Take(total);
                if (DBQuery.Any())
                {
                   foreach(var activity in DBQuery)
                    {
                        result_data += $@"<div class='d-flex align-items-start'>
	                                        <img src='{GetAccountProfilePicture(activity.ActivityBy)}' width='36' height='36' class='rounded-circle mr-2' alt='{GetAccountData(activity.ActivityBy, "FullName")}'>
	                                            <div class='flex-grow-1'>
		                                            <small class='float-right text-navy'>{GetTimeSince(activity.CreatedAt)} ago</small>
		                                            <strong>{GetAccountData(activity.ActivityBy, "FullName")}</strong> <span class='text-info'>{activity.Activity}</span><br />
                                                  </div>
                                            </div>
                                         <hr/>
                                        ";
                    }
                }
            }
            return new HtmlString(result_data);
        }


        // return how much time passed since date object
        public string GetTimeSince(DateTime? objDateTime)
        {
            DateTime newDateObject = (DateTime)((objDateTime == null) ? DateTime.Now : objDateTime);
            // here we are going to subtract the passed in DateTime from the current time converted to UTC
            TimeSpan ts = DateTime.Now.ToUniversalTime().Subtract(newDateObject);
            int intDays = ts.Days;
            int intHours = ts.Hours;
            int intMinutes = ts.Minutes;
            int intSeconds = ts.Seconds;

            if (intDays > 0)
                return string.Format("{0} days", intDays);

            if (intHours > 0)
                return string.Format("{0} hours", intHours);

            if (intMinutes > 0)
                return string.Format("{0} minutes", intMinutes);

            if (intSeconds > 0)
                return string.Format("{0} seconds", intSeconds);

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

        //Get List Of Countries
        /// <summary>
        /// Get the list of all countries
        /// </summary>
        /// <returns>list of countries</returns>
        public List<CountryModel> GetCountryList()
        {
            using (var db = new DBConnection())
            {
                List<CountryModel> country_list = new List<CountryModel>();

                //-- Get data from db --//
                country_list = db.Countries.OrderBy(s => s.Name).ToList();

                return country_list;
            }
        }


        /// <summary>
        /// Upload image(s) to the specified directory.
        /// </summary>
        /// <param name="ImageFile"></param>
        /// <param name="upload_max"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="watermark"></param>
        /// <param name="save_path"></param>
        /// <returns></returns>
        public string UploadImage(List<IFormFile> ImageFile, int? upload_max, int? width, int? height, string watermark, string save_path)
        {
            upload_max = (upload_max == null) ? 1: upload_max;
            int new_width = (width == null) ? 150 : (int)width;
            int new_height = (height == null) ? 150 : (int)height;
            watermark = (string.IsNullOrEmpty(watermark)) ? "" : watermark;

            string FileName = "";
            int count = 0;
            try
            {
                //create directory if not exist
                Directory.CreateDirectory(save_path);

                foreach (var file in ImageFile)
                {
                    if (count < upload_max)
                    {
                        if (file.Length > 0)
                        {
                            using (var stream = file.OpenReadStream())
                            {
                                using (var img = Image.FromStream(stream))
                                {
                                    FileName = RandomNumber() + "-" + file.FileName;

                                    img.ScaleAndCrop(new_width, new_height)
                                         .AddTextWatermark(watermark)
                                         .SaveAs($"{save_path}\\{FileName}");
                                    count++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //TODO log error
            }
            return FileName;
        }



        /// <summary>
        /// Uploads image as it is to the specified directory
        /// </summary>
        /// <param name="ImageFile"></param>
        /// <param name="upload_max"></param>
        /// <param name="save_path"></param>
        /// <returns></returns>
        public string UploadImage(List<IFormFile> ImageFile, int? upload_max, string save_path)
        {
            upload_max = (upload_max == null) ? 1 : upload_max;

            string FileName = "";
            int count = 0;
            try
            {
                //create directory if not exist
                Directory.CreateDirectory(save_path);

                foreach (var file in ImageFile)
                {
                    if (count < upload_max)
                    {
                        if (file.Length > 0)
                        {
                            using (var stream = file.OpenReadStream())
                            {
                                using (var img = Image.FromStream(stream))
                                {
                                    FileName = RandomNumber() + "-" + file.FileName;

                                    img.SaveAs($"{save_path}\\{FileName}");
                                    count++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //TODO log error
            }
            return FileName;
        }



        /// <summary>
        /// Upload image(s) to the specified directory and saves image name to specified model
        /// </summary>
        /// <param name="ImageFile"></param>
        /// <param name="upload_max"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="watermark"></param>
        /// <param name="save_path"></param>
        /// <param name="model_name"></param>
        /// <param name="model_key_name"></param>
        /// <param name="model_key_value"></param>
        /// <returns></returns>
        public string UploadImages(List<IFormFile> ImageFile, int? upload_max, int? width, int? height, string watermark, string save_path, string parent_id)
        {
            upload_max = (upload_max == null) ? 1 : upload_max;
            int new_width = (width == null) ? 150 : (int)width;
            int new_height = (height == null) ? 150 : (int)height;
            watermark = (string.IsNullOrEmpty(watermark)) ? "" : watermark;

            string FileName = "";
            int count = 0;
            try
            {
                //create directory if not exist
                Directory.CreateDirectory(save_path);

                foreach (var file in ImageFile)
                {
                    if (count < upload_max)
                    {
                        if (file.Length > 0)
                        {
                            using (var stream = file.OpenReadStream())
                            {
                                using (var img = Image.FromStream(stream))
                                {
                                    FileName = RandomNumber() + "-" + file.FileName;

                                    img.ScaleAndCrop(new_width, new_height)
                                         .AddTextWatermark(watermark)
                                         .SaveAs($"{save_path}\\{FileName}");

                                    //add uploaded image name to table
                                    AddGalleryImage(parent_id, FileName);

                                    count++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //TODO log error
            }
            return FileName;
        }


        /// <summary>
        /// Add gallery image links with relational parent id
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="image_link"></param>
        /// <returns></returns>
        public bool AddGalleryImage(string parent_id, string image_link)
        {
            using (var db = new DBConnection())
            {
                // Create GalleryImages object.
                GalleryImageModel images_data = new GalleryImageModel
                {
                    ParentID = parent_id,
                    Image = image_link,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Add object to the GalleryImages collection.
                db.GalleryImages.Add(images_data);

                // Submit the change to the database.
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    //TODO log error
                    Console.WriteLine(ex);

                    return false;
                }
            }
        }


        /// <summary>
        /// Upload video(s) to the specified directory.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="upload_max"></param>
        /// <param name="save_path"></param>
        /// <returns></returns>
        public string UploadVideo(List<IFormFile> files, int? upload_max, string save_path)
        {
            upload_max = (upload_max == null) ? 1 : upload_max;
            string FileName = "";
            int count = 0;
            try
            {
                long size = files.Sum(f => f.Length);

                var filePaths = new List<string>();
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0 && count < upload_max)
                    {
                        //Full file path
                        var SavePath = Path.Combine(save_path);
                        try
                        {
                            Directory.CreateDirectory(SavePath);
                        }
                        catch (Exception ex)
                        {
                            // handle them here
                            Console.WriteLine(ex);
                        }

                        //file infp
                        var Name = formFile.Name;
                        FileName = RandomNumber() + "-" + formFile.FileName; //generate random file name here
                        var FileContentType = formFile.ContentType;
                        var FileLength = formFile.Length;


                        // full path to file in temp location
                        var filePath = SavePath;
                        filePaths.Add(filePath);
                        using (var stream = new FileStream(Path.Combine(SavePath, FileName), FileMode.Create))
                        {
                            formFile.CopyToAsync(stream);
                        }
                    }

                    count++;
                }
            }
            catch (Exception)
            {
                //TODO log error
            }
            return FileName;
        }


        /// <summary>
        /// Upload document the specified directory. Uploads 1st if multiple documents passed
        /// </summary>
        /// <param name="files"></param>
        /// <param name="save_path"></param>
        /// <returns></returns>
        public string UploadSingleDocument(List<IFormFile> files, string save_path)
        {
            string FileName = "";
            int count = 0;
            try
            {
                long size = files.Sum(f => f.Length);

                var filePaths = new List<string>();
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0 && count == 0)
                    {
                        //Full file path
                        var SavePath = Path.Combine(save_path);
                        try
                        {
                            Directory.CreateDirectory(SavePath);
                        }
                        catch (Exception ex)
                        {
                            // handle them here
                            Console.WriteLine(ex);
                        }

                        //file infp
                        var Name = formFile.Name;
                        var FileExt = Path.GetExtension(formFile.FileName);
                        FileName = RandomNumber() + "_" + formFile.FileName.Replace(" ", "_"); //generate random file name here
                        var FileContentType = formFile.ContentType;
                        var FileLength = formFile.Length;

                        // full path to file in temp location
                        var filePath = SavePath;
                        filePaths.Add(filePath);
                        using (var stream = new FileStream(Path.Combine(SavePath, FileName), FileMode.Create))
                        {
                            formFile.CopyToAsync(stream);
                        }

                        //save document 
                    }

                    count++;
                }
            }
            catch (Exception)
            {
                //TODO log error
            }
            return FileName;
        }


        /// Random number
        /// <summary>
        /// Generates random
        /// </summary>
        /// <returns>random integer</returns>
        public int RandomNumber()
        {
            var random = new Random();
            int randomnumber = random.Next();
            return randomnumber;
        }


        //Validate required inputs 
        /// <summary>
        /// Validates array of inputs for not null
        /// </summary>
        /// <returns>boolean</returns>
        public bool ValidateInputs(string[] inputs)
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


        //Convert Text Case
        /// <summary>
        /// Convert Text Case to the desired format passed as parameter
        /// </summary>
        /// <returns>string</returns>
        public string ConvertCase(string text, string convert_to)
        {
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


        //Add record into table
        /// <summary>
        /// Adds new recored into entity table passed
        /// </summary>
        /// <returns>boolean</returns>
        public bool AddTableData(string model_name, string entry_column, string entry_value)
        {

            try
            {
                string connection_string = DBConnection();
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        //Insert record to Users db
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"INSERT INTO [" + model_name + "] ([" + entry_column + "]) VALUES (@value)";
                        cmd.Parameters.AddWithValue("@value", ((object)entry_value) ?? DBNull.Value);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (connection != null)
                        {
                            //cleanup connection i.e close 
                            connection.Close();
                        }

                        if (rowsAffected == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //TODO Log Error
            }

            return false;
        }


        //Update Data Model Record
        /// <summary>
        /// Updates a column value of the data entity passed
        /// </summary>
        /// <returns>boolean</returns>
        public bool UpdateTableData(string model_name, string pk_name, string pk_value, string update_column, string update_value)
        {
            try
            {
                string connection_string = DBConnection();
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    string DBQuery = $"Update [" + model_name + "] SET [" + update_column + "] = @update_value Where [" + pk_name + "] = '" + pk_value + "' ";
                    using (SqlCommand command = new SqlCommand(DBQuery, connection))
                    {
                        command.Parameters.AddWithValue("@update_value", update_value);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //TODO Provide for exceptions.
            }
            return false;
        }


        //gete table column value base on the key passed
        public string GetTableData(string model_name, string pk_name, string pk_value, string return_data)
        {
            string[] ValidationInputs = { model_name, pk_name, pk_value, return_data };
            if (!ValidateInputs(ValidationInputs))
            {
                return "";
            }

            string return_value = "";

            var DBQuery = @"SELECT [" + return_data + "] FROM [" + model_name + "] WHERE [" + pk_name + "]  = @key";
            try
            {
                string connection_string = DBConnection();

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


        //Delete table record
        /// <summary>
        /// Delete table record(s) base on the key passed
        /// </summary>
        /// <returns>boolean</returns>
        public bool DeleteTableData(string model_name, string pk_name, string pk_value)
        {
            var MsgCountQuery = @"DELETE FROM [" + model_name + "] WHERE [" + pk_name + "]  = @key";
            try
            {
                string connection_string = DBConnection();
                using (var con = new SqlConnection(connection_string))
                {
                    con.Open();
                    var cmd = new SqlCommand(MsgCountQuery, con);
                    cmd.Parameters.AddWithValue("@key", pk_value);
                    if (cmd.ExecuteScalar() != DBNull.Value)
                    {
                        //if model has orphaned data, remove orphan data
                        if (ModelHasOrphans(model_name) && !string.IsNullOrEmpty(ModelOrphans(model_name)))
                        {
                            string model_names = ModelOrphans(model_name);
                            if (model_names.Contains(","))
                            {
                                string[] model_name_array = model_names.Split(",");
                                // Loop over models and delete records
                                foreach (string model in model_name_array)
                                {
                                    DeleteOrphanedData(ModelOrphans(model), AppSettings.ModelKey(model), pk_value);
                                }
                            }
                            else
                            {
                                //delete records for single orphaned model
                                DeleteOrphanedData(ModelOrphans(model_name), AppSettings.ModelKey(model_name), pk_value);
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                //throw; TODO Log error
                return false;
            }
        }


        /// <summary>
        /// Check if model has potencial relational data orphaned on delete
        /// </summary>
        /// <param name="model_name"></param>
        /// <returns></returns>
        public bool ModelHasOrphans(string model_name)
        {
            switch (model_name)
            {
                case "Posts":
                case "Pages":
                case "Portfolio":
                    return true;
                default:
                    return false;
            }
        }


        /// <summary>
        /// Check if model has potencial relational data orphaned on delete
        /// </summary>
        /// <param name="model_name"></param>
        /// <returns></returns>
        public string ModelOrphans(string model_name)
        {
            switch (model_name)
            {
                case "Posts":
                case "Pages":
                case "Portfolio":
                    return "GalleryImages";
                default:
                    return null;
            }
        }

        //Deletes table record
        /// <summary>
        /// Delete table record(s) base on the key passed, used to delete orphaned records
        /// </summary>
        /// <returns>boolean</returns>
        public bool DeleteOrphanedData(string model_name, string pk_name, string pk_value)
        {
            var MsgCountQuery = @"DELETE FROM [" + model_name + "] WHERE [" + pk_name + "]  = @key";
            try
            {
                string connection_string = DBConnection();
                using (var con = new SqlConnection(connection_string))
                {
                    con.Open();
                    var cmd = new SqlCommand(MsgCountQuery, con);
                    cmd.Parameters.AddWithValue("@key", pk_value);
                    if (cmd.ExecuteScalar() != DBNull.Value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                //throw; TODO Log error
                return false;
            }
        }


        //Updates account permission
        /// <summary>
        /// Checks permission for user, if add updates, if empty, removes permission. 
        /// </summary>
        /// <returns>boolean</returns>
        public bool UpdatePermission(string permission_name, string permission_value, string account_id)
        {
            using (var db = new DBConnection())
            {
                try
                {
                    //get permission id
                    int PermissionID = 0;
                    if (db.Permissions.Any(s => s.PermissionName == permission_name))
                    {
                        PermissionID = db.Permissions.Where(s => s.PermissionName == permission_name).FirstOrDefault().PermissionID;
                    }

                    if (permission_value == "1")
                    {
                        //check if permission already exists for user
                        if (!db.AccountToPermissions.Any(s => s.AccountID == account_id && s.PermissionID == PermissionID))
                        {
                            //if not add permission
                            AddPermission(account_id, PermissionID);
                        }
                    }
                    else
                    {
                        //check if permission already exists for user
                        if (db.AccountToPermissions.Any(s => s.AccountID == account_id && s.PermissionID == PermissionID))
                        {
                            //if so remove permission
                            RemovePermission(account_id, PermissionID);
                        }
                    }
                    return true;
                }
                catch (Exception)
                {
                    //TODO log error
                    return false;
                }
            }

        }


        //Add permission data
        /// <summary>
        /// Add forgot password data to reset table
        /// </summary>
        /// <returns>boolean</returns>
        public bool AddPermission(string account_id, int permission_id)
        {
            using (var db = new DBConnection())
            {
                // Create AccountToPermission object.
                AccountToPermissionModel permission_data = new AccountToPermissionModel
                {
                    AccountID = account_id,
                    PermissionID = permission_id,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                // Add object to the AccountToPermission collection.
                db.AccountToPermissions.Add(permission_data);

                // Submit the change to the database.
                try
                {
                    db.SaveChanges();
                    return true;

                }
                catch (Exception ex)
                {
                    //TODO log error
                    Console.WriteLine(ex);

                    return false;
                }
            }
        }


        //Delete Permission
        /// <summary>
        /// Delete Permission for Account 
        /// </summary>
        /// <returns>boolean</returns>
        public bool RemovePermission(string account_id, int permission_id)
        {
            using (var db = new DBConnection())
            {
                try
                {
                    AccountToPermissionModel account = db.AccountToPermissions.Single(u => u.AccountID == account_id && u.PermissionID == permission_id);
                    db.AccountToPermissions.Remove(account);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //TODO Provide for exceptions.
                }
            }
            return false;
        }



        /// <summary>
        /// Add read contact record to table
        /// </summary>
        /// <param name="contact_id"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public bool MarkMessageRead(string contact_id, string account_id)
        {
            using (var db = new DBConnection())
            {
                if(!db.MessageViews.Where(s=> s.ContactID == contact_id && s.AccountID == account_id).Any())
                {
                    AppFunctions functions = new AppFunctions();
                    MessageViewModel message = new MessageViewModel
                    {
                        ViewID = functions.GetGuid(),
                        ContactID = contact_id,
                        AccountID = account_id,
                        UpdatedAt = DateTime.Now,
                        CreatedAt = DateTime.Now
                    };

                    db.MessageViews.Add(message);

                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //TODO log error
                        Console.WriteLine(ex);
                    }
                }
            }
            return false;
        }


        //Get current visitor ip address
        /// <summary>
        /// Get current user ip address.
        /// </summary>
        /// <returns>The IP Address</returns>
        public string FormatVisitorIP(string ip_address)
        {
            // Get the IP  
            if (ip_address == "::1")
            {
                ip_address = "127.0.0.1";
            }
            return ip_address;
        }


        //Get current visitor ip address
        /// <summary>
        /// Get current user ip address.
        /// </summary>
        /// <returns>The IP Address</returns>
        public string FormatVisitorIP(string ip_address, string optional_ip)
        {
            if (string.IsNullOrEmpty(ip_address))
            {
                ip_address = optional_ip;
            }

            // Get the IP  
            if (ip_address == "::1")
            {
                ip_address = "127.0.0.1";
            }
            return ip_address;
        }


        /// <summary>
        /// Log site visits
        /// </summary>
        public bool LogVisit(string document_id, string document_type, string ip_address, string browser, string device)
        {
            using (var db = new DBConnection())
            {
                try
                {
                    SiteVisitModel log = new SiteVisitModel
                    {
                        VisitID = GetGuid(),
                        DocumentID = document_id,
                        DocumentType = document_type,
                        IpAddress = ip_address,
                        Country = GetIpInfo(ip_address, "Country"),
                        Browser = browser,
                        Device = device,
                        CreatedAt = DateTime.Now
                    };

                    db.SiteVisits.Add(log);

                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //TODO log error
                        Console.WriteLine(ex);
                    }
                }
                catch (Exception ex)
                {
                    //TODO log error
                    Console.WriteLine(ex);
                }

            }
            return false;
        }



        //Get country info
        /// <summary>
        /// Get current visitor country info based on ip address
        /// </summary>
        /// <returns>The the info for the second parameter passed</returns>
        public string GetIpInfo(string ip_address, string return_data)
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
        ///get specific config data. Takes key and data to return
        /// </summary>
        public string GetCmsData(string unique_key)
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
        public string GetCmsData(string unique_key, string default_value)
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
        /// Add new subscriber
        /// </summary>
        public bool AddSubscriber(string subscriber_email)
        {
            using (var db = new DBConnection())
            {
                if (!db.Subscribers.Any(s => s.Email == subscriber_email))
                {
                    // Create a new Subscriber object.
                    SubscriberModel subscriber = new SubscriberModel
                    {
                        Email = subscriber_email,
                        UpdatedAt = DateTime.Now, 
                        CreatedAt = DateTime.Now
                    };

                    // Add the new object to the Orders collection.
                    db.Subscribers.Add(subscriber);

                    // Submit the change to the database.
                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            return false;
        }




        /// <summary>
        /// Checks if a user has permission access
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="permission_name"></param>
        /// <returns></returns>
        public bool UserHasPermission(string account_id, string permission_name)
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



        /// <summary>
        ///get content display status 
        /// </summary>
        public bool ContentDisplay(string unique_key)
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
        /// Update configurations value. Used for main script data update
        /// </summary>
        /// <param name="key"></param>
        /// <param name="config_value"></param>
        /// <returns></returns>
        public bool UpdateConfigValue(string key, string config_value)
        {
            using (var db = new DBConnection())
            {
                var query =
                    from config in db.Configurations
                    where config.ConfigurationKey == key
                    select config;

                foreach (ConfigurationModel config_data in query)
                {
                    config_data.ConfigurationValue = config_value;
                    config_data.UpdatedAt = DateTime.Now;
                }

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // TODO log error
                }
            }
            return false;
        }


        /// <summary>
        /// Gets search results for posts
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public string GetPostSearchResults(string q, int total = 10) {
            string result_data = "";

            using(var db = new DBConnection())
            {
                var DBQuery = db.Posts.Where(s => s.Status == 1 && (s.Title.Contains(q) || s.ShortDescription.Contains(q) || s.Content.Contains(q) ||
                    s.Categories.Contains(q) || s.PostTags.Contains(q) || s.SEOKeywords.Contains(q))).OrderByDescending(x=> x.CreatedAt).Take(total);

                foreach(var item in DBQuery)
                {
                    result_data += $@"<li class='list-group-item list-group-item-action'>
	                                        <a href='/Post/{item.Slug}' class='text-dark'>
		                                        <small>
			                                        <i class='bi bi-search'></i>
		                                        </small>
		                                        <strong>{item.Title}</strong>
	                                        </a>
                                        </li>";
                }
            }

            return result_data;
        }


        //Update Other Category by one
        /// <summary>
        /// Update Other Pages greater than updated page by one
        /// </summary>
        /// <returns>boolean</returns>
        //public bool PageSectionOrderReset(string section_id, int current_order)
        //{
        //    using (var db = new DBConnection())
        //    {
        //        // Query the database for the row to be updated.
        //        var DBQuery =
        //            from sections in db.PageSections
        //            where sections.SectionID != section_id && sections.SectionOrder >= current_order

        //            select sections;

        //        // Execute the query, and change the column values
        //        foreach (PageSectionsModel section_data in DBQuery)
        //        {
        //            section_data.SectionOrder = section_data.SectionOrder + 1;
        //            section_data.UpdatedAt = DateTime.Now;
        //        }

        //        // Submit the changes to the database.
        //        try
        //        {
        //            db.SaveChanges();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex);
        //            //TODO Provide for exceptions.
        //        }
        //    }
        //    return false;
        //}



        /// <summary>
        /// Gets search results for posts
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public string GetPageSearchResults(string q, int total = 10)
        {
            string result_data = "";

            using (var db = new DBConnection())
            {
                var DBQuery = db.Pages.Where(s => s.Status == 1 && (s.Title.Contains(q) || s.ShortDescription.Contains(q) || s.Content.Contains(q) || 
                    s.SEOKeywords.Contains(q))).OrderByDescending(x => x.CreatedAt).Take(total);

                foreach (var item in DBQuery)
                {
                    result_data += $@"<li class='list-group-item list-group-item-action'>
	                                        <a href='/Page/{item.Slug}' class='text-dark'>
		                                        <small>
			                                        <i class='bi bi-search'></i>
		                                        </small>
		                                        <strong>{item.Title}</strong>
	                                        </a>
                                        </li>";
                }
            }

            return result_data;
        }


        /// <summary>
        ///  Get Categories of Popular Posts
        /// </summary>
        /// <returns>string csv</returns>
        public string GetAllPopularCategories() 
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.PopularThisWeek.OrderByDescending(s => s.ValueOccurrence);

                foreach(var item in DBQuery)
                {
                    result_data += GetPostData(item.DocumentID, "Categories")+",";
                }
            }
            return result_data.TrimEnd(',');
        }


        /// <summary>
        ///  Get Categories of Popular Posts
        /// </summary>
        /// <returns>string csv</returns>
        public string GetAllPopularTags()
        {
            string result_data = "";
            using (var db = new DBConnection())
            {
                var DBQuery = db.PopularThisWeek.OrderByDescending(s => s.ValueOccurrence);

                foreach (var item in DBQuery)
                {
                    result_data += GetPostData(item.DocumentID, "PostTags") + ",";
                }
            }
            return result_data.TrimEnd(',');
        }


        /// <summary>
        /// Removes Duplicate CSV in string list
        /// </summary>
        /// <param name="csv_list"></param>
        /// <returns></returns>
        public string RemoveDuplicateCSV(string csv_list)
        {
            csv_list = csv_list.Replace(" ", "");
            List<string> uniqueValues = csv_list.Split(',').Distinct().ToList();
            string UniqueStrings = string.Join(",", uniqueValues);
            return UniqueStrings;
        }


        /// <summary>
        /// Removes old NLOG files
        /// </summary>
        public void RemoveNlogFiles(bool delete_old_nlog = true, int nlog_period = 3)
        {
            if (delete_old_nlog)
            {
                int min_nlog_period = nlog_period;
                int max_nlog_period = nlog_period + 6; //goes six months back from set date

                //loops from min_nlog_period (months) to six months before 
                for (int x = min_nlog_period; x <= max_nlog_period; x++)
                {
                    var DebugFilePath = @"bin\\Debug\\netcoreapp3.1\\";
                    var ReleaseFilePath = @"bin\\\Release\\netcoreapp3.1\\";
                    string BaseFileName = $"ngo-app-logger-{DateTime.Now.AddMonths(-x).ToString("yyyy-MM")}";
                    int MaxDay = 31;

                    try
                    {
                        for (int i = 1; i <= MaxDay; i++)
                        {
                            //append 0 to day if less than 10
                            string day = (i < 10) ? "0" + i : i.ToString();

                            //set file name in iteration
                            string FileName = $"{BaseFileName}-{day}.log";

                            // Check if file exists in Debug path  
                            if (File.Exists(Path.Combine(DebugFilePath, FileName)))
                            {
                                // If file found, delete it    
                                File.Delete(Path.Combine(DebugFilePath, FileName));
                            }

                            // Check if file exists in Release path  
                            if (File.Exists(Path.Combine(ReleaseFilePath, FileName)))
                            {
                                // If file found, delete it    
                                File.Delete(Path.Combine(ReleaseFilePath, FileName));
                            }
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }


    }

    public class AppSettings
    {

        public static string _connectionString = string.Empty;
        public static string _baseUrl = string.Empty;
        public AppSettings()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();

            _connectionString = root.GetSection("ConnectionStrings").GetSection("DBConnection").Value;
            _baseUrl = root.GetSection("systemConfiguration").GetSection("baseUrl").Value;
        }


        /// <summary>
        /// Gets base url
        /// </summary>
        /// <returns></returns>
        public static string BaseUrl()
        {
            return _baseUrl;
        }

        /// <summary>
        /// Gets db connection string
        /// </summary>
        /// <returns></returns>
        public static string DBConnection()
        {
            return _connectionString;
        }

        /// <summary>
        /// Get default profile picture url
        /// </summary>
        /// <returns></returns>
        public static string DefaultProfilePicture()
        {
            return "/admin/defaults/default-profile.jpg";
        }

        /// <summary>
        /// Get model key
        /// </summary>
        /// <param name="model_name"></param>
        /// <returns></returns>
        public static string ModelKey(string model_name)
        {
            switch (model_name)
            {
                case "ActivityLogs":
                    return "ActivityID"; 
                case "Accounts":
                    return "AccountID";
                case "AppMessages":
                    return "ID";
                case "Category":
                    return "CategoryID";
                case "Configurations":
                    return "ConfigurationID";
                case "Contact":
                    return "ContactID";
                case "ContactPersons":
                    return "ContactPersonID";
                case "ContractDocuments":
                    return "ID";
                case "DataTranslations":
                    return "ID";
                case "DocumentResources":
                    return "DocumentID";
                case "DonationCampaigns":
                    return "DonationID";
                case "FAQ":
                    return "FaqID";
                case "GalleryImages":
                    return "ID";
                case "HomeSliders":
                    return "SliderID";
                case "Navigation":
                    return "NavigationID";
                case "FooterNavigation":
                    return "NavigationID";
                case "Pages":
                    return "PageID";
                case "PageSections":
                    return "SectionID";
                case "Partner":
                    return "PartnerID";
                case "Permissions":
                    return "PermissionID";
                case "Portfolio":
                    return "PortfolioID";
                case "Posts":
                    return "PostID";
                case "Services":
                    return "ServiceID";
                case "ServiceStats":
                    return "ServiceStatID";
                case "SiteLanguages":
                    return "LanguageID";
                case "Subscribers":
                    return "ID";
                case "Team":
                    return "TeamID";
                case "Testimonials":
                    return "TestimonialID";
                case "Video":
                    return "VideoID";
                default:
                    return "ID";
            }
        }


        /// <summary>
        /// Gets display class for translation inputs, i.e. show or hide input
        /// </summary>
        /// <returns></returns>
        public static string GetTranslationInputClass(string model_name, string input_type)
        {
            if (input_type == "Title")
            {
                String[] TeaxAreaArray = { "Posts", "Category", "Pages", "Navigation", "FooterNavigation", "Services", "Portfolio", "PageSections", "HomeSliders", "Team", "Partner", "FAQ",
                    "Testimonials", "Apearance", "ServiceStats", "HeaderText", "DocumentResources", "DonationCampaigns", "Videos" };
                if (TeaxAreaArray.Contains(model_name))
                {
                    return "";
                }
                return "d-none";
            }
            else if (input_type == "Description")
            {
                String[] TeaxAreaArray = { "Posts", "Pages", "Services", "FAQ", "Testimonials", "HomeSliders", "Apearance", "ContentManagement", "DocumentResources", 
                    "DonationCampaigns" };
                if (TeaxAreaArray.Contains(model_name))
                {
                    return "";
                }
                return "d-none";
            }
            else if(input_type == "Content")
            {
                String[] TeaxAreaArray = { "Posts", "Pages", "PageSections", "Portfolio" };
                if (TeaxAreaArray.Contains(model_name))
                {
                    return "";
                }
                return "d-none";
            }
            return "";
        }



        /// <summary>
        /// Get translatile label name
        /// </summary>
        /// <param name="model_name"></param>
        /// <returns></returns>
        public static string GetTranslationLabelName(string model_name, string input_type)
        {
            if (input_type == "Title")
            {
                switch (model_name)
                {
                    case "Posts":
                    case "Pages":
                    case "Services":
                    case "PageSections":
                    case "Portfolio":
                    case "Team":
                    case "Testimonials":
                    case "Apearance":
                    case "HomeSliders":
                    case "Videos":
                        return "Title";
                    case "Navigation":
                    case "FooterNavigation":
                    case "Partner":
                    case "ServiceStats":
                    case "DocumentResources":
                        return "Name";
                    case "Category":
                        return "Category Name";
                    case "FAQ":
                        return "Question";
                    case "HeaderText":
                        return "Header Text";
                    default:
                        return "Title";
                }
            }
            else if (input_type == "Description")
            {
                switch (model_name)
                {
                    case "Posts":
                    case "Pages":
                    case "DocumentResources":
                        return "Description";
                    case "PageSections":
                        return "Content";
                    case "Services":
                        return "Short Description";
                    case "FAQ":
                        return "Answer";
                    case "Testimonials":
                        return "Testimonial";
                    case "Apearance":
                    case "HomeSliders":
                        return "Sub Text";
                    default:
                        return "Description";
                }
            }
            else if (input_type == "Content")
            {
                switch (model_name)
                {
                    case "Posts":
                        return "Post Content";
                    case "Pages":
                        return "Page Content";
                    case "Portfolio":
                        return "Portfolio Description";
                    default:
                        return "Content";
                }
            }
            return "Translation";
        }


        /// <summary>
        /// Gets input type for translation content (textarea or text-editor)
        /// </summary>
        /// <returns></returns>
        public static string GetTranslationContentType(string model_name) 
        {
            String[] TeaxAreaArray = { "Posts", "Pages" };
            if (TeaxAreaArray.Contains(model_name))
            {
                return "text-editor";
            }
            return "";
        }

    }
}
