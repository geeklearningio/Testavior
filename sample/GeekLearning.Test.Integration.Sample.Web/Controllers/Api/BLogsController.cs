using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeekLearning.Test.Integration.Sample.Data;
using Microsoft.EntityFrameworkCore;

namespace GeekLearning.Test.Integration.Sample.Controllers.Api
{
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
