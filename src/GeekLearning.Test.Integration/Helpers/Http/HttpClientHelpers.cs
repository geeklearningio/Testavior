namespace System.Net.Http
{
    using Collections.Generic;
    using GeekLearning.Test.Integration.Helpers;
    using Linq;
    using Newtonsoft.Json.Linq;
    using System.Threading.Tasks;

    public static class HttpClientHelper
    {
        public static async Task<HttpResponseMessage> PostAsJsonAntiForgeryAsync<TContent>(this HttpClient httpClient, string requestUri, TContent content)
        {
            var o = JObject.FromObject(content);
            var contentData = new Dictionary<string, string>();
            foreach (var property in o.Properties())
            {
                ExtractContent(property, contentData);
            }

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

        private static void ExtractContent(JProperty property, IDictionary<string, string> content)
        {
            if (property.HasValues)
            {
                if (property.Value.Type == JTokenType.Array)
                {
                    foreach (var childValue in property.Value.Children<JValue>().ToList())
                    {
                        content[childValue.Path] = childValue.Value.ToString();
                    }
                }
                else if (property.Value.Type == JTokenType.Object)
                {
                    foreach (var childProperty in property.Value.Children<JProperty>())
                    {
                        ExtractContent(childProperty, content);
                    }
                }
                else
                {
                    content[property.Path] = property.Value.ToString();
                }
            }
        }
    }
}