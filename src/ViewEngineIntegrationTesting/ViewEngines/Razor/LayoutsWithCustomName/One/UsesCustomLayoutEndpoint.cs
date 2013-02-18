using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewEngineIntegrationTesting.ViewEngines.Razor.LayoutsWithCustomName.One
{
    class UsesCustomLayoutEndpoint
    {
        public ViewModel Execute()
        {
            return new ViewModel();
        }
    }

    public class ViewModel { }
}