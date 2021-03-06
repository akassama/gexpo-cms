#pragma checksum "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9b9da57065bd2ba3208d07c5fcd60a01dfc1d470"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_Common__ProcessMessage), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/Common/_ProcessMessage.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9b9da57065bd2ba3208d07c5fcd60a01dfc1d470", @"/Views/Shared/TemplatesLayout/Common/_ProcessMessage.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_TemplatesLayout_Common__ProcessMessage : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<script>
    // Scroll to message div if existing 
    $(document).ready(function () {
        if ($("".alert-dismissible"").length) {
            $(document).scrollTop($("".alert-dismissible"").offset().top -100);
        }
    });
</script>

<!-- If any exists all together -->
");
#nullable restore
#line 11 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml"
 if (TempData["SuccessMessage"] != null || TempData["WarningMessage"] != null || TempData["ErrorMessage"] != null || TempData["InfoMessage"] != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"col-md-12 w-100 text-center p-1\">\r\n");
#nullable restore
#line 14 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml"
      
        if (TempData["SuccessMessage"] != null)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"alert alert-success alert-dismissible fade show\" role=\"alert\">\r\n                <strong>");
#nullable restore
#line 18 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml"
                   Write(Html.Raw(TempData["SuccessMessage"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong>\r\n                <button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"alert\" aria-label=\"Close\"></button>\r\n            </div>\r\n");
#nullable restore
#line 21 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml"
        }
        else if (TempData["WarningMessage"] != null)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"alert alert-warning alert-dismissible fade show\" role=\"alert\">\r\n                <strong>");
#nullable restore
#line 25 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml"
                   Write(Html.Raw(TempData["WarningMessage"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong>\r\n                <button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"alert\" aria-label=\"Close\"></button>\r\n            </div>\r\n");
#nullable restore
#line 28 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml"
        }
        else if (TempData["ErrorMessage"] != null)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"alert alert-danger alert-dismissible fade show\" role=\"alert\">\r\n                <strong>");
#nullable restore
#line 32 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml"
                   Write(Html.Raw(TempData["ErrorMessage"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong>\r\n                <button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"alert\" aria-label=\"Close\"></button>\r\n            </div>\r\n");
#nullable restore
#line 35 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml"
        }
        else if (TempData["InfoMessage"] != null)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"alert alert-primary alert-dismissible fade show\" role=\"alert\">\r\n                <strong>");
#nullable restore
#line 39 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml"
                   Write(Html.Raw(TempData["InfoMessage"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong>\r\n                <button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"alert\" aria-label=\"Close\"></button>\r\n            </div>\r\n");
#nullable restore
#line 42 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml"
        }
    

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 45 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\Common\_ProcessMessage.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
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
