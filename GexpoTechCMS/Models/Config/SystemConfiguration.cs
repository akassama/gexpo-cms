using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoExpoApp.Models
{
    public class SystemConfiguration
    {
        public string appEnvironment { get; set; }
        public string baseUrl { get; set; }
        public string profilePicture { get; set; }
        public string smtpEmail { get; set; }
        public string smtpPass { get; set; }
        public string smtpHost { get; set; }
        public int smtpPort { get; set; }
        public string authCookieKey { get; set; }
        public string sendGridKey { get; set; }
        public string organizationName { get; set; }
        public string seoAuthor { get; set; }
        public string seoDescription { get; set; }
        public string seoKeywords { get; set; }
        public string availableThemes { get; set; }
        public bool nLogDelete { get; set; }
        public int nLogDeletePeriod { get; set; }
    }
}
