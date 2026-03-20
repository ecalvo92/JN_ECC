using JN_WEB.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JN_WEB.Controllers
{
    [SesionActiva]
    public class ServicioController : Controller
    {
        [HttpGet]
        public IActionResult ConsultarServicios()
        {
            return View();
        }
    }
}
