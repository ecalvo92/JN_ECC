using JN_WEB.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JN_WEB.Controllers
{
    [SesionActiva]
    public class SeguridadController : Controller
    {
        [HttpGet]
        public IActionResult CambiarAcceso()
        {
            return View();
        }
    }
}
