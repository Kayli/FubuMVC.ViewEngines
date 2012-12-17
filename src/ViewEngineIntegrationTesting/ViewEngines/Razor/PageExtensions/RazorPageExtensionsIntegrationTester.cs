using FubuMVC.Core;
using FubuMVC.TestingHarness;
using FubuTestingSupport;
using NUnit.Framework;

namespace ViewEngineIntegrationTesting.ViewEngines.Razor.PageExtensions
{
    public class RazorPageExtensionsIntegrationTester : FubuRegistryHarness
    {
        protected override void configure(FubuRegistry registry)
        {
            registry.Actions.IncludeType<PageExtensionsController>();
        }

        [Test]
        public void razor_view_renders_appropriately_including_page_extensions()
        {
            var text = endpoints.Get<PageExtensionsController>(x => x.Get())
                .ReadAsText();

            text.ShouldContain("From view");
            text.ShouldContain("From controller");
            text.ShouldContain("With model");
            text.ShouldContain("Without model");
            text.ShouldContain("viewengineintegrationtesting/viewengines/razor/pageextensions/get2\"></a>");
            text.ShouldContain("<div id=\"null\" title=\"\"></div>");
        } 
    }
}