#pragma checksum "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3cf3492bd52b51030b0832aca1496dd52a7d0672"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Employees_CoupleEmployees), @"mvc.1.0.view", @"/Views/Employees/CoupleEmployees.cshtml")]
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
#line 1 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\_ViewImports.cshtml"
using Couple_Employees;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\_ViewImports.cshtml"
using Couple_Employees.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
using Couple_Employees.ViewModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3cf3492bd52b51030b0832aca1496dd52a7d0672", @"/Views/Employees/CoupleEmployees.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"168bfab0b5ca01ce6565c37f4398016ce56cc269", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Employees_CoupleEmployees : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<PrintViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<section class=""probootstrap-cover overflow-hidden relative"" style=""background-image: url('/img/bg_1.jpg');"" data-stellar-background-ratio=""0.5"" id=""section-home"">
    <div class=""overlay""></div>
    <div class=""container"">

        <h3>Employees worked the longest on common projects</h3>
        <table class=""table table-striped table-bordered text-center text-dark"">
            <tr>
                <th>EmployeeId#1</th>
                <th>EmployeeId#2</th>
                <th>ProjectIds</th>
                <th>Days Worked</th>
            </tr>

");
#nullable restore
#line 17 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
             foreach (var couple in Model.AllEmployeesByProjects)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\n                    <td>");
#nullable restore
#line 20 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
                   Write(couple.Column1Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                    <td>");
#nullable restore
#line 21 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
                   Write(couple.Column2Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                    <td>");
#nullable restore
#line 22 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
                   Write(couple.Column3Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                    <td>");
#nullable restore
#line 23 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
                   Write(couple.Column4Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                </tr>\n");
#nullable restore
#line 25 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
        </table>

        <h3>All employees worked by projects</h3>
        <table class=""table table-striped table-bordered text-center text-dark"">
            <tr>
                <th>Employee ID</th>
                <th>Project ID</th>
                <th>DateFrom</th>
                <th>DateTo</th>
            </tr>

");
#nullable restore
#line 38 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
             foreach (var couple in Model.AllEmployees)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\n                    <td>");
#nullable restore
#line 41 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
                   Write(couple.Column1Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                    <td>");
#nullable restore
#line 42 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
                   Write(couple.Column2Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                    <td>");
#nullable restore
#line 43 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
                   Write(couple.Column3Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                    <td>");
#nullable restore
#line 44 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
                   Write(couple.Column4Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                </tr>\n");
#nullable restore
#line 46 "D:\Projects\joro-paspalev-employees_NEW\Couple_Employees\Views\Employees\CoupleEmployees.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </table>\n\n    </div>\n</section>\n\n\n\n");
        }
        #pragma warning restore 1998
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PrintViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
