using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Razor;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser;
using System.Web.Razor.Parser.SyntaxTree;
using FubuCore;
using FubuCore.Util;
using FubuMVC.Core.View;
using FubuMVC.Core.View.Model;
using FubuMVC.Razor.Core;
using FubuMVC.Razor.RazorModel;
using FubuMVC.Razor.Rendering;

namespace FubuMVC.Razor
{
    public interface ITemplateFactory
    {
        IFubuRazorView GetView(ViewDescriptor<IRazorTemplate> descriptor);
    }

    public class TemplateFactoryCache : ITemplateFactory
    {
        private readonly CommonViewNamespaces _commonViewNamespaces;
        private readonly RazorEngineSettings _razorEngineSettings;
        private readonly Cache<string, long> _lastModifiedCache;
        private readonly IDictionary<string, Type> _cache;

        public TemplateFactoryCache(CommonViewNamespaces commonViewNamespaces, RazorEngineSettings razorEngineSettings)
        {
            _commonViewNamespaces = commonViewNamespaces;
            _razorEngineSettings = razorEngineSettings;
            _cache = new Dictionary<string, Type>();
            _lastModifiedCache = new Cache<string, long>(name => name.LastModified());
        }

        public IFubuRazorView GetView(ViewDescriptor<IRazorTemplate> descriptor)
        {
            Type viewType;
            var filePath = descriptor.Template.FilePath;
            _cache.TryGetValue(filePath, out viewType);
            var lastModified = filePath.LastModified();
            if (viewType == null || (_lastModifiedCache[filePath] != lastModified))
            {
                viewType = getViewType(descriptor);
                lock (_cache)
                {
                    _cache[filePath] = viewType;
                    _lastModifiedCache[filePath] = lastModified;
                }
            }
            return Activator.CreateInstance(viewType).As<IFubuRazorView>();
        }

        private Type getViewType(ViewDescriptor<IRazorTemplate> descriptor)
        {
             var className = ParserHelpers.SanitizeClassName(descriptor.ViewPath);
            var baseTemplateType = _razorEngineSettings.BaseTemplateType;
            var generatedClassContext = new GeneratedClassContext("Execute", "Write", "WriteLiteral", null, null, className, "DefineSection");
            var host = new RazorEngineHost(new FubuCSharpRazorCodeLanguage())
            {
                DefaultBaseClass = baseTemplateType.FullName,
                DefaultNamespace = "FubuMVC.Razor.GeneratedTemplates",
                GeneratedClassContext = generatedClassContext
            };
            host.NamespaceImports.UnionWith(_commonViewNamespaces.Namespaces);
            host.NamespaceImports.Add("System");
            var engine = new RazorTemplateEngine(host);
            GeneratorResults results;
            using (var fileStream = new FileStream(descriptor.Template.FilePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fileStream))
            {
                results = engine.GenerateCode(reader, className, host.DefaultNamespace, descriptor.ViewPath);
            }

            if (!results.Success)
            {
                throw CreateExceptionFromParserError(results.ParserErrors.Last(), descriptor.Name());
            }

            var compilerParameters = new CompilerParameters {GenerateInMemory = true, CompilerOptions = "/optimize"};
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamic)
                .Each(x => compilerParameters.ReferencedAssemblies.Add(x.Location));

            CompilerResults compilerResults;
            using (var codeDomProvider = Activator.CreateInstance(host.CodeLanguage.CodeDomProviderType).As<CodeDomProvider>())
            {
                compilerResults = codeDomProvider.CompileAssemblyFromDom(compilerParameters, results.GeneratedCode);
                if (compilerResults.Errors.HasErrors)
                {
                    using (var sw = new StringWriter())
                    using (var tw = new IndentedTextWriter(sw, "    "))
                    {
                        codeDomProvider.GenerateCodeFromCompileUnit(results.GeneratedCode, tw, new CodeGeneratorOptions());
                        var source = sw.ToString();
                        throw CreateExceptionFromCompileError(compilerResults, source);
                    }
                }
            }

            var templateTypeName = "{0}.{1}".ToFormat(host.DefaultNamespace, className);
            var templateType = compilerResults.CompiledAssembly.GetType(templateTypeName);
            return templateType;
        }

        private static HttpParseException CreateExceptionFromParserError(RazorError error, string virtualPath)
        {
            return new HttpParseException(error.Message + Environment.NewLine, null, virtualPath, null, error.Location.LineIndex + 1);
        }

        private static HttpCompileException CreateExceptionFromCompileError(CompilerResults compilerResults, string source)
        {
            var message = string.Join("{0}{0}".ToFormat(Environment.NewLine), compilerResults
                                                                                  .Errors
                                                                                  .OfType<CompilerError>()
                                                                                  .Where(x => !x.IsWarning)
                                                                                  .Select(error =>
                                                                                          "Compile error at {0}{1}line {2}: {1}compile error: {3}: {4}"
                                                                                          .ToFormat(error.FileName,
                                                                                                    Environment.NewLine,
                                                                                                    error.Line,
                                                                                                    error.ErrorNumber,
                                                                                                    error.ErrorText))
                                                                                  .ToArray());
            return new HttpCompileException("{0}{1}{2}".ToFormat(message, Environment.NewLine, source));
        }
    }
}