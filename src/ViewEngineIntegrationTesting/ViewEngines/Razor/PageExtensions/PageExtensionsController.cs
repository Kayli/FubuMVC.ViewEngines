namespace ViewEngineIntegrationTesting.ViewEngines.Razor.PageExtensions
{
    public class PageExtensionsController
    {
         public PageExtensionsViewModel Get()
         {
             return new PageExtensionsViewModel
             {
                 Message = "From controller",
             };
         }

        public LinkToViewModel Get2(LinkToInputModel input)
        {
            return new LinkToViewModel();
        }
    }

    public class LinkToInputModel
    {
    }

    public class LinkToViewModel
    {
    }

    public class PageExtensionsViewModel
    {
        public string Message { get; set; }
    }

    public class PartialModel
    {
        public string Message { get; set; }
    }
}