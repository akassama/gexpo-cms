#pragma checksum "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4ef2cbd1eb21caf82b3c8612b0eb863dff4216c2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_TempoTheme_Partials__Footer), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/TempoTheme/Partials/_Footer.cshtml")]
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
#line 1 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4ef2cbd1eb21caf82b3c8612b0eb863dff4216c2", @"/Views/Shared/TemplatesLayout/TempoTheme/Partials/_Footer.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_TemplatesLayout_TempoTheme_Partials__Footer : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "AddSubscriber", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "~/Views/Shared/TemplatesLayout/Common/_SearchModal.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
  
    string SessionLanguage = Helpers.DefaultData(HttpContextAccessor.HttpContext.Session.GetString("_SessionLanguage"), "en");

    string OrganizationName = SqlHelpers.GetTableData("ContentManagement", "ContentName", "OrganizationName", "ContentValue");
    string OrganizationAddress = SqlHelpers.GetTableData("ContentManagement", "ContentName", "OrganizationAddress", "ContentValue");
    string PhoneNumber = SqlHelpers.GetTableData("ContentManagement", "ContentName", "PhoneNumber", "ContentValue");
    string OptionalPhoneNumber = SqlHelpers.GetTableData("ContentManagement", "ContentName", "OptionalPhoneNumber", "ContentValue");
    string Email = SqlHelpers.GetTableData("ContentManagement", "ContentName", "Email", "ContentValue");

    string FacebookLink = SqlHelpers.GetTableData("ContentManagement", "ContentName", "FacebookLink", "ContentValue");
    string TwitterLink = SqlHelpers.GetTableData("ContentManagement", "ContentName", "TwitterLink", "ContentValue");
    string InstagramLink = SqlHelpers.GetTableData("ContentManagement", "ContentName", "InstagramLink", "ContentValue");
    string YouTubeLink = SqlHelpers.GetTableData("ContentManagement", "ContentName", "YouTubeLink", "ContentValue");
    string LinkedInLink = SqlHelpers.GetTableData("ContentManagement", "ContentName", "LinkedInLink", "ContentValue");

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<!-- ======= Footer ======= -->
<footer id=""footer"" class=""mt-5"">

    <div class=""footer-top"">
        <div class=""container"">
            <div class=""row"">

                <div class=""col-lg-3 col-md-6 footer-contact"">
                    <h3>");
#nullable restore
#line 29 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                   Write(OrganizationName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n                    <p>\r\n                        <strong>Address:</strong> ");
#nullable restore
#line 31 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                                             Write(OrganizationAddress);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        <br>\r\n                        <strong>Phone:</strong> ");
#nullable restore
#line 33 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                                           Write(PhoneNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral("<br>\r\n                        <strong>Email:</strong> ");
#nullable restore
#line 34 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                                           Write(Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("<br>\r\n                    </p>\r\n                </div>\r\n\r\n                <div class=\"col-lg-2 col-md-6 footer-links\">\r\n                    <h4>Footer Navigation</h4>\r\n                    <ul>\r\n                        ");
#nullable restore
#line 41 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                   Write(TempoTemplateHelpers.GetFooterNavagation(SessionLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </ul>\r\n                </div>\r\n\r\n                <div class=\"col-lg-3 col-md-6 footer-links\">\r\n                    <h4>Our Services</h4>\r\n                    <ul>\r\n                        ");
#nullable restore
#line 48 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                   Write(TempoTemplateHelpers.GetFooterServices(SessionLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                    </ul>
                </div>

                <div class=""col-lg-4 col-md-6 footer-newsletter"">
                    <h4>Join Our Newsletter</h4>
                    <p>
                        Subscribe to our newsletter to stay up to date, delivered straight to your inbox.
                    </p>
                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4ef2cbd1eb21caf82b3c8612b0eb863dff4216c210249", async() => {
                WriteLiteral("\r\n                        <input type=\"email\" name=\"Email\" id=\"Email\" required>\r\n                        <input type=\"submit\"");
                BeginWriteAttribute("class", " class=\"", 3084, "\"", 3092, 0);
                EndWriteAttribute();
                WriteLiteral(" value=\"Subscribe\">\r\n                    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                </div>

            </div>
        </div>
    </div>

    <div class=""container d-md-flex py-4"">

        <div class=""me-md-auto text-center text-md-start"">
            <div class=""copyright"">
                &copy; Copyright <strong><span>");
#nullable restore
#line 71 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                                          Write(OrganizationName);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span></strong>. All Rights Reserved
            </div>
            <div class=""credits"">
                Developed & Maintained by <a href=""https://gexpotech.com/"">GexpoTech</a>
            </div>
        </div>
        <div class=""social-links text-center text-md-right pt-3 pt-md-0"">
");
#nullable restore
#line 78 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
             if (CMSHelpers.ContentDisplay("SocialMedia"))
            {
                if (!string.IsNullOrEmpty(TwitterLink))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <a");
            BeginWriteAttribute("href", " href=\"", 3894, "\"", 3913, 1);
#nullable restore
#line 82 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
WriteAttributeValue("", 3901, TwitterLink, 3901, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"twitter\"><i class=\"bx bxl-twitter\"></i></a>\r\n");
#nullable restore
#line 83 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                }
                

#line default
#line hidden
#nullable disable
#nullable restore
#line 84 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                 if (!string.IsNullOrEmpty(FacebookLink))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <a");
            BeginWriteAttribute("href", " href=\"", 4086, "\"", 4106, 1);
#nullable restore
#line 86 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
WriteAttributeValue("", 4093, FacebookLink, 4093, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"facebook\"><i class=\"bx bxl-facebook\"></i></a>\r\n");
#nullable restore
#line 87 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 88 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                 if (!string.IsNullOrEmpty(InstagramLink))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <a");
            BeginWriteAttribute("href", " href=\"", 4282, "\"", 4303, 1);
#nullable restore
#line 90 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
WriteAttributeValue("", 4289, InstagramLink, 4289, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"instagram\"><i class=\"bx bxl-instagram\"></i></a>\r\n");
#nullable restore
#line 91 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 92 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                 if (!string.IsNullOrEmpty(YouTubeLink))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <a");
            BeginWriteAttribute("href", " href=\"", 4479, "\"", 4498, 1);
#nullable restore
#line 94 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
WriteAttributeValue("", 4486, YouTubeLink, 4486, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"google-plus\"><i class=\"bx bxl-youtube\"></i></a>\r\n");
#nullable restore
#line 95 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 96 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                 if (!string.IsNullOrEmpty(LinkedInLink))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <a");
            BeginWriteAttribute("href", " href=\"", 4675, "\"", 4695, 1);
#nullable restore
#line 98 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
WriteAttributeValue("", 4682, LinkedInLink, 4682, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"linkedin\"><i class=\"bx bxl-linkedin\"></i></a>\r\n");
#nullable restore
#line 99 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 99 "C:\Users\Laiman\Documents\Visual Studio 2019\Projects\GexpoTechCMS\GexpoTechCMS\Views\Shared\TemplatesLayout\TempoTheme\Partials\_Footer.cshtml"
                 
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </div>\r\n    </div>\r\n</footer>\r\n<!-- End Footer -->\r\n\r\n<!-- SEARCH MODAL PARTIAL -->\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "4ef2cbd1eb21caf82b3c8612b0eb863dff4216c219246", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
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
