namespace Microsoft.AspNetCore.Hosting
{
    using GeekLearning.Test.Configuration.Startup;
    using Microsoft.Extensions.DependencyInjection;

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureStartup<TStartupConfigurationService>(this IWebHostBuilder builder, string contentRootPath = null)            
            where TStartupConfigurationService : class, IStartupConfigurationService
        {
            if (contentRootPath != null)
            {
                builder.UseContentRoot(contentRootPath);
            }

            builder.ConfigureServices(s => s.AddSingleton<IStartupConfigurationService, TStartupConfigurationService>());
            return builder;
        }

        public static IWebHostBuilder ConfigureStartup<TStartupConfigurationService>(this IWebHostBuilder builder, TStartupConfigurationService configurationService, string contentRootPath = null)
            where TStartupConfigurationService : class, IStartupConfigurationService
        {
            if (contentRootPath != null)
            {
                builder.UseContentRoot(contentRootPath);
            }

            builder.ConfigureServices(s => s.AddSingleton<IStartupConfigurationService>(configurationService));
            return builder;
        }

        public static IWebHostBuilder ConfigureDefaultStartup(this IWebHostBuilder builder, string contentRootPath = null)
        {
            return builder.ConfigureStartup<DefaultStartupConfigurationService>(contentRootPath);
        }
    }
}
