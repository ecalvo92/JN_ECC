using JN_WEB.Filters;
using JN_WEB.Models;
using JN_WEB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JN_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;
        private readonly IPasswordHelper _password;
        public HomeController(IHttpClientFactory http, IConfiguration config, IPasswordHelper password)
        {
            _http = http;
            _config = config;
            _password = password;
        }

        [HttpGet]
        public IActionResult Index()
        {
            using var client = _http.CreateClient();
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Home/ConsultarTiendas";
            var result = client.GetAsync(url).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var lista = result.Content.ReadFromJsonAsync<List<Tienda>>().Result;
                return View(lista);
            }
            else if (result.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception();
            }

            return View(new List<Tienda>());
        }

        #region Inicio de sesión

        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario model)
        {
            model.Contrasenna = _password.Encrypt(model.Contrasenna);

            using var client = _http.CreateClient();
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Home/IniciarSesion";
            var result = client.PostAsJsonAsync(url, model).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var objeto = result.Content.ReadFromJsonAsync<Usuario>().Result;
                HttpContext.Session.SetString("NombreUsuario", objeto!.Nombre);
                HttpContext.Session.SetInt32("Consecutivo", objeto!.Consecutivo);
                HttpContext.Session.SetString("Token", objeto!.Token);
                HttpContext.Session.SetString("ImagenPerfil", objeto!.ImagenPerfil);
                HttpContext.Session.SetInt32("ConsecutivoRol", objeto!.ConsecutivoRol);
                HttpContext.Session.SetString("NombreRol", objeto!.NombreRol);

                return RedirectToAction("Index", "Home");
            }
            else if (result.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception();
            }

            ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;
            return View();
        }

        #endregion

        #region Crear Cuenta

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario model)
        {
            model.Contrasenna = _password.Encrypt(model.Contrasenna);

            using var client = _http.CreateClient();
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Home/RegistroUsuario";
            var result = client.PostAsJsonAsync(url, model).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (result.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception();
            }

            ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;
            return View();
        }

        #endregion

        #region Recuperar Acceso

        [HttpGet]
        public IActionResult RecuperarAcceso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarAcceso(Usuario model)
        {
            using var client = _http.CreateClient();
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Home/RecuperarAcceso";
            var result = client.PutAsJsonAsync(url, model).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (result.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception();
            }

            ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;
            return View();
        }

        #endregion

        #region Cerrar Sesión

        [SesionActiva]
        [HttpGet]
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        #endregion

    }
}
