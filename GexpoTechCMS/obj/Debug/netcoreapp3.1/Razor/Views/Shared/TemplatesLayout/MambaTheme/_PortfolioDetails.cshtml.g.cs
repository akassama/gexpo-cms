#pragma checksum "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\MambaTheme\_PortfolioDetails.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "67ee1f697ccf403494831561e04f7a64be631b3f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_TemplatesLayout_MambaTheme__PortfolioDetails), @"mvc.1.0.view", @"/Views/Shared/TemplatesLayout/MambaTheme/_PortfolioDetails.cshtml")]
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
#line 2 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\MambaTheme\_PortfolioDetails.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\MambaTheme\_PortfolioDetails.cshtml"
using NgoExpoApp.App_Code;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"67ee1f697ccf403494831561e04f7a64be631b3f", @"/Views/Shared/TemplatesLayout/MambaTheme/_PortfolioDetails.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90910fab5413ac91cd52b86ccacc8e61010c2c7f", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_TemplatesLayout_MambaTheme__PortfolioDetails : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<GexpoTechCMS.Models.AppModels.PortfolioModel>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "~/Views/Shared/TemplatesLayout/TempoTheme/Partials/_Head.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "~/Views/Shared/TemplatesLayout/Common/_ShowFBCheck.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "~/Views/Shared/TemplatesLayout/TempoTheme/Partials/_Navigation.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "~/Views/Shared/TemplatesLayout/Common/_ProcessMessage.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("theme-color"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "~/Views/Shared/TemplatesLayout/TempoTheme/Partials/_Footer.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "~/Views/Shared/TemplatesLayout/TempoTheme/Partials/_FooterScripts.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 6 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\MambaTheme\_PortfolioDetails.cshtml"
  
    ViewData["Title"] = Model.PortfolioTitle;
    ViewData["PropertyImage"] = Helpers.BaseUrl(PortfolioHelpers.GetPreviewImage(Model.PortfolioID));
    string active_theme = Helpers.GetActiveTheme();

    string SessionLanguage = Helpers.DefaultData(HttpContextAccessor.HttpContext.Session.GetString("_SessionLanguage"), "en");


    string PortfolioTitle = (SessionLanguage == "en") ? Model.PortfolioTitle : Helpers.GetTranslatableData(Model.PortfolioTitle, SessionLanguage, Model.PortfolioID, "Portfolio", "Title");
    string PortfolioContent = (SessionLanguage == "en") ? Model.PortfolioContent : Helpers.GetTranslatableData(Model.PortfolioContent, SessionLanguage, Model.PortfolioID, "Portfolio", "Content");


#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 19 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\MambaTheme\_PortfolioDetails.cshtml"
 if (active_theme == "Mamba")
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!DOCTYPE html>\r\n    <html lang=\"en\">\r\n\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "67ee1f697ccf403494831561e04f7a64be631b3f8813", async() => {
                WriteLiteral("\r\n        <!-- HEAD PARTIAL -->\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "67ee1f697ccf403494831561e04f7a64be631b3f9112", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "67ee1f697ccf403494831561e04f7a64be631b3f11008", async() => {
                WriteLiteral("\r\n\r\n        <!-- CHECK TO SHOW FB COMMENTS -->\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "67ee1f697ccf403494831561e04f7a64be631b3f11325", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n        <!-- HEAD NAV PARTIAL -->\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "67ee1f697ccf403494831561e04f7a64be631b3f12551", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_2.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n\r\n        <main id=\"main\">\r\n            <!-- Action messages partial -->\r\n            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "67ee1f697ccf403494831561e04f7a64be631b3f13826", async() => {
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
                WriteLiteral("\r\n\r\n            <!-- ======= Breadcrumbs ======= -->\r\n            <section id=\"breadcrumbs\" class=\"breadcrumbs\">\r\n                <div class=\"container\">\r\n\r\n                    <ol>\r\n                        <li>");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "67ee1f697ccf403494831561e04f7a64be631b3f15230", async() => {
                    WriteLiteral("Home");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_5.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("</li>\r\n                        <li><a href=\"/#portfolio\" class=\"theme-color\">Portfolio</a></li>\r\n                        <li>");
#nullable restore
#line 49 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\MambaTheme\_PortfolioDetails.cshtml"
                       Write(Helpers.FormatLongText(PortfolioTitle, 100));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</li>
                    </ol>

                </div>
            </section>
            <!-- End Breadcrumbs -->
            <!-- ======= Portfolio Details Section ======= -->
            <section id=""portfolio-details"" class=""portfolio-details"">
                <div class=""container"">

                    <div class=""row gy-4"">

                        <div class=""col-lg-8"">
                            <div class=""portfolio-details-slider swiper-container"">
                                <div class=""swiper-wrapper align-items-center"">

                                    <div class=""swiper-slide"">
                                        <img");
                BeginWriteAttribute("src", " src=\"", 2699, "\"", 2757, 1);
#nullable restore
#line 66 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\MambaTheme\_PortfolioDetails.cshtml"
WriteAttributeValue("", 2705, PortfolioHelpers.GetPreviewImage(Model.PortfolioID), 2705, 52, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" alt=\"Portfolio Image\">\r\n                                    </div>\r\n\r\n                                    ");
#nullable restore
#line 69 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\MambaTheme\_PortfolioDetails.cshtml"
                               Write(PortfolioHelpers.GetPortfolioImages(Model.PortfolioID));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"

                                </div>
                                <div class=""swiper-pagination""></div>
                            </div>
                        </div>

                        <div class=""col-lg-4"">
                            <div class=""portfolio-description mt-lg-2"">
                                <h2>");
#nullable restore
#line 78 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\MambaTheme\_PortfolioDetails.cshtml"
                               Write(PortfolioTitle);

#line default
#line hidden
#nullable disable
                WriteLiteral("</h2>\r\n                                ");
#nullable restore
#line 79 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\MambaTheme\_PortfolioDetails.cshtml"
                           Write(Html.Raw(PortfolioContent));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                            </div>
                        </div>

                    </div>

                </div>
            </section>
            <!-- End Portfolio Details Section -->

        </main>
        <!-- End #main -->
        <!-- FOOTER PARTIAL -->
        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "67ee1f697ccf403494831561e04f7a64be631b3f20208", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_7.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n        <!-- FOOTER SCRIPTS PARTIAL -->\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "67ee1f697ccf403494831561e04f7a64be631b3f21440", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_8.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_8);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n</html>\r\n");
#nullable restore
#line 100 "C:\Users\Abdoulie Kassama\Documents\Visual Studio 2019\Projects\GexpoTech\GexpoTechCMS\Views\Shared\TemplatesLayout\MambaTheme\_PortfolioDetails.cshtml"
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<GexpoTechCMS.Models.AppModels.PortfolioModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
