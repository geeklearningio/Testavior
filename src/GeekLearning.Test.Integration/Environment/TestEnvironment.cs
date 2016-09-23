namespace GeekLearning.Test.Integration.Environment
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
        public TestServer Server { get; }

        public HttpClient Client { get; }

        public IServiceProvider ServiceProvider { get; private set; }

        public TestEnvironment()
        {
            this.Server = this.CreateTestServer();
            this.Client = Server.CreateClient();
        }

        protected virtual TestServer CreateTestServer()
        {
            IStartupConfigurationService externalStartupConfigurationService = new TStartupConfigurationService();
            externalStartupConfigurationService.RegisterExternalStartupConfigured(() => ServiceProvider = externalStartupConfigurationService.ServiceProvider);

            return new TestServer(new WebHostBuilder().ConfigureStartup(externalStartupConfigurationService).UseStartup<TStartup>());
        }
    }
}
