#pragma checksum "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Faq.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e82d30a33853b135ca99f2e1b1f65172a63b78e6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_TempoTheme_Partials__Faq), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/TempoTheme/Partials/_Faq.cshtml")]
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
#line 1 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Faq.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Faq.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e82d30a33853b135ca99f2e1b1f65172a63b78e6", @"/Views/Shared/TemplatesLayout/TempoTheme/Partials/_Faq.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_TemplatesLayout_TempoTheme_Partials__Faq : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Faq.cshtml"
  
    string SessionLanguage = Helpers.DefaultData(HttpContextAccessor.HttpContext.Session.GetString("_SessionLanguage"), "en");

    string FAQHeaderID = SqlHelpers.GetTableData("ContentManagement", "ContentName", "FAQ", "ContentID");
    string FAQHeader = SqlHelpers.GetTableData("ContentManagement", "ContentName", "FAQ", "ContentValue");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 12 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Faq.cshtml"
 if (CMSHelpers.ContentDisplay("FAQ"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!-- ======= F.A.Q Section ======= -->\r\n    <section id=\"faq\" class=\"faq\">\r\n        <div class=\"container\">\r\n            <div class=\"section-title\">\r\n                <h2>F.A.Q</h2>\r\n                <h3> ");
#nullable restore
#line 19 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Faq.cshtml"
                Write(Helpers.GetTranslatableData(FAQHeader, SessionLanguage, FAQHeaderID, "ContentManagement", "Description"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n            </div>\r\n\r\n            <ul class=\"faq-list\">\r\n                ");
#nullable restore
#line 23 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Faq.cshtml"
           Write(TempoTemplateHelpers.GetHomeFAQ(SessionLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </ul>\r\n        </div>\r\n    </section>\r\n    <!-- End F.A.Q Section -->\r\n");
#nullable restore
#line 28 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Faq.cshtml"
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
