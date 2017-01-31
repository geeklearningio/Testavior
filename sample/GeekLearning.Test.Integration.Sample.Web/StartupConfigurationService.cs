namespace GeekLearning.Test.Integration.Sample
{
    using Configuration.Startup;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class StartupConfigurationService : DefaultStartupConfigurationService
    {
        public override void ConfigureService(IServiceCollection services, IConfigurationRoot configuration)
        {
            base.ConfigureService(services, configuration);

            var connection = @"Server=(localdb)\MSSQLLocalDB;Database=Gl.Test.Integration.Sample;Trusted_Connection=True;MultipleActiveResultSets=true";
            
            services.AddDbContext<Data.BloggingContext>(options =>
               options.UseSqlServer(connection));
        }
    }
}
