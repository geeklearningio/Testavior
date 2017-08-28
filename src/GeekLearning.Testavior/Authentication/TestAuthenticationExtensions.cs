namespace GeekLearning.Testavior.Authentication
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;
    using System;

    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<TestAuthenticationOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<TestAuthenticationOptions>, TestAuthenticationPostConfigureOptions>());
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
