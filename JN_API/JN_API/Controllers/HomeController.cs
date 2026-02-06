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
        [HttpPost("RegistroUsuario")]
        public IActionResult RegistroUsuario(Usuario model)
        {
            using (var context = new SqlConnection("Server=localhost\\MSSQLSERVER01;Database=JN_DB;Integrated Security=True;TrustServerCertificate=True;"))
            {
                var parametros = new DynamicParameters();
                parametros.Add("@Identificacion", model.Identificacion);
                parametros.Add("@Nombre", model.Nombre);
                parametros.Add("@CorreoElectronico", model.CorreoElectronico);
                parametros.Add("@Contrasenna", model.Contrasenna);

                context.Execute("sp_RegistrarCuenta", parametros);
            }

            return Ok();
        }
    }
}
