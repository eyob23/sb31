using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sendMessage.ServiceBus;

namespace sb31.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private IServiceBusSendMessage _serviceBusSendMessage;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IServiceBusSendMessage serviceBusSendMessage)
        {
            _logger = logger;
            _serviceBusSendMessage = serviceBusSendMessage;
        }


        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();

            _logger.LogInformation("started" + DateTime.Now);
            //do something 
            var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            // Send this to the bus for the other services
            await _serviceBusSendMessage.SendMessage(new MessageDto
            {
                Id = Guid.NewGuid(),
                FirstName = Summaries[rng.Next(Summaries.Length)],
                LastName = Summaries[rng.Next(Summaries.Length)],
                WeatherForecast = forecast

            });
            _logger.LogInformation("finished" + DateTime.Now);
            return forecast;

        }
    }
}
