using JN_WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace JN_WEB.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region Inicio de sesión

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

        #endregion

        #region Crear Cuenta

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario model)
        {
            return View();
        }

        #endregion

        #region Recuperar Acceso

        [HttpGet]
        public IActionResult RecuperarAcceso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarAcceso(Usuario model)
        {
            return View();
        }

        #endregion

    }
}
