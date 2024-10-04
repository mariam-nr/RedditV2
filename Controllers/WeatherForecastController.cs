using Microsoft.AspNetCore.Mvc;

namespace Reddit.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
           throw new Exception("I am exception");
        }
    }
}
