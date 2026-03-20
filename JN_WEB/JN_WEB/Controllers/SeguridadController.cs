using JN_WEB.Filters;
using JN_WEB.Models;
using JN_WEB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JN_WEB.Controllers
{
    [SesionActiva]
    public class SeguridadController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;
        private readonly IPasswordHelper _password;
        private readonly IWebHostEnvironment _env;
        public SeguridadController(IHttpClientFactory http, IConfiguration config, IPasswordHelper password, IWebHostEnvironment env)
        {
            _http = http;
            _config = config;
            _password = password;
            _env = env;
        }

        #region CambiarAcceso

        [HttpGet]
        public IActionResult CambiarAcceso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CambiarAcceso(Seguridad model)
        {
            model.NuevaContrasenna = _password.Encrypt(model.NuevaContrasenna);
            model.ConfirmarContrasenna = _password.Encrypt(model.ConfirmarContrasenna);

            var token = HttpContext.Session.GetString("Token");

            using var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Seguridad/CambiarAcceso";
            var result = client.PutAsJsonAsync(url, model).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("CerrarSesion", "Home");
            }
            else if (result.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception();
            }

            ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;
            return View();
        }

        #endregion

        #region CambiarPerfil

        [HttpGet]
        public IActionResult CambiarPerfil()
        {
            var token = HttpContext.Session.GetString("Token");

            using var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Seguridad/ConsultarUsuario";
            var result = client.GetAsync(url).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var objeto = result.Content.ReadFromJsonAsync<Usuario>().Result;
                return View(objeto);
            }
            else if (result.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception();
            }

            ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;
            return View();
        }

        [HttpPost]
        public IActionResult CambiarPerfil(Usuario model, IFormFile? ImagenPerfil)
        {
            var token = HttpContext.Session.GetString("Token");
            var consecutivo = HttpContext.Session.GetInt32("Consecutivo");
            model.ImagenPerfil = string.Empty;

            if (ImagenPerfil != null && ImagenPerfil.Length > 0)
            {
                var carpeta = Path.Combine(_env.WebRootPath, "uploads");
                
                if(!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{consecutivo}.png";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using var stream = new FileStream(rutaCompleta, FileMode.Create);
                ImagenPerfil.CopyTo(stream);

                model.ImagenPerfil = $"/uploads/{nombreArchivo}";
            }

            using var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Seguridad/CambiarPerfil";
            var result = client.PutAsJsonAsync(url, model).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;
                return View(model);
            }
            else if (result.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception();
            }

            ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;
            return View();
        }

        #endregion

    }
}
