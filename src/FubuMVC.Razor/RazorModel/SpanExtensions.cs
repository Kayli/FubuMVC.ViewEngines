using System.Collections.Generic;
using System.Linq;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;
using FubuMVC.Razor.Core;

namespace FubuMVC.Razor.RazorModel
{
    public static class SpanExtensions
    {
        public static string Master(this IEnumerable<Span> chunks)
        {
            var retVal = chunks.Select(x => x.CodeGenerator).OfType<SetLayoutCodeGenerator>().FirstValue(x => x.LayoutPath);
            return retVal;
            //var codeBlock = chunks.OfType<CodeSpan>().FirstOrDefault(x => x.Content.Contains("_Layout"));
            //if (codeBlock == null)
            //    return null;
            //var codeBlockContent = codeBlock.Content;
            //var layoutIndex = codeBlockContent.IndexOf("_Layout", StringComparison.Ordinal);
            //var endLayoutIndex = codeBlockContent.IndexOf(';', layoutIndex);
            //var layoutSlice = codeBlockContent.Substring(layoutIndex, endLayoutIndex - layoutIndex);
            //var layoutValueStart = layoutSlice.IndexOf('"') + 1;
            //var layoutValueEnd = layoutSlice.IndexOf('"', layoutValueStart);
            //var layoutName = layoutSlice.Substring(layoutValueStart, layoutValueEnd - layoutValueStart);
            //return layoutName;
        }

        public static string ViewModel(this IEnumerable<Span> chunks)
        {
            return chunks.Select(x => x.CodeGenerator).OfType<SetModelTypeCodeGenerator>().FirstValue(x => x.ModelType);
        }


        public static IEnumerable<string> Namespaces(this IEnumerable<Span> chunks)
        {
            return chunks.Select(x => x.CodeGenerator).OfType<AddImportCodeGenerator>().Select(x => x.Namespace);
        }
    }
}