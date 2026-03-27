using Dapper;
using JN_API.Models;
using JN_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace JN_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ServicioController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("ConsultarTienda")]
        public IActionResult ConsultarTienda()
        {
            var consecutivo = User.FindFirst("consecutivo")?.Value;

            using var context = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DefaultConnection"));
            var parametros = new DynamicParameters();
            parametros.Add("@ConsecutivoUsuario", consecutivo);

            var result = context.QueryFirstOrDefault<TiendaResponse>("sp_ConsultarTienda", parametros);

            if (result == null)
                return NotFound("Su información no se validó correctamente");

            return Ok(result);
        }

        [HttpPut("CambiarTienda")]
        public IActionResult CambiarTienda(TiendaRequest model)
        {
            var consecutivo = User.FindFirst("consecutivo")?.Value;

            using var context = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DefaultConnection"));
            var parametros = new DynamicParameters();
            parametros.Add("@ConsecutivoUsuario", consecutivo);
            parametros.Add("@Nombre", model.Nombre);
            parametros.Add("@Contacto", model.Contacto);
            parametros.Add("@Descripcion", model.Descripcion);
            parametros.Add("@Ubicacion", model.Ubicacion);

            var result = context.Execute("sp_ActualizarTienda", parametros);

            if (result <= 0)
                return BadRequest("La información de la tienda no se actualizó correctamente");

            return Ok("La información de la tienda se actualizó correctamente");
        }        

    }
}
