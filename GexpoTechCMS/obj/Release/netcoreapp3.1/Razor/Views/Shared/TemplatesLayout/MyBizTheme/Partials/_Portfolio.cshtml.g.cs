#pragma checksum "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_Portfolio.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "28abdcf4b594e6a24e0533394e07fc15757f914e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_MyBizTheme_Partials__Portfolio), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/MyBizTheme/Partials/_Portfolio.cshtml")]
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
#line 1 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\_ViewImports.cshtml"
using NgoExpoApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\_ViewImports.cshtml"
using NgoExpoApp.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_Portfolio.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_Portfolio.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"28abdcf4b594e6a24e0533394e07fc15757f914e", @"/Views/Shared/TemplatesLayout/MyBizTheme/Partials/_Portfolio.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_TemplatesLayout_MyBizTheme_Partials__Portfolio : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_Portfolio.cshtml"
  
    string SessionLanguage = Helpers.DefaultData(HttpContextAccessor.HttpContext.Session.GetString("_SessionLanguage"), "en");

    string PortfolioHeaderID = SqlHelpers.GetTableData("ContentManagement", "ContentName", "Portfolio", "ContentID");
    string PortfolioHeader = SqlHelpers.GetTableData("ContentManagement", "ContentName", "Portfolio", "ContentValue");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 12 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_Portfolio.cshtml"
 if (CMSHelpers.ContentDisplay("Portfolio"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!-- ======= Portfolio Section ======= -->\r\n    <section id=\"portfolio\" class=\"portfolio\">\r\n        <div class=\"container\">\r\n\r\n            <div class=\"section-title\">\r\n                <span>Portfolio</span>\r\n                <h2>\r\n                    ");
#nullable restore
#line 21 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_Portfolio.cshtml"
               Write(Helpers.GetTranslatableData(PortfolioHeader, SessionLanguage, PortfolioHeaderID, "ContentManagement", "Description"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </h2>\r\n            </div>\r\n\r\n            <div class=\"row portfolio-container\">\r\n                ");
#nullable restore
#line 26 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_Portfolio.cshtml"
           Write(MyBizTemplateHelpers.GetHomePortfolio(SessionLanguage, 9));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </section>\r\n    <!-- End Portfolio Section -->\r\n");
#nullable restore
#line 31 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_Portfolio.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IHttpContextAccessor HttpContextAccessor { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
