using HW_04_30_2025.Data;
using HW_04_30_2025.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace HW_04_30_2025.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly string _connectionString;
        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            var repo = new Repository(_connectionString);

            return View(new IndexModel
            {
                Images = repo.GetAll()
            });
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file, string title)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

            using var stream = new FileStream(fullPath, FileMode.CreateNew);
            file.CopyTo(stream);

            var repo = new Repository(_connectionString);
            repo.Add(title, fileName);

            return Redirect("/");
        }

        public IActionResult ViewImage(int id)
        {
            var repo = new Repository(_connectionString);
            var image = repo.GetById(id);

            var liked = HttpContext.Session.Get<List<int>>("liked") ?? new List<int>();
            ViewBag.HasLiked = liked.Contains(id);

            return View(image);
        }

        [HttpPost]
        public IActionResult Like(int id)
        {
            var liked = HttpContext.Session.Get<List<int>>("liked") ?? new List<int>();
            if (!liked.Contains(id))
            {
                liked.Add(id);
                HttpContext.Session.Set("liked", liked);

                var repo = new Repository(_connectionString);
                repo.IncrementLikes(id);
            }
            return Json(new { likes = liked });
        }

        public IActionResult GetLikes(int id)
        {
            var repo = new Repository(_connectionString);
            int likes = repo.GetLikes(id);
            return Json(new { likes });
        }



    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}
