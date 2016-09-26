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
using GeekLearning.Test.Integration.Mvc;

namespace GeekLearning.Test.Integration.Sample.Test.GetBlogs.Web
{
    [Binding]
    public class GetBlogsSteps
    {
        [When(@"I get the list of blogs from Web")]
        public void WhenIGetTheListOfBlogs()
        {
            var testEnvironment = ScenarioContext.Current.Get<ITestEnvironment>("TestEnvironment");
            var response = testEnvironment.Client.GetAsync("/").Result;
            response.EnsureSuccessStatusCode();
        }

        [Then(@"the result must be the following model")]
        public void ThenTheResultMustBeTheFollowingList(Table expectedResult)
        {
            var testEnvironment = ScenarioContext.Current.Get<ITestEnvironment>("TestEnvironment");
            var viewModelRepository = testEnvironment.ServiceProvider.GetRequiredService<ViewModelRepository>();
            var blogListResult = expectedResult.CreateSet<Blog>();
        }
    }
}
