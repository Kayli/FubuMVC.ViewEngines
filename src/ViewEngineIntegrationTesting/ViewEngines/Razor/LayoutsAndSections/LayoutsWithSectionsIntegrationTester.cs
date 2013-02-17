using FubuTestingSupport;
using NUnit.Framework;
using ViewEngineIntegrationTesting.ViewEngines.Razor.LayoutsAndSections.One;

namespace ViewEngineIntegrationTesting.ViewEngines.Razor.LayoutsAndSections
{
    [TestFixture]
    public class LayoutsWithSectionsIntegrationTester : SharedHarnessContext
    {
        [Test]
        public void get_views_with_optional_sections()
        {
            var text = endpoints.Get<HasLayoutsWithSectionsEndpoint>(x => x.Get(new HasLayoutsWithSectionInput()))
                .ReadAsText();

            text.ShouldContain("<header>Header from template.</header>");
            text.ShouldContain("<p>HasLayoutWithSections.cshtml</p>");
        }
    }
}