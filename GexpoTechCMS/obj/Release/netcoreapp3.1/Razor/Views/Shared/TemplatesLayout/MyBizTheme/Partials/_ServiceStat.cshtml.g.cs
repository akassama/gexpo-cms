#pragma checksum "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ServiceStat.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cd658a55b12fdb6e4e1e90ecf4a43f695a14da5e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_MyBizTheme_Partials__ServiceStat), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/MyBizTheme/Partials/_ServiceStat.cshtml")]
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
#line 1 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ServiceStat.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ServiceStat.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cd658a55b12fdb6e4e1e90ecf4a43f695a14da5e", @"/Views/Shared/TemplatesLayout/MyBizTheme/Partials/_ServiceStat.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_TemplatesLayout_MyBizTheme_Partials__ServiceStat : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ServiceStat.cshtml"
  
    string SessionLanguage = Helpers.DefaultData(HttpContextAccessor.HttpContext.Session.GetString("_SessionLanguage"), "en");

    string OurServicesID = SqlHelpers.GetTableData("ContentManagement", "ContentName", "Services", "ContentID");
    string OurServicesHeader = SqlHelpers.GetTableData("ContentManagement", "ContentName", "Services", "ContentValue");

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ServiceStat.cshtml"
 if (CMSHelpers.ContentDisplay("ServiceStatistics"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!-- ======= Service Stats Section ======= -->\r\n    <section id=\'counts\' class=\'counts\'>\r\n        <div class=\'container position-relative\'>\r\n\r\n            <div class=\'row counters\'>\r\n                ");
#nullable restore
#line 18 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ServiceStat.cshtml"
           Write(MyBizTemplateHelpers.GetServiceStats(SessionLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n\r\n        </div>\r\n    </section>\r\n    <!-- End Service Stats Section -->\r\n");
#nullable restore
#line 24 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\MyBizTheme\Partials\_ServiceStat.cshtml"
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
