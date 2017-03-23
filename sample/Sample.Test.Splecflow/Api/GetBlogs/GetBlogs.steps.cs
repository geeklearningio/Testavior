namespace GeekLearning.Test.Integration.Sample.Test.GetBlogs.Api
{
    using Data;
    using Environment;
	using Newtonsoft.Json;
	using System.Collections.Generic;
    using System.Net.Http;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class GetBlogsSteps
    {
        [When(@"I get the list of blogs from Api")]
        public void WhenIGetTheListOfBlogs()
        {
            var testEnvironment = ScenarioContext.Current.Get<ITestEnvironment>("TestEnvironment");
            var response = testEnvironment.Client.GetAsync("/api/blogs").Result;
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<Blog[]>(response.Content.ReadAsStringAsync().Result);

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
