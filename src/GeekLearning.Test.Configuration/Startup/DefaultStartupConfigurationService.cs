namespace GeekLearning.Test.Configuration.Startup
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    public class DefaultStartupConfigurationService : IStartupConfigurationService
    {
        public IServiceProvider ServiceProvider
        {
            get
            {
                return null;
            }
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) { }

        public virtual void ConfigureEnvironment(IHostingEnvironment env) { }

        public virtual void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration) { }
    }
}
