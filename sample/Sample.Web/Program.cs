namespace GeekLearning.Test.Integration.Sample
{
    using Microsoft.AspNetCore.Hosting;
    using System.IO;

    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .ConfigureStartup<StartupConfigurationService>()
                .Build()
                .Run();
        }
    }
}
