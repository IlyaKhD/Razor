#pragma checksum "DynamicAttributeTagHelpers.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "782463195265ee647cc2fc63fd5095a80090845b"
namespace TestOutput
{
    using Microsoft.AspNet.Razor.Runtime.TagHelpers;
    using System;
    using System.Threading.Tasks;

    public class DynamicAttributeTagHelpers
    {
        #line hidden
        #pragma warning disable 0414
        private TagHelperContent __tagHelperStringValueBuffer = null;
        #pragma warning restore 0414
        private TagHelperExecutionContext __tagHelperExecutionContext = null;
        private TagHelperRunner __tagHelperRunner = null;
        private TagHelperScopeManager __tagHelperScopeManager = new TagHelperScopeManager();
        private InputTagHelper __InputTagHelper = null;
        #line hidden
        public DynamicAttributeTagHelpers()
        {
        }

        #pragma warning disable 1998
        public override async Task ExecuteAsync()
        {
            __tagHelperRunner = __tagHelperRunner ?? new TagHelperRunner();
            Instrumentation.BeginContext(33, 2, true);
            WriteLiteral("\r\n");
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", TagMode.SelfClosing, "test", async() => {
            }
            , StartTagHelperWritingScope, EndTagHelperWritingScope);
            __InputTagHelper = CreateTagHelper<InputTagHelper>();
            __tagHelperExecutionContext.Add(__InputTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "unbound", 2);
            AddHtmlAttributeValue("", 51, "prefix", 51, 6, true);
            AddHtmlAttributeValue(" ", 57, DateTime.Now, 58, 14, false);
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.Output = await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            Instrumentation.BeginContext(35, 40, false);
            await WriteTagHelperAsync(__tagHelperExecutionContext);
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            Instrumentation.BeginContext(75, 4, true);
            WriteLiteral("\r\n\r\n");
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", TagMode.SelfClosing, "test", async() => {
            }
            , StartTagHelperWritingScope, EndTagHelperWritingScope);
            __InputTagHelper = CreateTagHelper<InputTagHelper>();
            __tagHelperExecutionContext.Add(__InputTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "unbound", 2);
            AddHtmlAttributeValue("", 95, new Template((__razor_attribute_value_writer) => {
#line 5 "DynamicAttributeTagHelpers.cshtml"
                 if (true) { 

#line default
#line hidden

                Instrumentation.BeginContext(109, 12, false);
#line 5 "DynamicAttributeTagHelpers.cshtml"
WriteTo(__razor_attribute_value_writer, string.Empty);

#line default
#line hidden
                Instrumentation.EndContext();
#line 5 "DynamicAttributeTagHelpers.cshtml"
                                           } else { 

#line default
#line hidden

                Instrumentation.BeginContext(132, 5, false);
#line 5 "DynamicAttributeTagHelpers.cshtml"
             WriteTo(__razor_attribute_value_writer, false);

#line default
#line hidden
                Instrumentation.EndContext();
#line 5 "DynamicAttributeTagHelpers.cshtml"
                                                           }

#line default
#line hidden

            }
            ), 95, 44, false);
            AddHtmlAttributeValue(" ", 139, "suffix", 140, 7, true);
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.Output = await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            Instrumentation.BeginContext(79, 71, false);
            await WriteTagHelperAsync(__tagHelperExecutionContext);
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            Instrumentation.BeginContext(150, 4, true);
            WriteLiteral("\r\n\r\n");
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", TagMode.SelfClosing, "test", async() => {
            }
            , StartTagHelperWritingScope, EndTagHelperWritingScope);
            __InputTagHelper = CreateTagHelper<InputTagHelper>();
            __tagHelperExecutionContext.Add(__InputTagHelper);
            StartTagHelperWritingScope();
            WriteLiteral("prefix ");
#line 7 "DynamicAttributeTagHelpers.cshtml"
         WriteLiteral(DateTime.Now);

#line default
#line hidden
            WriteLiteral(" suffix");
            __tagHelperStringValueBuffer = EndTagHelperWritingScope();
            __InputTagHelper.Bound = __tagHelperStringValueBuffer.GetContent(HtmlEncoder);
            __tagHelperExecutionContext.AddTagHelperAttribute("bound", __InputTagHelper.Bound);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "unbound", 3);
            AddHtmlAttributeValue("", 206, "prefix", 206, 6, true);
            AddHtmlAttributeValue(" ", 212, DateTime.Now, 213, 14, false);
            AddHtmlAttributeValue(" ", 226, "suffix", 227, 7, true);
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.Output = await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            Instrumentation.BeginContext(154, 83, false);
            await WriteTagHelperAsync(__tagHelperExecutionContext);
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            Instrumentation.BeginContext(237, 4, true);
            WriteLiteral("\r\n\r\n");
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", TagMode.SelfClosing, "test", async() => {
            }
            , StartTagHelperWritingScope, EndTagHelperWritingScope);
            __InputTagHelper = CreateTagHelper<InputTagHelper>();
            __tagHelperExecutionContext.Add(__InputTagHelper);
            StartTagHelperWritingScope();
#line 9 "DynamicAttributeTagHelpers.cshtml"
  WriteLiteral(long.MinValue);

#line default
#line hidden
            WriteLiteral(" ");
#line 9 "DynamicAttributeTagHelpers.cshtml"
                              if (true) { 

#line default
#line hidden

#line 9 "DynamicAttributeTagHelpers.cshtml"
                              WriteLiteral(string.Empty);

#line default
#line hidden
#line 9 "DynamicAttributeTagHelpers.cshtml"
                                                        } else { 

#line default
#line hidden

#line 9 "DynamicAttributeTagHelpers.cshtml"
                                                     WriteLiteral(false);

#line default
#line hidden
#line 9 "DynamicAttributeTagHelpers.cshtml"
                                                                        }

#line default
#line hidden

            WriteLiteral(" ");
#line 9 "DynamicAttributeTagHelpers.cshtml"
                                                              WriteLiteral(int.MaxValue);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndTagHelperWritingScope();
            __InputTagHelper.Bound = __tagHelperStringValueBuffer.GetContent(HtmlEncoder);
            __tagHelperExecutionContext.AddTagHelperAttribute("bound", __InputTagHelper.Bound);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "unbound", 3);
            AddHtmlAttributeValue("", 347, long.MinValue, 347, 14, false);
            AddHtmlAttributeValue(" ", 361, new Template((__razor_attribute_value_writer) => {
#line 10 "DynamicAttributeTagHelpers.cshtml"
                                if (true) { 

#line default
#line hidden

                Instrumentation.BeginContext(376, 12, false);
#line 10 "DynamicAttributeTagHelpers.cshtml"
     WriteTo(__razor_attribute_value_writer, string.Empty);

#line default
#line hidden
                Instrumentation.EndContext();
#line 10 "DynamicAttributeTagHelpers.cshtml"
                                                          } else { 

#line default
#line hidden

                Instrumentation.BeginContext(399, 5, false);
#line 10 "DynamicAttributeTagHelpers.cshtml"
                            WriteTo(__razor_attribute_value_writer, false);

#line default
#line hidden
                Instrumentation.EndContext();
#line 10 "DynamicAttributeTagHelpers.cshtml"
                                                                          }

#line default
#line hidden

            }
            ), 362, 45, false);
            AddHtmlAttributeValue(" ", 406, int.MaxValue, 407, 14, false);
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.Output = await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            Instrumentation.BeginContext(241, 183, false);
            await WriteTagHelperAsync(__tagHelperExecutionContext);
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            Instrumentation.BeginContext(424, 4, true);
            WriteLiteral("\r\n\r\n");
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", TagMode.SelfClosing, "test", async() => {
            }
            , StartTagHelperWritingScope, EndTagHelperWritingScope);
            __InputTagHelper = CreateTagHelper<InputTagHelper>();
            __tagHelperExecutionContext.Add(__InputTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "unbound", 5);
            AddHtmlAttributeValue("", 444, long.MinValue, 444, 14, false);
            AddHtmlAttributeValue(" ", 458, DateTime.Now, 459, 14, false);
            AddHtmlAttributeValue(" ", 472, "static", 473, 7, true);
            AddHtmlAttributeValue("    ", 479, "content", 483, 11, true);
            AddHtmlAttributeValue(" ", 490, int.MaxValue, 491, 14, false);
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.Output = await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            Instrumentation.BeginContext(428, 80, false);
            await WriteTagHelperAsync(__tagHelperExecutionContext);
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            Instrumentation.BeginContext(508, 4, true);
            WriteLiteral("\r\n\r\n");
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", TagMode.SelfClosing, "test", async() => {
            }
            , StartTagHelperWritingScope, EndTagHelperWritingScope);
            __InputTagHelper = CreateTagHelper<InputTagHelper>();
            __tagHelperExecutionContext.Add(__InputTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "unbound", 1);
            AddHtmlAttributeValue("", 528, new Template((__razor_attribute_value_writer) => {
#line 14 "DynamicAttributeTagHelpers.cshtml"
                 if (true) { 

#line default
#line hidden

                Instrumentation.BeginContext(542, 12, false);
#line 14 "DynamicAttributeTagHelpers.cshtml"
WriteTo(__razor_attribute_value_writer, string.Empty);

#line default
#line hidden
                Instrumentation.EndContext();
#line 14 "DynamicAttributeTagHelpers.cshtml"
                                           } else { 

#line default
#line hidden

                Instrumentation.BeginContext(565, 5, false);
#line 14 "DynamicAttributeTagHelpers.cshtml"
             WriteTo(__razor_attribute_value_writer, false);

#line default
#line hidden
                Instrumentation.EndContext();
#line 14 "DynamicAttributeTagHelpers.cshtml"
                                                           }

#line default
#line hidden

            }
            ), 528, 44, false);
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.Output = await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            Instrumentation.BeginContext(512, 64, false);
            await WriteTagHelperAsync(__tagHelperExecutionContext);
            Instrumentation.EndContext();
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
        }
        #pragma warning restore 1998
    }
}
