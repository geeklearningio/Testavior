﻿namespace GeekLearning.Test.Integration.Environment
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public interface IStartupConfigurationService
    {
        IServiceProvider ServiceProvider { get; }

        void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory);

        void ConfigureEnvironment(IHostingEnvironment env);

        void ConfigureService(IServiceCollection services, IConfigurationRoot configuration);

        void RegisterExternalStartupConfigured(Action callback);
    }
}
