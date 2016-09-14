using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GeekLearning.Test.Integration.Sample.Data;
using Microsoft.EntityFrameworkCore;
using GeekLearning.Test.Integration.Environment;

namespace GeekLearning.Test.Integration.Sample
{
    public class Startup
    {
        private IStartupConfigurationService externalStartupConfiguration;

        public Startup(IHostingEnvironment env, IStartupConfigurationService externalStartupConfiguration)
        {
            this.externalStartupConfiguration = externalStartupConfiguration;
            this.externalStartupConfiguration.ConfigureEnvironment(env);
        }

        public void ConfigureServices(IServiceCollection services)
        {          
            services.AddMvc();
            
            // Pass configuration (IConfigurationRoot) to the configuration service if needed
            this.externalStartupConfiguration.ConfigureService(services, null);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            this.externalStartupConfiguration.Configure(app, env, loggerFactory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.MapWhen(
                httpContext => httpContext.Request.Path.StartsWithSegments("/api"),
                apiApp => apiApp.UseMvc());

            app.UseMvc();
        }
    }
}
