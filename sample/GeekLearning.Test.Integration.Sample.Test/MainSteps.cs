using GeekLearning.Test.Integration.Environment;
using GeekLearning.Test.Integration.Sample.Data;
using TechTalk.SpecFlow;

namespace GeekLearning.Test.Integration.Sample.Test
{
    [Binding]
    public class MainSteps
    {
        [Given(@"A configured environment")]
        public void GivenAWorkingEnvironment()
        {
            ScenarioContext.Current.Add(
                "ApiTestEnvironment",
                new TestEnvironment<Startup, TestStartupConfigurationService<BloggingContext>>());
                        
            // add additional data configuration here
        }
    }
}
