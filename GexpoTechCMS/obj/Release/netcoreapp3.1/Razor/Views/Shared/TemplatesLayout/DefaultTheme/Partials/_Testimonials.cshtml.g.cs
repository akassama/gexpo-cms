#pragma checksum "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Testimonials.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8ecbd16c58543de93d573651fb26b1b2c85a6114"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_DefaultTheme_Partials__Testimonials), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/DefaultTheme/Partials/_Testimonials.cshtml")]
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
#line 1 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Testimonials.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Testimonials.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8ecbd16c58543de93d573651fb26b1b2c85a6114", @"/Views/Shared/TemplatesLayout/DefaultTheme/Partials/_Testimonials.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_TemplatesLayout_DefaultTheme_Partials__Testimonials : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Testimonials.cshtml"
  
    string SessionLanguage = Helpers.DefaultData(HttpContextAccessor.HttpContext.Session.GetString("_SessionLanguage"), "en");

    string TestimonialHeaderID = SqlHelpers.GetTableData("ContentManagement", "ContentName", "Testimonials", "ContentID");
    string TestimonialHeader = SqlHelpers.GetTableData("ContentManagement", "ContentName", "Testimonials", "ContentValue");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 12 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Testimonials.cshtml"
 if (CMSHelpers.ContentDisplay("Testimonials"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <!-- ======= Testimonials Section ======= -->
    <section id=""testimonials"" class=""testimonials section-bg"">
        <div class=""container"" data-aos=""fade-up"">

            <div class=""section-title"">
                <h2>Testimonials</h2>
                <p");
            BeginWriteAttribute("class", " class=\"", 822, "\"", 830, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n                    ");
#nullable restore
#line 21 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Testimonials.cshtml"
               Write(Helpers.GetTranslatableData(TestimonialHeader, SessionLanguage, TestimonialHeaderID, "ContentManagement", "Description"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                </p>
            </div>

            <div class=""testimonials-slider swiper-container"" data-aos=""fade-up"" data-aos-delay=""100"">
                <div class=""swiper-wrapper"">
                    <!-- Begin testimonial items -->
                    ");
#nullable restore
#line 28 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Testimonials.cshtml"
               Write(TemplateHelpers.GetTestimonials(SessionLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    <!-- End testimonial items -->\r\n\r\n                </div>\r\n                <div class=\"swiper-pagination\"></div>\r\n            </div>\r\n\r\n        </div>\r\n    </section>\r\n    <!-- End Testimonials Section -->\r\n");
#nullable restore
#line 38 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\DefaultTheme\Partials\_Testimonials.cshtml"
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