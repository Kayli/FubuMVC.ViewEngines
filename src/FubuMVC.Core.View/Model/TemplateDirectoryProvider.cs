using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuMVC.Core.View.Model.Sharing;

namespace FubuMVC.Core.View.Model
{
    public interface ITemplateDirectoryProvider<T> where T : ITemplateFile
    {
        IEnumerable<string> ReachablesOf(T template);
        IEnumerable<string> SharedViewPathsForOrigin(string origin);
    }

    public class TemplateDirectoryProvider<T> : ITemplateDirectoryProvider<T> where T : ITemplateFile
    {
        private readonly ISharedPathBuilder _builder;
        private readonly ITemplateRegistry<T> _templates;
        private readonly ISharingGraph _graph;

        public TemplateDirectoryProvider(ISharedPathBuilder builder, ITemplateRegistry<T> templates, ISharingGraph graph)
        {
            _builder = builder;
            _templates = templates;
            _graph = graph;
        }

        public IEnumerable<string> ReachablesOf(T template)
        {
            var directories = new List<string>();

            var locals = _builder.BuildBy(template.FilePath, template.RootPath, true);
            directories.AddRange(locals);

            _graph.SharingsFor(template.Origin).Each(sh =>
            {
                var root = _templates.ByOrigin(sh).FirstValue(t => t.RootPath);
                if (root == null) return;

                var sharings = _builder.BuildBy(root);
                directories.AddRange(sharings);
            });

            return directories;
        }

        public IEnumerable<string> SharedViewPathsForOrigin(string origin)
        {
            return _graph.SharingsFor(origin)
                .SelectMany(x => _templates
                                 .ByOrigin(x)
                                 .Select(t => t.ViewPath.DirectoryPath())
                                 .Where(path => _builder.SharedFolderNames.Any(path.EndsWith))
                                 .Select(t => t))
                                 .Distinct();
        }
    }
}