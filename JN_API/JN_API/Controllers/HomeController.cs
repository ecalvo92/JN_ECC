using Dapper;
using JN_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace JN_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("RegistroUsuario")]
        public IActionResult RegistroUsuario(Usuario model)
        {
            using (var context = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DefaultConnection")))
            {
                var parametros = new DynamicParameters();
                parametros.Add("@Identificacion", model.Identificacion);
                parametros.Add("@Nombre", model.Nombre);
                parametros.Add("@CorreoElectronico", model.CorreoElectronico);
                parametros.Add("@Contrasenna", model.Contrasenna);

                var result = context.Execute("sp_RegistrarCuenta", parametros);

                if(result <= 0)
                    return BadRequest("Su información no se registró correctamente");

                return Ok("Su información se registró correctamente");                    
            }
        }
    }
}
