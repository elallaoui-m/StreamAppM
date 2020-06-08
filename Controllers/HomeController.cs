using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StreamApp.Models;
using StreamApp.utils;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StreamApp.Controllers
{
    public class HomeController : Controller
    {


        private readonly ProducerConfig config;
        private readonly IConfiguration configuration;

        public HomeController(ProducerConfig config, IConfiguration configuration)
        {
            this.config = config;
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (isLogged())
            {
                return Redirect("home/room");  
            }

            return View();

        }

        [HttpPost]
        public JsonResult Index(string username)
        {
            CookieOptions option = new CookieOptions();
            option.HttpOnly = true;
            Response.Cookies.Append("username", username, option);

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("username");
            return Redirect("/");
        }

       
        [HttpGet]
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
        public async Task<JsonResult> SendMessageAsync(string message)
        {
            try
            {
                var topic = configuration["topic"];
                var producer = new ProducerWrapper(this.config, topic);
                await producer.writeMessage(message);
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
