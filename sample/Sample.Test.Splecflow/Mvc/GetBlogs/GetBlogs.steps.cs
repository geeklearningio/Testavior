namespace GeekLearning.Test.Integration.Sample.Test.GetBlogs.Mvc
{
    using GeekLearning.Testavior;
    using GeekLearning.Testavior.Environment;
    using GeekLearning.Testavior.Mvc;
    using GeekLearning.Testavior.Sample.Data;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

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

