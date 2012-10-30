using System.Collections.Generic;
using System.Web;
using Bottles;
using Bottles.Diagnostics;
using FubuCore;
using FubuMVC.Core.UI;
using FubuMVC.Core.View;
using FubuMVC.Razor.Rendering;
using HtmlTags;
using RazorEngine.Configuration;

namespace FubuMVC.Razor
{
	public class RazorActivator : IActivator
	{
        private readonly ITemplateServiceConfiguration _engine;
	    private readonly CommonViewNamespaces _namespaces;

	    public RazorActivator (ITemplateServiceConfiguration engine, CommonViewNamespaces namespaces)
		{
		    _engine = engine;
		    _namespaces = namespaces;
		}

	    public void Activate (IEnumerable<IPackageInfo> packages, IPackageLog log)
		{
            log.Trace("Running {0}".ToFormat(GetType().Name));
			
            configureRazorSettings(log);
            setEngineDependencies(log);
		}

        private void configureRazorSettings(IPackageLog log)
        {
            _engine.Namespaces.Add(typeof(VirtualPathUtility).Namespace); // System.Web
            _engine.Namespaces.Add(typeof(RazorViewFacility).Namespace); // FubuMVC.Razor
            _engine.Namespaces.Add(typeof(IPartialInvoker).Namespace); // FubuMVC.Core.UI
            _engine.Namespaces.Add(typeof(HtmlTag).Namespace); // HtmlTags  

            _namespaces.Namespaces.Each(x => _engine.Namespaces.Add(x));

            log.Trace("Adding namespaces to RazorSettings:");
            _engine.Namespaces.Each(x => log.Trace("  - {0}".ToFormat(x)));
        }

	    private void setEngineDependencies(IPackageLog log)
	    {
	        ((TemplateServiceConfiguration) _engine).BaseTemplateType = typeof (FubuRazorView);
        }
	}
}