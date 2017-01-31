namespace Microsoft.Extensions.DependencyInjection
{
    using GeekLearning.Test.Configuration.Mvc;
    using System.Linq;

    public static class FilterExtensions
    {
        public static IMvcBuilder AddFilterCollection(this IMvcBuilder mvcBuilder)
        {
            var filterCollection = new FilterInceptionCollection();

            mvcBuilder.Services.Add(ServiceDescriptor.Singleton(filterCollection));

            mvcBuilder.AddMvcOptions(options =>
            {
                filterCollection.ToList().ForEach(ft => options.Filters.Add(ft));
            });

            return mvcBuilder;
        }
    }
}

namespace Microsoft.AspNetCore.Builder
{
    using GeekLearning.Test.Configuration.Mvc;
    using Mvc.Filters;

    public static class FilterExtensions
    {
        public static FilterInceptionCollection AddTestFilter<TFilterType>(this IApplicationBuilder app)
            where TFilterType : IFilterMetadata
        {
            var filterCollection = app.ApplicationServices.GetService(typeof(FilterInceptionCollection)) as FilterInceptionCollection;

            if (filterCollection != null)
            {
                filterCollection.Add(typeof(TFilterType));
            }

            return filterCollection;
        }
    }
}
