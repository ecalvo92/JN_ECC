using Dapper;
using JN_API.Models;
using JN_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JN_API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IPasswordHelper _password;
        public HomeController(IConfiguration config, IPasswordHelper password)
        {
            _config = config;
            _password = password;
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

            result.Token = GenerarToken(result.Consecutivo, result.ConsecutivoRol);
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
            parametrosActualizacion.Add("@Contrasenna", _password.Encrypt(nuevaContrasenna));
            var actualizacion = context.Execute("sp_ActualizarContrasenna", parametrosActualizacion);

            //Se notifica al usuario
            var contenido = ObtenerPlantillaCorreo(result.Nombre, nuevaContrasenna);
            _password.EnviarCorreo(result.CorreoElectronico, "Recuperación de Acceso", contenido);
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

        private string GenerarToken(int consecutivo, int consecutivoRol)
        {
            var key = Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key")!);

            var claims = new[]
            {
                new Claim("consecutivo", consecutivo.ToString()),
                new Claim("consecutivoRol", consecutivoRol.ToString()),
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            );

            var tokenDescriptor = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}
