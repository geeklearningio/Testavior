namespace GeekLearning.Test.Integration.Sample.Test
{
    using Data;
    using Environment;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public abstract class BaseTestClass
    {
        protected TestEnvironment<Startup, TestStartupConfigurationService<BloggingContext>> TestEnvironment { get; } =
             new TestEnvironment<Startup, TestStartupConfigurationService<BloggingContext>>(
                        Path.Combine(System.AppContext.BaseDirectory, @"..\..\..\..\..\Sample.Web"));

        protected void CreateBlogs()
        {
            using (var serviceScope = TestEnvironment.ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<Data.BloggingContext>();

                dbContext.Blogs.AddRange(new List<Blog>
                {
                    new Blog { Url = "http://blog1.io" },
                    new Blog { Url = "http://blog2.io" },
                    new Blog { Url = "http://blog3.io" }
                });

                dbContext.SaveChanges();
            }
        }
    }
}
