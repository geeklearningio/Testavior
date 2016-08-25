namespace GeekLearning.Test.Integration.Environment
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    public class StartupConfigurationService : IStartupConfigurationService
    {
        public IServiceProvider ServiceProvider
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) { }

        public void ConfigureEnvironment(IHostingEnvironment env) { }

        public void ConfigureService(IServiceCollection services, IConfigurationRoot configuration) { }

        public void RegisterExternalStartupConfigured(Action callback)
        {
            throw new NotImplementedException();
        }
    }
}
