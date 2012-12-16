using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;
using FubuMVC.Razor.Core;
using FubuMVC.Razor.RazorModel;

namespace FubuMVC.Razor.Registration
{
    public class ViewParser : IViewParser
    {
        public IEnumerable<Span> Parse(string viewFile)
        {
            RazorCodeLanguage language;
            switch (viewFile.FileExtension())
            {
                case ".cshtml":
                    language = new FubuCSharpRazorCodeLanguage();
                    break;
                //case ".vbhtml":
                //    language = new VBRazorCodeLanguage(true);
                //    break;
                default:
                    throw new ArgumentException("Invalid extension for Razor engine.");
            }

            using (var fileStream = new FileStream(viewFile, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fileStream))
            {
                var templateEngine = new RazorTemplateEngine(new RazorEngineHost(new FubuCSharpRazorCodeLanguage()));
                var parseResults = templateEngine.ParseTemplate(reader);
                return parseResults.Document.Flatten();
            }
        }
    }

    public interface IViewParser
    {
        IEnumerable<Span> Parse(string viewFile);
    }
}