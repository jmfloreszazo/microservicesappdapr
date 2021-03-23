using System;
using System.Net.Http;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace BService.Controllers
{
    [ApiController]
    public class BController : ControllerBase
    {
        private readonly DaprClient _dapr;
        private readonly ILogger<BController> _logger;

        public BController(ILogger<BController> logger, DaprClient dapr)
        {
            _logger = logger;
            _dapr = dapr;
        }

        [Topic("dapr-eh", "CallBService")]
        [HttpPost]
        [Route("dapr")]
        public async Task<IActionResult> ProcessEvent([FromBody] ObjectEvent ev)
        {
            _logger.LogInformation($"Received new event");
            _logger.LogInformation("{0} {1} {2}", ev.Id, ev.Name, ev.Type);

            switch (ev.Type)
            {
                case ObjectEvent.EventType.Created:
                    if (await Get(ev.Id))
                    {
                        _logger.LogInformation($"Created the Object Group {ev.Id}");
                    }
                    else
                    {
                        _logger.LogInformation($"Object Group {ev.Id} not found in event Created");
                    }

                    break;
                case ObjectEvent.EventType.Updated:
                    if (await Get(ev.Id))
                    {
                        _logger.LogInformation($"Updated the Object Group {ev.Id}");
                    }
                    else
                    {
                        _logger.LogInformation($"Object Group {ev.Id} not found in event Updated");
                    }

                    break;
                case ObjectEvent.EventType.Deleted:
                    _logger.LogInformation($"Deleted the Object Group {ev.Id}");
                    break;
            }

            return Ok();
        }

        private async Task<bool> Get(Guid id)
        {
            try
            {
                await _dapr.InvokeMethodAsync<object, ObjectGroup>(HttpMethod.Get, "get", id.ToString(), null);
                return true;
            }
            catch (Exception)
            {
                //handle errors
            }

            return false;
        }
    }
}