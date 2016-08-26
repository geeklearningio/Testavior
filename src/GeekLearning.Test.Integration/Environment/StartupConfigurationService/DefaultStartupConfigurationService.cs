namespace GeekLearning.Test.Integration.Environment
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
                throw new NotImplementedException();
            }
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) { }

        public virtual void ConfigureEnvironment(IHostingEnvironment env) { }

        public virtual void ConfigureService(IServiceCollection services, IConfigurationRoot configuration) { }

        public virtual void RegisterExternalStartupConfigured(Action callback)
        {
            throw new NotImplementedException();
        }
    }
}
