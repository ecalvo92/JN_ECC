using JN_WEB.Filters;
using JN_WEB.Models;
using JN_WEB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace JN_WEB.Controllers
{
    [SesionActiva]
    [PerfilAdmin]
    public class ServicioController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;
        public ServicioController(IHttpClientFactory http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        #region CambiarTienda

        [HttpGet]
        public IActionResult CambiarTienda()
        {
            var token = HttpContext.Session.GetString("Token");

            using var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Servicio/ConsultarTienda";
            var result = client.GetAsync(url).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var objeto = result.Content.ReadFromJsonAsync<Tienda>().Result;
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
        public IActionResult CambiarTienda(Tienda model)
        {
            var token = HttpContext.Session.GetString("Token");

            using var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Servicio/CambiarTienda";
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

        [HttpGet]
        public IActionResult ConsultarServicios()
        {
            return View();
        }

    }
}
