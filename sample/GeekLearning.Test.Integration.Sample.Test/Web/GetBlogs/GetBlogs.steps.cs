using GeekLearning.Test.Integration.Environment;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using GeekLearning.Test.Integration.Sample.Data;
using Microsoft.Extensions.DependencyInjection;
using GeekLearning.Test.Integration.Mvc;
using System.Net.Http;

namespace GeekLearning.Test.Integration.Sample.Test.GetBlogs.Web
{
    [Binding]
    public class GetBlogsSteps
    {
        [When(@"I get the list of blogs from Web")]
        public void WhenIGetTheListOfBlogs()
        {
            ScenarioContext.Current.Get<ITestEnvironment>("TestEnvironment").Client.GetAsync("/").Result.EnsureSuccessStatusCode();
        }

        [Then(@"the result must be the following model")]
        public void ThenTheResultMustBeTheFollowingList(Table expectedResult)
        {
            expectedResult.CreateSet<Blog>()
                          .IsEqual(
                            ScenarioContext.Current.Get<ITestEnvironment>("TestEnvironment")
                                                   .ServiceProvider
                                                   .GetRequiredService<ViewModelRepository>()
                                                   .Get<List<Blog>>(),
                            ignoredProperties: "BlogId");
        }
    }
}
