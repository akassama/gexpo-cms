#pragma checksum "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomePosts.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9c486ae1839ec612e566d2f6b510991f62b8a777"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_ShuffleTheme_Partials__HomePosts), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/ShuffleTheme/Partials/_HomePosts.cshtml")]
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
#line 1 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomePosts.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomePosts.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9c486ae1839ec612e566d2f6b510991f62b8a777", @"/Views/Shared/TemplatesLayout/ShuffleTheme/Partials/_HomePosts.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_TemplatesLayout_ShuffleTheme_Partials__HomePosts : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomePosts.cshtml"
  
    string SessionLanguage = Helpers.DefaultData(HttpContextAccessor.HttpContext.Session.GetString("_SessionLanguage"), "en");

    string PostsHeaderID = SqlHelpers.GetTableData("ContentManagement", "ContentName", "Posts", "ContentID");
    string PostsHeader = SqlHelpers.GetTableData("ContentManagement", "ContentName", "Posts", "ContentValue");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 12 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomePosts.cshtml"
 if (CMSHelpers.ContentDisplay("Posts"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!-- ======= Recent Posts ======= -->\r\n    <section class=\"more-services section-bg\">\r\n        <div class=\"container\">\r\n            <div class=\"text-center\">\r\n                <h3>");
#nullable restore
#line 18 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomePosts.cshtml"
               Write(Helpers.GetTranslatableData(PostsHeader, SessionLanguage, PostsHeaderID, "ContentManagement", "Description"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n            </div>\r\n            <div class=\"row justify-content-center\">\r\n                ");
#nullable restore
#line 21 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomePosts.cshtml"
           Write(ShuffleTemplateHelpers.GetHomePosts(SessionLanguage, 9));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n\r\n        </div>\r\n    </section>\r\n    <!-- End Posts Section -->\r\n");
#nullable restore
#line 27 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\ShuffleTheme\Partials\_HomePosts.cshtml"
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
