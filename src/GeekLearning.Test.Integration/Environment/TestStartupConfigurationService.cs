namespace GeekLearning.Test.Integration.Environment
{
    using Authentication;
    using Configuration.Startup;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Mvc;
    using System;
    using System.Security.Claims;

    public class TestStartupConfigurationService<TDbContext> : IStartupConfigurationService
        where TDbContext : DbContext
    {
        private Action externalStartupConfiguredCallback;

        public IServiceProvider ServiceProvider { get; private set; }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ServiceProvider = app.ApplicationServices;

            SetupBuilder(app);

            SetupStore(app);

            app.AddTestFilter<SaveViewModelResultFilter>();

            this.externalStartupConfiguredCallback();
        }

        public virtual void ConfigureEnvironment(IHostingEnvironment env)
        {
            env.EnvironmentName = "Development";
        }

        public virtual void ConfigureService(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped(typeof(ViewModelRepository));

            ConfigureStore(services);

            ConfigureAuthentication(services);
        }

        public void RegisterExternalStartupConfigured(Action callback)
        {
            this.externalStartupConfiguredCallback = callback;
        }

        protected virtual void SetupStore(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<TDbContext>();

                dbContext.Database.OpenConnection();
                dbContext.Database.EnsureCreated();
            }
        }

        protected virtual void SetupBuilder(IApplicationBuilder app)
        {
            app.UseMiddleware<TestAuthenticationMiddleware>();
        }

        protected virtual void ConfigureStore(IServiceCollection services)
        {
            services.AddSingleton<TestAuthenticationOptions>();
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            services.AddDbContext<TDbContext>(options => options.UseSqlite(connection));
        }

        protected virtual void ConfigureAuthentication(IServiceCollection services)
        {
            services.Configure<TestAuthenticationOptions>(o => o.Identity = ConfigureIdentity());
        }

        protected virtual ClaimsIdentity ConfigureIdentity()
        {
            return new ClaimsIdentity(new Claim[]
            {
               new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()),
               new Claim("http://schemas.microsoft.com/identity/claims/tenantid", "test"),
               new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", Guid.NewGuid().ToString()),
               new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "test"),
               new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", "test"),
               new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn", "test"),
               new Claim("http://schemas.microsoft.com/identity/claims/scope", "user_impersonation")
            }, "test");
        }
    }
}
