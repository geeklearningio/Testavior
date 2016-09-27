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
            //var r = ScenarioContext.Current.Get<ITestEnvironment>("TestEnvironment").Client.PostAsJsonAntiForgeryAsync("/blogs/create", new Blog()).Result;
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

//namespace System.Net.Http
//{
//    using GeekLearning.Test.Integration.Helpers;
//    using Linq;
//    using Newtonsoft.Json;
//    using System.Threading.Tasks;

//    public static class HttpClientHelper
//    {
//        public static async Task<HttpResponseMessage> PostAsJsonAntiForgeryAsync<TContent>(this HttpClient httpClient, string requestUri, TContent content)
//        {
//            var contentData = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(content));
            
//            var responseMsg = await httpClient.GetAsync(requestUri);
//            var antiForgeryToken = await responseMsg.ExtractAntiForgeryTokenAsync();

//            contentData.Add("__RequestVerificationToken", antiForgeryToken);

//            List<KeyValuePair<string, string>> formUrlEncodedData = new List<KeyValuePair<string, string>>();
//            contentData.Keys.ToList().ForEach(key =>
//            {
//                formUrlEncodedData.Add(new KeyValuePair<string, string>(key, contentData[key]));
//            });
//            var httpContent = new FormUrlEncodedContent(formUrlEncodedData);
            
//            var requestMsg = new HttpRequestMessage(HttpMethod.Post, responseMsg.RequestMessage.RequestUri)
//            {
//                Content = httpContent
//            };

//            CookiesHelper.CopyCookiesFromResponse(requestMsg, responseMsg);

//            return await httpClient.SendAsync(requestMsg);
//        }
//    }
//}

