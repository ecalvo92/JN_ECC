using Dapper;
using JN_API.Models;
using JN_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace JN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IPasswordHelper _password;
        public SeguridadController(IConfiguration config, IPasswordHelper password)
        {
            _config = config;
            _password = password;
        }

        [HttpPut("CambiarAcceso")]
        public IActionResult CambiarAcceso(SeguridadRequest model)
        {
            using var context = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DefaultConnection"));
            var parametros = new DynamicParameters();
            parametros.Add("@Consecutivo", model.Consecutivo);
            parametros.Add("@Contrasenna", _password.Encrypt(model.NuevaContrasenna));

            var result = context.Execute("sp_ActualizarContrasenna", parametros);

            if (result <= 0)
                return BadRequest("Su información no se actualizó correctamente");

            return Ok("Su información se actualizó correctamente");
        }

    }
}
