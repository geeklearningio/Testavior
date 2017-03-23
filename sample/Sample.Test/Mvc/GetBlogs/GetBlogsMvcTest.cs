namespace GeekLearning.Test.Integration.Sample.Test.Mvc.GetBlogs
{
    using GeekLearning.Test.Integration.Mvc;
    using GeekLearning.Test.Integration.Sample.Data;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;

    [TestClass]
    public class GetBlogsMvcTest : BaseTestClass
    {
        [TestMethod]
        public void Mvc_GetBlogsShouldBeOk()
        {
            base.CreateBlogs();

            base.TestEnvironment.Client.GetAsync("/").Result.EnsureSuccessStatusCode();

            var expectedResult = new List<Blog>
            {
                new Blog { Url = "http://blog1.io" },
                new Blog { Url = "http://blog2.io" },
                new Blog { Url = "http://blog3.io" }
            };

            expectedResult.IsEqual(
                            base.TestEnvironment
                                .ServiceProvider
                                .GetRequiredService<ViewModelRepository>()
                                .Get<List<Blog>>(),
                            ignoredProperties: "BlogId");
        }
    }
}
