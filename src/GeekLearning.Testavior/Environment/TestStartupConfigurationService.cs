namespace GeekLearning.Testavior.Environment
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
    using System.Linq;
    using System.Security.Claims;

    public class TestStartupConfigurationService<TDbContext> : IStartupConfigurationService
        where TDbContext : DbContext
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IConfigurationRoot configuration)
        {
            ServiceProvider = app.ApplicationServices;

            SetupBuilder(app);

            SetupStore(app);

            app.AddTestFilter<SaveViewModelResultFilter>();
        }

        public virtual void ConfigureEnvironment(IHostingEnvironment env)
        {
            env.EnvironmentName = "Test";
        }

        public virtual void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton(typeof(ViewModelRepository));

            ConfigureStore(services);

            ConfigureAuthentication(services);
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
            app.UseAuthentication();
        }

        protected virtual void ConfigureStore(IServiceCollection services)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            services.AddDbContext<TDbContext>(options => options.UseSqlite(connection));
        }

        protected virtual void ConfigureAuthentication(IServiceCollection services, params string[] authenticationDefaultSchemes)
        {
            if (authenticationDefaultSchemes?.Any() != true)
            {
                authenticationDefaultSchemes = new string[] { "Test" };
            }

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = authenticationDefaultSchemes.First();
            })
            .AddTestAuthentication(authenticationDefaultSchemes, "Test Authentication Scheme", o =>
            {
                o.Identity = ConfigureIdentity();
            });
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
               new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn", "test")
            }, "test");
        }
    }
}
