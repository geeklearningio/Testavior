namespace GeekLearning.Test.Integration.Environment
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
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

            return new TestServer(new WebHostBuilder().ConfigureServices(s => s.AddSingleton(externalStartupConfigurationService))
                                                      .UseStartup<TStartup>());
        }
    }
}
