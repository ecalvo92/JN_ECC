using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace JN_WEB.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult CapturarError()
        {
            //Captura los detalles del error presentado
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            return View("Error");
        }
    }
}
