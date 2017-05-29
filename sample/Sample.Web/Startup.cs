namespace GeekLearning.Testavior.Sample
{
    using GeekLearning.Testavior.Configuration.Startup;
    using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc.Authorization;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

public class Startup
{
    private IStartupConfigurationService externalStartupConfiguration;

    public Startup(IHostingEnvironment env, IStartupConfigurationService externalStartupConfiguration = null)
    {
        this.externalStartupConfiguration = externalStartupConfiguration;
        this.externalStartupConfiguration.ConfigureEnvironment(env);
    }

    public void ConfigureServices(IServiceCollection services)
    {
		services
			.AddMvc(c => c.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())))
			.AddFilterCollection();

        // Pass configuration (IConfigurationRoot) to the configuration service if needed
        this.externalStartupConfiguration.ConfigureServices(services, null);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        this.externalStartupConfiguration.Configure(app, env, loggerFactory);

        using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            serviceScope.ServiceProvider.GetService<Data.BloggingContext>().Database.EnsureCreated();
        }

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
