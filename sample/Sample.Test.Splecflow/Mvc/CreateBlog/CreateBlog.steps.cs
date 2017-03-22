namespace GeekLearning.Test.Integration.Sample.Test.Mvc.CreateBlog
{
    using Environment;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Net.Http;
    using TechTalk.SpecFlow;

    [Binding]
    public class CreateBlogSteps
    {
        [When(@"I create a new blog : '(.*)'")]
        public void WhenICreateANewBlog(string blogUrl)
        {
            ScenarioContext.Current.Get<ITestEnvironment>("TestEnvironment")
                                   .Client
                                   .PostAsJsonAntiForgeryAsync("blogs/create", new Data.Blog { Url = blogUrl }).Wait();
        }

        [Then(@"the blog '(.*)' must be created")]
        public void ThenTheBlogMustBeCreated(string blogUrl)
        {
            using (var serviceScope = ScenarioContext.Current.Get<ITestEnvironment>("TestEnvironment").ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                Assert.IsNotNull(serviceScope.ServiceProvider
                                             .GetService<Data.BloggingContext>()
                                             .Blogs
                                             .FirstOrDefault(b => b.Url == blogUrl));
            }
        }
    }
}
