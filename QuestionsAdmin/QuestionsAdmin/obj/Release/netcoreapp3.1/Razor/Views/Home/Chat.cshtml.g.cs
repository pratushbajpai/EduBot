#pragma checksum "C:\github\CS6460\QuestionsAdmin\QuestionsAdmin\Views\Home\Chat.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ccab1b3e6c2db7bf1d2d3650bc9b798a72b60e57"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Chat), @"mvc.1.0.view", @"/Views/Home/Chat.cshtml")]
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
#line 1 "C:\github\CS6460\QuestionsAdmin\QuestionsAdmin\Views\_ViewImports.cshtml"
using QuestionsAdmin;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\github\CS6460\QuestionsAdmin\QuestionsAdmin\Views\_ViewImports.cshtml"
using QuestionsAdmin.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ccab1b3e6c2db7bf1d2d3650bc9b798a72b60e57", @"/Views/Home/Chat.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0e840957b97e43db07fe9adff66388f0a09deced", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Chat : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\github\CS6460\QuestionsAdmin\QuestionsAdmin\Views\Home\Chat.cshtml"
  
    ViewData["Title"] = "Chat with Quiz Bot";

#line default
#line hidden
#nullable disable
            WriteLiteral("<h1>");
#nullable restore
#line 4 "C:\github\CS6460\QuestionsAdmin\QuestionsAdmin\Views\Home\Chat.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n\r\n<p>Use this page to Test your quiz bot.</p>\r\n\r\n<iframe src=\'https://webchat.botframework.com/embed/QuizMeBot?s=dCGXhNdVJJs.yTIoyxaLhPhLWNBpMX90vaoRYI9NxJiguP8TIKlKLlI\' style=\'min-width: 400px; width: 100%; min-height: 500px;\'></iframe>\r\n");
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