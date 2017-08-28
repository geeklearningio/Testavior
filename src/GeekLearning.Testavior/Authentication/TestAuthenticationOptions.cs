namespace GeekLearning.Testavior.Authentication
{
    using Microsoft.AspNetCore.Authentication;
    using System.Security.Claims;

    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public ClaimsIdentity Identity { get; set; }
    }
}
