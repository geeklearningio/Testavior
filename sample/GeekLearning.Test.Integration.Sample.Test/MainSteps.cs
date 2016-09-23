using GeekLearning.Test.Integration.Environment;
using GeekLearning.Test.Integration.Sample.Data;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace GeekLearning.Test.Integration.Sample.Test
{
    [Binding]
    public class MainSteps
    {
        [Given(@"A configured environment")]
        public void GivenAWorkingEnvironment()
        {
            ScenarioContext.Current.Add(
                "TestEnvironment",
                new TestEnvironment<Startup, TestStartupConfigurationService<BloggingContext>>());

            // add additional data configuration here
        }

        [Given(@"the following blogs")]
        public void GivenTheFollowingBlogs(Table table)
        {
            var apiTestEnvironment = ScenarioContext.Current.Get<ITestEnvironment>("TestEnvironment");
            using (var serviceScope = apiTestEnvironment.ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<Data.BloggingContext>();
                var blogSet = table.CreateSet<Data.Blog>();
                dbContext.Blogs.AddRange(blogSet);
                dbContext.SaveChanges();
            }
        }
    }
}
