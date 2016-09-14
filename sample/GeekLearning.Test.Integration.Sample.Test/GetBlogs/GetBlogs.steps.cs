using GeekLearning.Test.Integration.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Net.Http;
using GeekLearning.Test.Integration.Sample.Data;
using Microsoft.Extensions.DependencyInjection;

namespace GeekLearning.Test.Integration.Sample.Test.GetBlogs
{
    [Binding]
    public class GetBlogsSteps
    {
        [Given(@"the following blogs")]
        public void GivenTheFollowingBlogs(Table table)
        {
            var apiTestEnvironment = ScenarioContext.Current.Get<ITestEnvironment>("ApiTestEnvironment");
            using (var serviceScope = apiTestEnvironment.ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<Data.BloggingContext>();
                var blogSet = table.CreateSet<Data.Blog>();
                dbContext.Blogs.AddRange(blogSet);
                dbContext.SaveChanges();
            }
        }

        [When(@"I get the list of blogs")]
        public void WhenIGetTheListOfBlogs()
        {
            var apiTestEnvironment = ScenarioContext.Current.Get<ITestEnvironment>("ApiTestEnvironment");
            var response = apiTestEnvironment.Client.GetAsync("/api/blogs").Result;
            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsAsync<Blog[]>().Result;

            ScenarioContext.Current.Add("result", result);
        }

        [Then(@"the result must be the following list")]
        public void ThenTheResultMustBeTheFollowingList(Table expectedResult)
        {
            expectedResult.CreateSet<Blog>()
                          .IsEqual(ScenarioContext.Current.Get<IEnumerable<Blog>>("result"), ignoredProperties: "BlogId");            
        }
    }
}
