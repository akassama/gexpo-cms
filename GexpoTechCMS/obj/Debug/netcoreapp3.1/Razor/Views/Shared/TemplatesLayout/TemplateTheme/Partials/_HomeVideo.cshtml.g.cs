#pragma checksum "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6743d847c929ff7b86de83c48c3a5f44577f4653"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_TemplateTheme_Partials__HomeVideo), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/TemplateTheme/Partials/_HomeVideo.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\_ViewImports.cshtml"
using NgoExpoApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\_ViewImports.cshtml"
using NgoExpoApp.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6743d847c929ff7b86de83c48c3a5f44577f4653", @"/Views/Shared/TemplatesLayout/TemplateTheme/Partials/_HomeVideo.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_TemplatesLayout_TemplateTheme_Partials__HomeVideo : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
  
    string SessionLanguage = Helpers.DefaultData(HttpContextAccessor.HttpContext.Session.GetString("_SessionLanguage"), "en");

    //Home Video Data
    string HomeVideoLink = SqlHelpers.GetTableData("ContentManagement", "ContentName", "HomeVideo", "ContentValue");
    string HomeVideoTitle = SqlHelpers.GetTableData("ContentManagement", "ContentName", "HomeVideoTitle", "ContentValue");
    string HomeVideoDescription = SqlHelpers.GetTableData("ContentManagement", "ContentName", "HomeVideoDescription", "ContentValue");
    string HomeVideoID = SqlHelpers.GetTableData("ContentManagement", "ContentName", "HomeVideoTitle", "ContentID");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 15 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
 if (CMSHelpers.ContentDisplay("HomeVideo"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!-- ======= Home Video Section ======= -->\r\n    <section id=\"why-us\" class=\"why-us section-bg\">\r\n        <div class=\"container-fluid\" data-aos=\"fade-up\">\r\n            <div class=\"row\">\r\n");
#nullable restore
#line 21 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
                 if (string.IsNullOrEmpty(HomeVideoTitle) && string.IsNullOrEmpty(HomeVideoDescription))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"col-lg-12 border border-1\">\r\n                        <a");
            BeginWriteAttribute("href", " href=\"", 1224, "\"", 1245, 1);
#nullable restore
#line 24 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 1231, HomeVideoLink, 1231, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" target=\"_blank\">\r\n                            <div class=\"bg-image\"");
            BeginWriteAttribute("style", "\r\n                                 style=\"", 1314, "\"", 1511, 6);
            WriteAttributeValue("", 1356, "background-image:", 1356, 17, true);
            WriteAttributeValue(" ", 1373, "url(\'https://i3.ytimg.com/vi/", 1374, 30, true);
#nullable restore
#line 26 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 1403, Helpers.GetYouTubeVideoID(HomeVideoLink), 1403, 41, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1444, "/hqdefault.jpg\');", 1444, 17, true);
            WriteAttributeValue("\r\n                                    ", 1461, "height:", 1499, 45, true);
            WriteAttributeValue(" ", 1506, "50vh", 1507, 5, true);
            EndWriteAttribute();
            WriteLiteral(@">
                                <div class=""video-thumbnail w-100 h-100"">
                                    <i class=""bi bi-play-circle-fill play-btn""></i>
                                </div>
                            </div>
                        </a>
                    </div>
");
#nullable restore
#line 34 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
                }
                else if (!string.IsNullOrEmpty(HomeVideoTitle) && string.IsNullOrEmpty(HomeVideoDescription))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <h3 class=\"text-center m-3\">\r\n                        ");
#nullable restore
#line 38 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
                   Write(HomeVideoTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </h3>\r\n                    <div class=\"col-lg-12 border border-1\">\r\n                        <a");
            BeginWriteAttribute("href", " href=\"", 2163, "\"", 2184, 1);
#nullable restore
#line 41 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 2170, HomeVideoLink, 2170, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" target=\"_blank\">\r\n                            <div class=\"bg-image\"");
            BeginWriteAttribute("style", "\r\n                                 style=\"", 2253, "\"", 2450, 6);
            WriteAttributeValue("", 2295, "background-image:", 2295, 17, true);
            WriteAttributeValue(" ", 2312, "url(\'https://i3.ytimg.com/vi/", 2313, 30, true);
#nullable restore
#line 43 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 2342, Helpers.GetYouTubeVideoID(HomeVideoLink), 2342, 41, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 2383, "/hqdefault.jpg\');", 2383, 17, true);
            WriteAttributeValue("\r\n                                    ", 2400, "height:", 2438, 45, true);
            WriteAttributeValue(" ", 2445, "50vh", 2446, 5, true);
            EndWriteAttribute();
            WriteLiteral(@">
                                <div class=""video-thumbnail w-100 h-100"">
                                    <i class=""bi bi-play-circle-fill play-btn""></i>
                                </div>
                            </div>
                        </a>
                    </div>
");
#nullable restore
#line 51 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"col-lg-6 border border-1\">\r\n                        <a");
            BeginWriteAttribute("href", " href=\"", 2894, "\"", 2915, 1);
#nullable restore
#line 55 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 2901, HomeVideoLink, 2901, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" target=\"_blank\">\r\n                            <div class=\"bg-image\"");
            BeginWriteAttribute("style", "\r\n                                 style=\"", 2984, "\"", 3181, 6);
            WriteAttributeValue("", 3026, "background-image:", 3026, 17, true);
            WriteAttributeValue(" ", 3043, "url(\'https://i3.ytimg.com/vi/", 3044, 30, true);
#nullable restore
#line 57 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 3073, Helpers.GetYouTubeVideoID(HomeVideoLink), 3073, 41, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3114, "/hqdefault.jpg\');", 3114, 17, true);
            WriteAttributeValue("\r\n                                    ", 3131, "height:", 3169, 45, true);
            WriteAttributeValue(" ", 3176, "50vh", 3177, 5, true);
            EndWriteAttribute();
            WriteLiteral(@">
                                <div class=""video-thumbnail w-100 h-100"">
                                    <i class=""bi bi-play-circle-fill play-btn""></i>
                                </div>
                            </div>
                        </a>
                    </div>
");
            WriteLiteral("                    <div class=\"col-lg-6 d-flex flex-column justify-content-center align-items-stretch\">\r\n                        <div class=\"content\">\r\n                            <h3>\r\n                                ");
#nullable restore
#line 69 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
                           Write(HomeVideoTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </h3>\r\n                            ");
#nullable restore
#line 71 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
                       Write(Helpers.GetTranslatableData(HomeVideoDescription, SessionLanguage, HomeVideoID, "ContentManagement", "Description"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n");
#nullable restore
#line 74 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </section>\r\n    <!-- End Home Video Section -->\r\n");
#nullable restore
#line 80 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\TemplateTheme\Partials\_HomeVideo.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IHttpContextAccessor HttpContextAccessor { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
