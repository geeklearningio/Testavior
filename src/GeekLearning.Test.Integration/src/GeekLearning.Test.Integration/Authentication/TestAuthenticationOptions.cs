namespace GeekLearning.Test.Integration.Authentication
{
    using Microsoft.AspNetCore.Builder;
    using System.Security.Claims;

    public class TestAuthenticationOptions : AuthenticationOptions
    {
        public ClaimsIdentity Identity { get; set; }

        public TestAuthenticationOptions()
        {
            this.AuthenticationScheme = "TestAuthenticationMiddleware";
            this.AutomaticAuthenticate = true;
            this.AutomaticChallenge = true;
        }
    }
}
