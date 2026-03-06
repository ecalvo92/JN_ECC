using Dapper;
using JN_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Mail;

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
        public IActionResult RegistroUsuario(RegistrarUsuarioRequest model)
        {
            using var context = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DefaultConnection"));
            var parametros = new DynamicParameters();
            parametros.Add("@Identificacion", model.Identificacion);
            parametros.Add("@Nombre", model.Nombre);
            parametros.Add("@CorreoElectronico", model.CorreoElectronico);
            parametros.Add("@Contrasenna", model.Contrasenna);

            var result = context.Execute("sp_RegistrarCuenta", parametros);

            if (result <= 0)
                return BadRequest("Su información no se registró correctamente");

            return Ok("Su información se registró correctamente");
        }

        [HttpPost("IniciarSesion")]
        public IActionResult IniciarSesion(IniciarSesionRequest model)
        {
            using var context = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DefaultConnection"));
            var parametros = new DynamicParameters();
            parametros.Add("@CorreoElectronico", model.CorreoElectronico);
            parametros.Add("@Contrasenna", model.Contrasenna);

            var result = context.QueryFirstOrDefault<UsuarioResponse>("sp_IniciarSesion", parametros);

            if (result == null)
                return NotFound("Su información no se autenticó correctamente");

            return Ok(result);
        }

        [HttpPut("RecuperarAcceso")]
        public IActionResult RecuperarAcceso(RecuperarAccesoRequest model)
        {
            using var context = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DefaultConnection"));

            //Se valida el correo
            var parametros = new DynamicParameters();
            parametros.Add("@CorreoElectronico", model.CorreoElectronico);
            var result = context.QueryFirstOrDefault<UsuarioResponse>("sp_ValidarCorreo", parametros);

            if (result == null)
                return NotFound("Su información no se validó correctamente");

            //Se genera la nueva contraseña
            var nuevaContrasenna = GenerarContrasenna();

            //Se actualiza la contraseña
            var parametrosActualizacion = new DynamicParameters();
            parametrosActualizacion.Add("@Consecutivo", result.Consecutivo);
            parametrosActualizacion.Add("@Contrasenna", nuevaContrasenna);
            var actualizacion = context.Execute("sp_ActualizarContrasenna", parametrosActualizacion);

            //Se notifica al usuario
            var contenido = ObtenerPlantillaCorreo(result.Nombre, nuevaContrasenna);
            EnviarCorreo(result.CorreoElectronico, "Recuperación de Acceso", contenido);
            return Ok(result);
        }

        private static string GenerarContrasenna()
        {
            const string letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string([.. Enumerable.Range(0, 8).Select(_ => letras[Random.Shared.Next(letras.Length)])]);
        }

        private static string ObtenerPlantillaCorreo(string nombre, string contrasenna)
        {
            var ruta = Path.Combine(AppContext.BaseDirectory, "Templates", "RecuperarAcceso.html");
            var plantilla = System.IO.File.ReadAllText(ruta);
            return plantilla
                .Replace("{{Nombre}}", nombre)
                .Replace("{{Contrasenna}}", contrasenna);
        }

        private void EnviarCorreo(string destinatario, string asunto, string contenido)
        {
            var host = _config.GetValue<string>("Smtp:Host");
            var port = _config.GetValue<int>("Smtp:Port");
            var usuario = _config.GetValue<string>("Smtp:Usuario");
            var contrasenna = _config.GetValue<string>("Smtp:Contrasenna");

            using var smtp = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(usuario, contrasenna),
                EnableSsl = true
            };

            var mensaje = new MailMessage
            {
                From = new MailAddress(usuario!),
                Subject = asunto,
                Body = contenido,
                IsBodyHtml = true
            };

            mensaje.To.Add(destinatario);
            smtp.Send(mensaje);
        }
    }
}
