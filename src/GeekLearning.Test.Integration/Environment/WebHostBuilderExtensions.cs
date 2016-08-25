namespace GeekLearning.Test.Integration.Environment
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureStartup<TStartupConfigurationService>(this IWebHostBuilder builder)            
            where TStartupConfigurationService : class, IStartupConfigurationService
        {
            builder.ConfigureServices(s => s.AddSingleton<IStartupConfigurationService, TStartupConfigurationService>());
            return builder;
        }

        public static IWebHostBuilder ConfigureStartup<TStartupConfigurationService>(this IWebHostBuilder builder, TStartupConfigurationService configurationService)
            where TStartupConfigurationService : class, IStartupConfigurationService
        {
            builder.ConfigureServices(s => s.AddSingleton<IStartupConfigurationService>(configurationService));
            return builder;
        }

        public static IWebHostBuilder ConfigureDefaultStartup(this IWebHostBuilder builder)
        {
            return builder.ConfigureStartup<DefaultStartupConfigurationService>();
        }
    }
}
