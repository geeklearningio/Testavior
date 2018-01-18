namespace GeekLearning.Testavior.Authentication
{
    using Microsoft.AspNetCore.Authentication;
    using System;

    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }

        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder, string[] authenticationSchemes, string displayName, Action<TestAuthenticationOptions> configureOptions)
        {
            foreach (var authenticationScheme in authenticationSchemes)
            {
                builder = builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
            }

            return builder;
        }
    }
}
