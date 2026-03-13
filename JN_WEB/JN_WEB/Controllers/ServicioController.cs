using JN_WEB.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JN_WEB.Controllers
{
    [SesionActiva]
    public class ServicioController : Controller
    {
        [HttpGet]
        public IActionResult Consulta()
        {
            return View();
        }
    }
}
