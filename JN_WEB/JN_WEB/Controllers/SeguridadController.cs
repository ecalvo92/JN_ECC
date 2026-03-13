using JN_WEB.Filters;
using JN_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace JN_WEB.Controllers
{
    [SesionActiva]
    public class SeguridadController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;
        public SeguridadController(IHttpClientFactory http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        [HttpGet]
        public IActionResult CambiarAcceso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CambiarAcceso(Seguridad model)
        {
            model.Consecutivo = HttpContext.Session.GetInt32("Consecutivo") ?? 0;
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
    }
}
