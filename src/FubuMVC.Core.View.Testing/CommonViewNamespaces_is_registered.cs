using FubuMVC.Core.Registration;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuMVC.Core.View.Testing
{
    [TestFixture]
    public class CommonViewNamespaces_is_registered
    {
        [Test]
        public void is_registered()
        {
            var graph = BehaviorGraph.BuildFrom(x => {
                x.Import<CommonViewNamespacesRegistration>();
            });

            graph.Services.DefaultServiceFor<CommonViewNamespaces>()
                .ShouldNotBeNull();
        }
    }
}