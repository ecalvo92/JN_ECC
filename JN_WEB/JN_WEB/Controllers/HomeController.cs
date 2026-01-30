using JN_WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace JN_WEB.Controllers
{
    public class HomeController : Controller
    {
        //Pantalla principal de inicio
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //Pantalla de inicio de sesión
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario model)
        {
            return View();
        }

    }
}
