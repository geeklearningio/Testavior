namespace GeekLearning.Testavior.Sample
{
    using GeekLearning.Testavior.Configuration.Startup;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class StartupConfigurationService : DefaultStartupConfigurationService
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            base.ConfigureServices(services, configuration);

            var connection = @"Server=(localdb)\MSSQLLocalDB;Database=Gl.Test.Integration.Sample;Trusted_Connection=True;MultipleActiveResultSets=true";
            
            services.AddDbContext<Data.BloggingContext>(options =>
               options.UseSqlServer(connection));
        }
    }
}
