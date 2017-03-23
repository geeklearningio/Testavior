namespace GeekLearning.Test.Integration.Sample.Controllers.Api
{
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/blogs")]
    public class BlogsController : Controller
    {
        private BloggingContext context;

        public BlogsController(BloggingContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Blog>> GeBlogs()
        {
            return await this.context.Blogs.ToListAsync();
        }
    }
}
