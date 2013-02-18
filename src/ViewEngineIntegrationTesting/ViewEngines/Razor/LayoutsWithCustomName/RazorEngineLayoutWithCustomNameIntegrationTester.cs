using FubuMVC.Core;
using ViewEngineIntegrationTesting.ViewEngines.Razor.LayoutsWithCustomName.One;
using FubuMVC.TestingHarness;
using NUnit.Framework;
using FubuTestingSupport;

namespace ViewEngineIntegrationTesting.ViewEngines.Razor.LayoutsWithCustomName
{
    [TestFixture]
    public class RazorEngineLayoutWithCustomNameIntegrationTester : FubuRegistryHarness
    {
        protected override void configure(FubuRegistry registry)
        {
            registry.Actions.IncludeType<UsesCustomLayoutEndpoint>();
        }

        [Test]
        public void uses_closest_layout_with_custom_name()
        {
            var text = endpoints.Get<UsesCustomLayoutEndpoint>(x => x.Execute())
                .ReadAsText();

            text.ShouldContain("<h2>CustomLayout.cshtml</h2>");
            text.ShouldContain("<h1>UsesCustomLayout.cshtml</h1>");
        }
    }
}