using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StreamApp.Models;
using StreamApp.utils;
using System.Diagnostics;

namespace StreamApp.Controllers
{
    public class HomeController : Controller
    {

        Producer producer= new Producer();
        Consumer consumer = new Consumer();


        [HttpGet]
        public IActionResult Index()
        {
            if (isLogged())
            {
                return Redirect("home/room");  
            }

            return View();

        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("username");
            return Redirect("/");
        }

        [HttpPost]
        public JsonResult Index(string username)
        {
            CookieOptions option = new CookieOptions();
            option.HttpOnly = true;
            Response.Cookies.Append("username", username, option);

            return Json(new { success = true });
        }

        public IActionResult Room()
        {
            if (isLogged())
            {
                 return View();
            }

            return Redirect("./");
           
        }

        private bool isLogged()
        {
            return this.ControllerContext.HttpContext.Request.Cookies["username"] != null;
        }

        [HttpPost]
        public JsonResult SendMessage(string message)
        {
            try
            {
                producer.sendMessage(message);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false});
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
