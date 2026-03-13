using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpPost("CapturarError")]
        public IActionResult CapturarError()
        {
            //Captura los detalles del error presentado
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var fecha = DateTime.Now;
            var usuario = User.FindFirst("consecutivo")?.Value ?? "0";

            return StatusCode(500, "Ocurrió un error interno");
        }
    }
}
