using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using AutoFixture;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SortingJobScheduler.Testing.Common.Attributes
{
    public class AutoNSubstituteDataAttribute : AutoDataAttribute
    {
        public AutoNSubstituteDataAttribute()
          : base(() => CreateFixture())
        {
        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
               .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());

            fixture.Customize(new AutoNSubstituteCustomization());

            return fixture;
        }
    }
}
