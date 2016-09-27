namespace System.Net.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;

    // http://www.stefanhendriks.com/2016/05/11/integration-testing-your-asp-net-core-app-dealing-with-anti-request-forgery-csrf-formdata-and-cookies/
    public static class PostRequestHelper
    {
        public static HttpRequestMessage CreatePostMessage(this object content, string path, Dictionary<string, string> additionalFormPostBodyData = null)
        {
            var properties = content.GetType().GetProperties();

            Dictionary<string, string> propValues = new Dictionary<string, string>();
            foreach (var prop in properties.Where(p => !p.PropertyType.IsConstructedGenericType))
            {
                propValues.Add($"{prop.Name}", prop.GetValue(content)?.ToString());
            }

            if (additionalFormPostBodyData != null)
            {
                additionalFormPostBodyData.ToList().ForEach(data => propValues.Add(data.Key, data.Value));
            }

            return CreatePostMessage(path, propValues);
        }

        public static HttpRequestMessage CreatePostMessage(string path, Dictionary<string, string> formPostBodyData)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = new FormUrlEncodedContent(ToFormPostData(formPostBodyData))
            };

            return httpRequestMessage;
        }

        private static List<KeyValuePair<string, string>> ToFormPostData(Dictionary<string, string> formPostBodyData)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            formPostBodyData.Keys.ToList().ForEach(key =>
            {
                result.Add(new KeyValuePair<string, string>(key, formPostBodyData[key]));
            });
            return result;
        }
    }
}
