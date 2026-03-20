using Dapper;
using JN_API.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace JN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ErrorController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("CapturarError")]
        public IActionResult CapturarError()
        {
            //Captura los detalles del error presentado
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var usuario = User.FindFirst("consecutivo")?.Value ?? "0";

            using var context = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DefaultConnection"));
            var parametros = new DynamicParameters();
            parametros.Add("@Error", exception?.Error.Message);
            parametros.Add("@Fecha", DateTime.Now);
            parametros.Add("@Origen", exception?.Path);
            parametros.Add("@Usuario", usuario);

            context.Execute("sp_RegistrarError", parametros);

            return StatusCode(500, "Ocurrió un error interno");
        }
    }
}
