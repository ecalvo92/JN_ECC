using JN_WEB.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JN_WEB.Controllers
{
    [SesionActiva]
    [PerfilAdmin]
    public class ServicioController : Controller
    {
        [HttpGet]
        public IActionResult CambiarTienda()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConsultarServicios()
        {
            return View();
        }

    }
}
