namespace System.Net.Http
{
    using Collections.Generic;
    using GeekLearning.Test.Integration.Helpers.Http;
    using System.Threading.Tasks;

    public static class HttpClientHelper
    {
        public static async Task<HttpResponseMessage> PostAsJsonAntiForgeryAsync<TContent>(this HttpClient httpClient, string requestUri, TContent content)
        {
            // Get the form view
            HttpResponseMessage responseMsg = await httpClient.GetAsync(requestUri);
            if (!responseMsg.IsSuccessStatusCode)
            {
                return responseMsg;
            }

            // Extract Anti Forgery Token
            var antiForgeryToken = await responseMsg.ExtractAntiForgeryTokenAsync();

            // Serialize data to Key/Value pairs
            IDictionary<string, string> contentData = content.ToKeyValue();

            // Create the request message with previously serialized data + the Anti Forgery Token
            contentData.Add("__RequestVerificationToken", antiForgeryToken);
            var requestMsg = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new FormUrlEncodedContent(contentData)
            };

            // Copy the cookies from the response (containing the Anti Forgery Token) to the request that is about to be sent
            requestMsg.CopyCookiesFromResponse(responseMsg);

            return await httpClient.SendAsync(requestMsg);
        }
    }    
}
