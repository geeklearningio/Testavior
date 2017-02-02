namespace System.Net.Http
{
    using Collections.Generic;
    using GeekLearning.Test.Integration.Helpers;
    using Globalization;
    using Linq;
    using Newtonsoft.Json.Linq;
    using System.Threading.Tasks;

    public static class HttpClientHelper
    {
        public static async Task<HttpResponseMessage> PostAsJsonAntiForgeryAsync<TContent>(this HttpClient httpClient, string requestUri, TContent content)
        {
            IDictionary<string, string> contentData = ExtractContent(content);

            HttpResponseMessage responseMsg = await httpClient.GetAsync(requestUri);
            if (!responseMsg.IsSuccessStatusCode)
            {
                return responseMsg;
            }

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
            if (metaToken == null)
            {
                return null;
            }

            JToken token = metaToken as JToken;
            if (token == null)
            {
                return ExtractContent(JObject.FromObject(metaToken));
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (var child in token.Children().ToList())
                {
                    contentData = contentData.Concat(ExtractContent(child)).ToDictionary(k => k.Key, v => v.Value);
                }

                return contentData;
            }

            var jValue = token as JValue;
            var value = jValue?.Type == JTokenType.Date ?
                            jValue?.ToString("o", CultureInfo.InvariantCulture) :
                            jValue?.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, Uri.EscapeDataString(value ?? string.Empty) } };
        }
    }
}