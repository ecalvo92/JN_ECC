using Microsoft.AspNetCore.Mvc;

namespace JN_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet(Name = "ConsultarDeudasCedula")]
        public IActionResult ConsultarDeudasCedula(string cedula)
        {
            return NotFound("3 deudas");
        }
    }
}
