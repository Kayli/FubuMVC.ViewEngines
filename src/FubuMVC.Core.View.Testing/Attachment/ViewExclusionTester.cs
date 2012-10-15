﻿using System.Collections.Generic;
using FubuMVC.Core.Registration;
using NUnit.Framework;
using System.Linq;
using FubuTestingSupport;

namespace FubuMVC.Core.View.Testing.Attachment
{
    [TestFixture]
    public class ViewExclusionTester
    {
        [Test]
        public void do_not_use_the_excluded_views()
        {
            var registry = new FubuRegistry();
            registry.AlterSettings<ViewEngines>(x => {
                x.AddFacility(new FakeViewEngine1());
                x.AddFacility(new FakeViewEngine2());

                x.ExcludeViews(v => v.Name().StartsWith("A"));
                x.ExcludeViews(v => v.Name().StartsWith("C"));
            });

            var views = BehaviorGraph.BuildFrom(registry).Settings.Get<ViewEngines>().Views;

            views.Views.OrderBy(x => x.Name()).Select(x => x.Name())
                .ShouldHaveTheSameElementsAs("B1", "B2", "B3", "B4", "B5", "B6");
        }
    }

    public class FakeViewEngine1 : IViewFacility
    {
        public IEnumerable<IViewToken> FindViews(BehaviorGraph graph)
        {
            yield return new FakeViewToken{ViewName = "A1"};
            yield return new FakeViewToken{ViewName = "A2"};
            yield return new FakeViewToken{ViewName = "A3"};
            yield return new FakeViewToken{ViewName = "B1"};
            yield return new FakeViewToken{ViewName = "B2"};
            yield return new FakeViewToken{ViewName = "B3"};
            yield return new FakeViewToken{ViewName = "C1"};
            yield return new FakeViewToken{ViewName = "C2"};
            yield return new FakeViewToken{ViewName = "C3"};
            yield return new FakeViewToken{ViewName = "C4"};
        }
    }

    public class FakeViewEngine2 : IViewFacility
    {
        public IEnumerable<IViewToken> FindViews(BehaviorGraph graph)
        {
            yield return new FakeViewToken { ViewName = "A4" };
            yield return new FakeViewToken { ViewName = "A5" };
            yield return new FakeViewToken { ViewName = "A6" };
            yield return new FakeViewToken { ViewName = "B4" };
            yield return new FakeViewToken { ViewName = "B5" };
            yield return new FakeViewToken { ViewName = "B6" };
            yield return new FakeViewToken { ViewName = "C5" };
            yield return new FakeViewToken { ViewName = "C6" };
            yield return new FakeViewToken { ViewName = "C7" };
            yield return new FakeViewToken { ViewName = "C8" };
        }
    }
}