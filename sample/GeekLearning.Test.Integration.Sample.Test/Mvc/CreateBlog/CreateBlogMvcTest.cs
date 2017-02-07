namespace GeekLearning.Test.Integration.Sample.Test.Mvc.CreateBlog
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Net.Http;

    [TestClass]
    public class CreateBlogMvcTest : BaseTestClass
    {
        [TestMethod]
        public void CreateBlogShouldBeOk()
        {
            base.CreateBlogs();

            base.TestEnvironment
                .Client
                .PostAsJsonAntiForgeryAsync("blogs/create", new Data.Blog { Url = "http://blog4.io" }).Wait();

            using (var serviceScope = base.TestEnvironment.ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                Assert.IsNotNull(serviceScope.ServiceProvider
                                             .GetService<Data.BloggingContext>()
                                             .Blogs
                                             .FirstOrDefault(b => b.Url == "http://blog4.io"));
            }
        }
    }
}
