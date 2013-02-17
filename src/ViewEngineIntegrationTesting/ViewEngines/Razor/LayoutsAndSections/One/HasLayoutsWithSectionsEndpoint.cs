namespace ViewEngineIntegrationTesting.ViewEngines.Razor.LayoutsAndSections.One
{
    public class HasLayoutsWithSectionsEndpoint
    {
        public HasLayoutsWithSectionModel Get(HasLayoutsWithSectionInput input)
        {
            return new HasLayoutsWithSectionModel {Message = "Hello from endpoint"};
        }
    }

    public class HasLayoutsWithSectionInput
    {
    }

    public class HasLayoutsWithSectionModel
    {
        public string Message { get; set; }
    }
}