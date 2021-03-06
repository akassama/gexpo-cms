#pragma checksum "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9c313ec467855eac6090dcb34dd5a7ace18f1a17"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_DefaultTheme_Partials__HomeVideo), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/DefaultTheme/Partials/_HomeVideo.cshtml")]
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
#line 1 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9c313ec467855eac6090dcb34dd5a7ace18f1a17", @"/Views/Shared/TemplatesLayout/DefaultTheme/Partials/_HomeVideo.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_TemplatesLayout_DefaultTheme_Partials__HomeVideo : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
  
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
#line 15 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
 if (CMSHelpers.ContentDisplay("HomeVideo"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!-- ======= Home Video Section ======= -->\r\n    <section id=\"why-us\" class=\"why-us section-bg\">\r\n        <div class=\"container-fluid\" data-aos=\"fade-up\">\r\n            <div class=\"row\">\r\n");
#nullable restore
#line 21 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
                 if (string.IsNullOrEmpty(HomeVideoTitle) && string.IsNullOrEmpty(HomeVideoDescription))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"col-lg-12 align-items-stretch video-box\"");
            BeginWriteAttribute("style", " style=\'", 1209, "\'", 1322, 4);
            WriteAttributeValue("", 1217, "background-image:", 1217, 17, true);
            WriteAttributeValue(" ", 1234, "url(\"https://i3.ytimg.com/vi/", 1235, 30, true);
#nullable restore
#line 23 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 1264, Helpers.GetYouTubeVideoID(HomeVideoLink), 1264, 41, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1305, "/hqdefault.jpg\");", 1305, 17, true);
            EndWriteAttribute();
            WriteLiteral(" data-aos=\"zoom-in\" data-aos-delay=\"100\">\r\n                        <a");
            BeginWriteAttribute("href", " href=\"", 1392, "\"", 1413, 1);
#nullable restore
#line 24 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 1399, HomeVideoLink, 1399, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" target=\"_blank\" class=\"venobox play-btn mb-4\" data-vbtype=\"video\" data-autoplay=\"true\"></a>\r\n                    </div>\r\n");
#nullable restore
#line 26 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
                }
                else if (!string.IsNullOrEmpty(HomeVideoTitle) && string.IsNullOrEmpty(HomeVideoDescription))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <h3 class=\"text-center m-3\">\r\n                        ");
#nullable restore
#line 30 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
                   Write(HomeVideoTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </h3>\r\n                    <div class=\"col-lg-12 align-items-stretch video-box\"");
            BeginWriteAttribute("style", " style=\'", 1875, "\'", 1988, 4);
            WriteAttributeValue("", 1883, "background-image:", 1883, 17, true);
            WriteAttributeValue(" ", 1900, "url(\"https://i3.ytimg.com/vi/", 1901, 30, true);
#nullable restore
#line 32 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 1930, Helpers.GetYouTubeVideoID(HomeVideoLink), 1930, 41, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1971, "/hqdefault.jpg\");", 1971, 17, true);
            EndWriteAttribute();
            WriteLiteral(" data-aos=\"zoom-in\" data-aos-delay=\"100\">\r\n                        <a");
            BeginWriteAttribute("href", " href=\"", 2058, "\"", 2079, 1);
#nullable restore
#line 33 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 2065, HomeVideoLink, 2065, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" target=\"_blank\" class=\"venobox play-btn mb-4\" data-vbtype=\"video\" data-autoplay=\"true\"></a>\r\n                    </div>\r\n");
#nullable restore
#line 35 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"col-lg-6 align-items-stretch video-box\"");
            BeginWriteAttribute("style", " style=\'", 2333, "\'", 2446, 4);
            WriteAttributeValue("", 2341, "background-image:", 2341, 17, true);
            WriteAttributeValue(" ", 2358, "url(\"https://i3.ytimg.com/vi/", 2359, 30, true);
#nullable restore
#line 38 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 2388, Helpers.GetYouTubeVideoID(HomeVideoLink), 2388, 41, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 2429, "/hqdefault.jpg\");", 2429, 17, true);
            EndWriteAttribute();
            WriteLiteral(" data-aos=\"zoom-in\" data-aos-delay=\"100\">\r\n                        <a");
            BeginWriteAttribute("href", " href=\"", 2516, "\"", 2537, 1);
#nullable restore
#line 39 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 2523, HomeVideoLink, 2523, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" target=\"_blank\" class=\"venobox play-btn mb-4\" data-vbtype=\"video\" data-autoplay=\"true\"></a>\r\n                    </div>\r\n");
            WriteLiteral("                    <div class=\"col-lg-6 d-flex flex-column justify-content-center align-items-stretch\">\r\n                        <div class=\"content\">\r\n                            <h3>\r\n                                ");
#nullable restore
#line 45 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
                           Write(HomeVideoTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </h3>\r\n                            ");
#nullable restore
#line 47 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
                       Write(Helpers.GetTranslatableData(HomeVideoDescription, SessionLanguage, HomeVideoID, "ContentManagement", "Description"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n");
#nullable restore
#line 50 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </section>\r\n    <!-- End Home Video Section -->\r\n");
#nullable restore
#line 56 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_HomeVideo.cshtml"
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
