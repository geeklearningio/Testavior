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

namespace GeekLearning.Test.Integration.Sample.Test.GetBlogs.Api
{
    [Binding]
    public class GetBlogsSteps
    {
        [When(@"I get the list of blogs from Api")]
        public void WhenIGetTheListOfBlogs()
        {
            var testEnvironment = ScenarioContext.Current.Get<ITestEnvironment>("TestEnvironment");
            var response = testEnvironment.Client.GetAsync("/api/blogs").Result;
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
