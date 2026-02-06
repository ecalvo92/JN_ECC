using JN_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace JN_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpPost(Name = "RegistroUsuario")]
        public IActionResult RegistroUsuario(Usuario model)
        {
            /* 
               Reglas de negocio
               Cálculos
               Llamadas a la base de datos
            */

            return Ok();
        }
    }
}
