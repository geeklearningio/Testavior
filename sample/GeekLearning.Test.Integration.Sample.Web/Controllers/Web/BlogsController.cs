namespace GeekLearning.Test.Integration.Sample.Controllers.Web
{
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    [Route("blogs")]
    public class BlogsController : Controller
    {
        private BloggingContext context;

        public BlogsController(BloggingContext context)
        {
            this.context = context;
        }

        [Route("~/")]
        public IActionResult Index()
        {
            return View(context.Blogs.ToList());
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Blog blog)
        {
            if (ModelState.IsValid)
            {
                context.Blogs.Add(blog);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }
    }
}
