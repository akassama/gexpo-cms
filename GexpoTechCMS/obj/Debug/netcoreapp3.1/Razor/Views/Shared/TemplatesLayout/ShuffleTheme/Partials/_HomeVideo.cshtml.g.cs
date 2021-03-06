#pragma checksum "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "804be0fe17048a91f2c4b22cf1311360d6f551f6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_ShuffleTheme_Partials__HomeVideo), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/ShuffleTheme/Partials/_HomeVideo.cshtml")]
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
#line 1 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"804be0fe17048a91f2c4b22cf1311360d6f551f6", @"/Views/Shared/TemplatesLayout/ShuffleTheme/Partials/_HomeVideo.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_TemplatesLayout_ShuffleTheme_Partials__HomeVideo : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
  
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
#line 15 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
 if (CMSHelpers.ContentDisplay("HomeVideo"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!-- ======= Video Section ======= -->\r\n    <section id=\"video\" class=\"about\">\r\n        <div class=\"container\">\r\n            <div class=\"row mt-5\">\r\n");
#nullable restore
#line 21 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
                 if (string.IsNullOrEmpty(HomeVideoTitle) && string.IsNullOrEmpty(HomeVideoDescription))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"col-lg-12\">\r\n                        <div class=\"embed-responsive embed-responsive-16by9\">\r\n                            <iframe class=\"embed-responsive-item w-100\" style=\"min-height: 22em;\"");
            BeginWriteAttribute("src", " src=\"", 1320, "\"", 1403, 3);
            WriteAttributeValue("", 1326, "https://www.youtube.com/embed/", 1326, 30, true);
#nullable restore
#line 25 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 1356, Helpers.GetYouTubeVideoID(HomeVideoLink), 1356, 41, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1397, "?rel=0", 1397, 6, true);
            EndWriteAttribute();
            WriteLiteral(" allowfullscreen></iframe>\r\n                        </div>\r\n                    </div>\r\n");
#nullable restore
#line 28 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
                }
                else if (!string.IsNullOrEmpty(HomeVideoTitle) && string.IsNullOrEmpty(HomeVideoDescription))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"col-lg-6\">\r\n                        <div class=\"embed-responsive embed-responsive-16by9\">\r\n                            <iframe class=\"embed-responsive-item w-100\" style=\"min-height: 22em;\"");
            BeginWriteAttribute("src", " src=\"", 1861, "\"", 1944, 3);
            WriteAttributeValue("", 1867, "https://www.youtube.com/embed/", 1867, 30, true);
#nullable restore
#line 33 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 1897, Helpers.GetYouTubeVideoID(HomeVideoLink), 1897, 41, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1938, "?rel=0", 1938, 6, true);
            EndWriteAttribute();
            WriteLiteral(" allowfullscreen></iframe>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-lg-6 pt-4 pt-lg-0 content\">\r\n                        <h3>");
#nullable restore
#line 37 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
                       Write(HomeVideoTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n                    </div>\r\n");
#nullable restore
#line 39 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"col-lg-6\">\r\n                        <div class=\"embed-responsive embed-responsive-16by9\">\r\n                            <iframe class=\"embed-responsive-item w-100\" style=\"min-height: 22em;\"");
            BeginWriteAttribute("src", " src=\"", 2456, "\"", 2539, 3);
            WriteAttributeValue("", 2462, "https://www.youtube.com/embed/", 2462, 30, true);
#nullable restore
#line 44 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
WriteAttributeValue("", 2492, Helpers.GetYouTubeVideoID(HomeVideoLink), 2492, 41, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 2533, "?rel=0", 2533, 6, true);
            EndWriteAttribute();
            WriteLiteral(" allowfullscreen></iframe>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-lg-6 pt-4 pt-lg-0 content\">\r\n                        <h3>");
#nullable restore
#line 48 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
                       Write(HomeVideoTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n                        ");
#nullable restore
#line 49 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
                   Write(Helpers.GetTranslatableData(HomeVideoDescription, SessionLanguage, HomeVideoID, "ContentManagement", "Description"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n");
#nullable restore
#line 51 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\r\n        </div>\r\n    </section>\r\n    <!-- End Home Video Section -->\r\n");
#nullable restore
#line 56 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomeVideo.cshtml"
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
