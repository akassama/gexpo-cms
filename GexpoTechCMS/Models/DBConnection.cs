using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GexpoTechCMS.Models.AppModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NgoExpoApp.Models;

namespace NgoExpoApp.Models
{
    public class DBConnection : DbContext
    {
        public DBConnection()
        {
        }

        public DBConnection(DbContextOptions<DBConnection> options)
            : base(options)
        {
        }

        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<ConfigurationModel> Configurations { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<PopularThisWeekModel> PopularThisWeek { get; set; }
        public DbSet<NavigationModel> Navigations { get; set; }
        public DbSet<FooterNavigationModel> FooterNavigations { get; set; }
        public DbSet<PageModel> Pages { get; set; }
        public DbSet<ServiceModel> Services { get; set; }
        public DbSet<PortfolioModel> Portfolios { get; set; }
        public DbSet<TeamModel> Teams { get; set; }
        public DbSet<PartnerModel> Partners { get; set; }
        public DbSet<ContactModel> Contacts { get; set; }
        public DbSet<MessageViewModel> MessageViews { get; set; }
        public DbSet<FAQModel> FAQs { get; set; }
        public DbSet<TestimonialModel> Testimonials { get; set; }
        public DbSet<HomeSliderModel> HomeSliders { get; set; }
        public DbSet<ThemeSettingModel> ThemeSettings { get; set; }
        public DbSet<ContentManagementModel> ContentManagement { get; set; }
        public DbSet<ServiceStatModel> ServiceStats { get; set; }
        public DbSet<ContentDisplayModel> ContentDisplay { get; set; }
        public DbSet<ActivityLogModel> ActivityLogs { get; set; }
        public DbSet<SubscriberModel> Subscribers { get; set; }
        public DbSet<GalleryImageModel> GalleryImages { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<PermissionModel> Permissions { get; set; }
        public DbSet<AccountToPermissionModel> AccountToPermissions { get; set; }
        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<PasswordForgotModel> PasswordForgot { get; set; }
        public DbSet<ChangePasswordModel> ChangePassword { get; set; }
        public DbSet<LoginViewModel> LoginView { get; set; }
        public DbSet<AppMessageModel> AppMessages { get; set; }
        public DbSet<SiteVisitModel> SiteVisits { get; set; }
        public DbSet<LanguagesModel> Languages { get; set; }
        public DbSet<SiteLanguagesModel> SiteLanguages { get; set; }
        public DbSet<DataTranslationsModel> DataTranslations { get; set; }
        public DbSet<DonationCampaignsModel> DonationCampaigns { get; set; }
        public DbSet<PageSectionsModel> PageSections { get; set; }
        public DbSet<DocumentResourcesModel> DocumentResources { get; set; }
        public DbSet<VideosModel> Videos { get; set; }
        public DbSet<ContractDocumentsModel> ContractDocuments { get; set; }


        //Initialize DbContextOptions within the context, used in AppHelper file
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBConnection"));
        }
    }
}
