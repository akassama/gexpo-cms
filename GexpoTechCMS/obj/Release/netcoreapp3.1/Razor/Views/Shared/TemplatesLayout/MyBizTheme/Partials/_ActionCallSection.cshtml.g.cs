#pragma checksum "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ActionCallSection.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "139133fe21f2959b56d33d26147bec53ddb164ce"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_MyBizTheme_Partials__ActionCallSection), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/MyBizTheme/Partials/_ActionCallSection.cshtml")]
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
#line 1 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ActionCallSection.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ActionCallSection.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"139133fe21f2959b56d33d26147bec53ddb164ce", @"/Views/Shared/TemplatesLayout/MyBizTheme/Partials/_ActionCallSection.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_TemplatesLayout_MyBizTheme_Partials__ActionCallSection : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ActionCallSection.cshtml"
  
    string SessionLanguage = Helpers.DefaultData(HttpContextAccessor.HttpContext.Session.GetString("_SessionLanguage"), "en");

    //Call To Action Data
    string CallToActionoLink = SqlHelpers.GetTableData("ContentManagement", "ContentName", "CallToActionoLink", "ContentValue");
    string CallToActionID = SqlHelpers.GetTableData("ContentManagement", "ContentName", "CallToAction", "ContentID");
    string CallToActionHeader = SqlHelpers.GetTableData("ContentManagement", "ContentName", "CallToAction", "ContentValue");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 14 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ActionCallSection.cshtml"
 if (CMSHelpers.ContentDisplay("CallToAction"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!-- ======= Cta Section ======= -->\r\n    <section id=\"cta\" class=\"cta \">\r\n        <div class=\"container bg-light\" data-aos=\"zoom-in\">\r\n            <div class=\"text-center\">\r\n                <a class=\"cta-btn \"");
            BeginWriteAttribute("href", "href=\"", 921, "\"", 945, 1);
#nullable restore
#line 20 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ActionCallSection.cshtml"
WriteAttributeValue("", 927, CallToActionoLink, 927, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    ");
#nullable restore
#line 21 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ActionCallSection.cshtml"
               Write(Helpers.GetTranslatableData(CallToActionHeader, SessionLanguage, CallToActionID, "ContentManagement", "Description"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </a>\r\n            </div>\r\n        </div>\r\n    </section>\r\n    <!-- End Cta Section -->\r\n");
#nullable restore
#line 27 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ActionCallSection.cshtml"
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
