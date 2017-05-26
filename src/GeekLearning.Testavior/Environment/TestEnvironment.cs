namespace GeekLearning.Testavior.Environment
{
    using Configuration.Startup;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using System;
    using System.Net.Http;

    public class TestEnvironment<TStartup, TStartupConfigurationService> : ITestEnvironment
        where TStartup : class
        where TStartupConfigurationService : class, IStartupConfigurationService, new()
    {
        private string contentRootPath;

        public TestServer Server { get; }

        public HttpClient Client { get; }

        public IServiceProvider ServiceProvider
        {
            get
            {
                return this.Server?.Host?.Services;
            }
        }

        public TestEnvironment(string contentRootPath = null)
        {
            this.contentRootPath = contentRootPath;
            this.Server = this.CreateTestServer();
            this.Client = Server.CreateClient();
        }

        protected virtual TestServer CreateTestServer()
        {
            return new TestServer
            (
                new WebHostBuilder()
                        .ConfigureStartup(new TStartupConfigurationService(), this.contentRootPath)
                        .UseStartup<TStartup>()
            );
        }
    }
}
