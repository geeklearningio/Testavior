using GeekLearning.Test.Integration.Environment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekLearning.Test.Integration.Sample
{
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
