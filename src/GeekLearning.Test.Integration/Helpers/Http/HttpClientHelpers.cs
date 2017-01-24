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
            IDictionary<string, string> contentData = ExtractContent(content);

            var responseMsg = await httpClient.GetAsync(requestUri);
            var antiForgeryToken = await responseMsg.ExtractAntiForgeryTokenAsync();
            contentData.Add("__RequestVerificationToken", antiForgeryToken);

            var requestMsg = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new FormUrlEncodedContent(contentData)
            };

            CookiesHelper.CopyCookiesFromResponse(requestMsg, responseMsg);

            return await httpClient.SendAsync(requestMsg);
        }

        private static IDictionary<string, string> ExtractContent(object metaToken)
        {
            var token = metaToken as JToken;
            if (token == null)
            {
                return ExtractContent(JObject.FromObject(metaToken));
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (JToken child in token.Children().ToList())
                {
                    contentData = contentData.Concat(ExtractContent(child)).ToDictionary(k => k.Key, v => v.Value);
                }

                return contentData;
            }

            var value = token as JValue;
            switch (value?.Type)
            {
                case null:
                    return new Dictionary<string, string> { { token.Path, null } };
                case JTokenType.Date:
                    return new Dictionary<string, string> { { token.Path, value.ToString("o") } };
                default:
                    return new Dictionary<string, string> { { token.Path, value.ToString() } };
            }
        }
    }
}