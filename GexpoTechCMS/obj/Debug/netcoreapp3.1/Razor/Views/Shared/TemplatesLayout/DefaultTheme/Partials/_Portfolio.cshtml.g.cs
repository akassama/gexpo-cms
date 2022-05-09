#pragma checksum "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Portfolio.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "347f5d3abdaf8a562e47f051765b5cd6b3336c10"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_DefaultTheme_Partials__Portfolio), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/DefaultTheme/Partials/_Portfolio.cshtml")]
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
#line 1 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Portfolio.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Portfolio.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"347f5d3abdaf8a562e47f051765b5cd6b3336c10", @"/Views/Shared/TemplatesLayout/DefaultTheme/Partials/_Portfolio.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_TemplatesLayout_DefaultTheme_Partials__Portfolio : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Portfolio.cshtml"
  
    string SessionLanguage = Helpers.DefaultData(HttpContextAccessor.HttpContext.Session.GetString("_SessionLanguage"), "en");

    string PortfolioHeaderID = SqlHelpers.GetTableData("ContentManagement", "ContentName", "Portfolio", "ContentID");
    string PortfolioHeader = SqlHelpers.GetTableData("ContentManagement", "ContentName", "Portfolio", "ContentValue");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 12 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Portfolio.cshtml"
 if (CMSHelpers.ContentDisplay("Portfolio"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!-- ======= Portfolio Section ======= -->\r\n    <section id=\"portfolio\" class=\"portfolio\">\r\n        <div class=\"container\" data-aos=\"fade-up\">\r\n\r\n            <div class=\"section-title\">\r\n                <h2>Portfolio</h2>\r\n                <p");
            BeginWriteAttribute("class", " class=\"", 786, "\"", 794, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n                    ");
#nullable restore
#line 21 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Portfolio.cshtml"
               Write(Helpers.GetTranslatableData(PortfolioHeader, SessionLanguage, PortfolioHeaderID, "ContentManagement", "Description"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </p>\r\n            </div>\r\n\r\n            <div class=\"row portfolio-container d-flex justify-content-center\" data-aos=\"fade-up\" data-aos-delay=\"200\">\r\n\r\n                ");
#nullable restore
#line 27 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Portfolio.cshtml"
           Write(TemplateHelpers.GetHomePortfolio(SessionLanguage, 12));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n            </div>\r\n\r\n        </div>\r\n    </section>\r\n    <!-- End Portfolio Section -->\r\n");
#nullable restore
#line 34 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Portfolio.cshtml"
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
