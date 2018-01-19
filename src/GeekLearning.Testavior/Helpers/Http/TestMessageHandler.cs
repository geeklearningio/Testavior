using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekLearning.Testavior
{
    public class TestMessageHandler : DelegatingHandler
    {
        private readonly CookieContainer cookies = new System.Net.CookieContainer();

        public TestMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Cookie", this.cookies.GetCookieHeader(request.RequestUri));

            var resp = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (resp.Headers.TryGetValues("Set-Cookie", out var newCookies))
            {
                foreach (var item in SetCookieHeaderValue.ParseList(newCookies.ToList()))
                {
                    if (item.Domain.HasValue)
                    {
                        this.cookies.Add(request.RequestUri, new Cookie(item.Name.Value, item.Value.Value, item.Path.Value, item.Domain.Value));
                    }
                    else
                    {
                        this.cookies.Add(request.RequestUri, new Cookie(item.Name.Value, item.Value.Value, item.Path.Value));
                    }
                }
            }

            return resp;
        }
    }
}
