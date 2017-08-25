namespace GeekLearning.Testavior.Sample.Test
{
    using Data;
    using Environment;
    using Microsoft.Extensions.DependencyInjection;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

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
