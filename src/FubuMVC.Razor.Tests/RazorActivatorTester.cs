using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Bottles;
using Bottles.Diagnostics;
using FubuMVC.Core.UI;
using FubuMVC.Core.View;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;

namespace FubuMVC.Razor.Tests
{
    [TestFixture]
    public class RazorActivatorTester : InteractionContext<RazorActivator>
    {
        private CommonViewNamespaces _commonViewNamespaces;

        protected override void beforeEach()
        {
            _commonViewNamespaces = new CommonViewNamespaces();
            _commonViewNamespaces.Add("Foo");
            _commonViewNamespaces.Add("Bar");

            Services.Inject(_commonViewNamespaces);

            ClassUnderTest.Activate(Enumerable.Empty<IPackageInfo>(), MockFor<IPackageLog>());
        }

        [Test]
        public void default_namespaces_are_set_including_anything_from_CommonViewNamespaces()
        {
            var useNamespaces = _commonViewNamespaces.Namespaces;
            useNamespaces.Each(x => Debug.WriteLine(x));

            useNamespaces.ShouldHaveTheSameElementsAs(new[]
            { 
                "Foo",
                "Bar",
                typeof(VirtualPathUtility).Namespace,
                typeof(RazorViewFacility).Namespace,
                typeof(IPartialInvoker).Namespace,
                typeof(HtmlTag).Namespace,
                typeof(string).Namespace
            });
        }

       
    }
}
