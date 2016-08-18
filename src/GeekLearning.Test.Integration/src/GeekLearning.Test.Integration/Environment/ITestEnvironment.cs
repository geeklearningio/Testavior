namespace GeekLearning.Test.Integration.Environment
{
    using Microsoft.AspNetCore.TestHost;
    using System;
    using System.Net.Http;

    public interface ITestEnvironment
    {
        TestServer Server { get; }

        HttpClient Client { get; }

        IServiceProvider ServiceProvider { get; }
    }
}
