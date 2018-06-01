using AspNetCoreSignalRDemo.Server.Hubs;
using AspNetCoreSignalRDemo.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSignalRDemo.Server.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly IHubContext<ChatHub> hubContext;

        public SampleDataController(IHubContext<ChatHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
        
        public async Task<ActionResult> Post(MessageModel model)
        {
            await this.hubContext.Clients.All.SendAsync("broadcastMessage", model.Name, model.Message);
            return Ok();
        }
    }
}
