namespace System.Net.Http
{
    using Collections.Generic;
    using GeekLearning.Test.Integration.Helpers;
    using Linq;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public static class HttpClientHelper
    {
        public static async Task<HttpResponseMessage> PostAsJsonAntiForgeryAsync<TContent>(this HttpClient httpClient, string requestUri, TContent content)
        {
            var contentData = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(content));

            var responseMsg = await httpClient.GetAsync(requestUri);
            var antiForgeryToken = await responseMsg.ExtractAntiForgeryTokenAsync();

            contentData.Add("__RequestVerificationToken", antiForgeryToken);

            List<KeyValuePair<string, string>> formUrlEncodedData = new List<KeyValuePair<string, string>>();
            contentData.Keys.ToList().ForEach(key =>
            {
                formUrlEncodedData.Add(new KeyValuePair<string, string>(key, contentData[key]));
            });
            var httpContent = new FormUrlEncodedContent(formUrlEncodedData);

            var requestMsg = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = httpContent
            };

            CookiesHelper.CopyCookiesFromResponse(requestMsg, responseMsg);

            return await httpClient.SendAsync(requestMsg);
        }
    }
}