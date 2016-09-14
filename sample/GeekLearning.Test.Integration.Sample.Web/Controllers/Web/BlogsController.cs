using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeekLearning.Test.Integration.Sample.Data;

namespace GeekLearning.Test.Integration.Sample.Controllers.Web
{
    [Route("[controller]"), Route("/")]
    public class BlogsController : Controller
    {
        private BloggingContext context;

        public BlogsController(BloggingContext context)
        {
            this.context = context;
        }

        [HttpGet("~/")]
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
