using Microsoft.Extensions.Options;

namespace GeekLearning.Testavior.Authentication
{
    public class TestAuthenticationPostConfigureOptions : IPostConfigureOptions<TestAuthenticationOptions>
    {
        public void PostConfigure(string name, TestAuthenticationOptions options)
        {
            
        }
    }
}
