using FubuCore;
using FubuMVC.Core.View.Rendering;

namespace FubuMVC.Razor.Rendering
{
    public class LayoutActivation : BasicViewModifier<IFubuRazorView>
    {
        public override IFubuRazorView Modify(IFubuRazorView view)
        {
            if (view.LayoutView != null)
            {
                view.LayoutView.As<IFubuRazorView>().ServiceLocator = view.ServiceLocator;
                Modify(view.LayoutView.As<IFubuRazorView>());
            }
            return view;
        }
    }

    public class FubuPartialRendering : IViewModifier<IFubuRazorView>
    {
        private bool _shouldInvokeAsPartial;

        public bool Applies(IFubuRazorView view)
        {
            if (_shouldInvokeAsPartial)
            {
                return true;
            }

            _shouldInvokeAsPartial = true;
            return false;
        }

        public IFubuRazorView Modify(IFubuRazorView view)
        {
            view.NoLayout();
            return view;
        }
    }
}