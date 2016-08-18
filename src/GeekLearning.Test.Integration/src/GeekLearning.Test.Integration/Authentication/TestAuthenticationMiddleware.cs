namespace GeekLearning.Test.Integration.Authentication
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Text.Encodings.Web;

    public class TestAuthenticationMiddleware : AuthenticationMiddleware<TestAuthenticationOptions>
    {
        private readonly RequestDelegate next;

        public TestAuthenticationMiddleware(RequestDelegate next, IOptions<TestAuthenticationOptions> options, ILoggerFactory loggerFactory)
            : base(next, options, loggerFactory, UrlEncoder.Default)
        {
            this.next = next;
        }

        protected override AuthenticationHandler<TestAuthenticationOptions> CreateHandler()
        {
            return new TestAuthenticationHandler();
        }
    }
}
