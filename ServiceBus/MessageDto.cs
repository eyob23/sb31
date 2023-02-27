using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sb31;

namespace sendMessage.ServiceBus
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public WeatherForecast[] WeatherForecast { get; set; }
    }
}