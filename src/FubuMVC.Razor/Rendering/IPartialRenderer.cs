using System.Web;
using FubuCore;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.View.Model;
using FubuMVC.Core.View.Rendering;
using FubuMVC.Razor.RazorModel;

namespace FubuMVC.Razor.Rendering
{
    public interface IPartialRenderer
    {
        HtmlString Render(IFubuRazorView view, string name);
        HtmlString Render(IFubuRazorView view, string name, object model);
    }

    public class PartialRenderer : IPartialRenderer
    {
        private readonly ISharedTemplateLocator<IRazorTemplate> _sharedTemplateLocator;
        private readonly ITemplateFactory _templateFactory;
        private readonly IViewModifierService<IFubuRazorView> _viewModifierService;
        private readonly IFubuRequest _fubuRequest;

        public PartialRenderer(ISharedTemplateLocator<IRazorTemplate> sharedTemplateLocator, ITemplateFactory templateFactory, IViewModifierService<IFubuRazorView> viewModifierService, IFubuRequest fubuRequest)
        {
            _sharedTemplateLocator = sharedTemplateLocator;
            _templateFactory = templateFactory;
            _viewModifierService = viewModifierService;
            _fubuRequest = fubuRequest;
        }

        public HtmlString Render(IFubuRazorView view, string name)
        {
            return renderInternal(view, name);
        }

        public HtmlString Render(IFubuRazorView view, string name, object model)
        {
            _fubuRequest.Set(model);
            return renderInternal(view, name);
        }

        private HtmlString renderInternal(IFubuRazorView view, string name)
        {
            var template = _sharedTemplateLocator.LocatePartial(name, view.OriginTemplate);
            var partialView = _templateFactory.GetView(template.Descriptor.As<ViewDescriptor<IRazorTemplate>>());

            partialView = _viewModifierService.Modify(partialView);
            partialView.Execute();
            return new HtmlString(partialView.Result.ToString());
        }
    }
}