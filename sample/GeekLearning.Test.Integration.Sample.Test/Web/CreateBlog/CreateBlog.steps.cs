using GeekLearning.Test.Integration.Environment;
using System.Linq;
using TechTalk.SpecFlow;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GeekLearning.Test.Integration.Sample.Test.Web.CreateBlog
{
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
                Assert.NotNull(serviceScope.ServiceProvider.GetService<Data.BloggingContext>()
                                                           .Blogs
                                                           .FirstOrDefault(b => b.Url == blogUrl));
            }
        }
    }
}
