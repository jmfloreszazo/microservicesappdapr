using System;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace AService.Controllers
{
    [ApiController]  
    public class OrderController : ControllerBase
    {
        private readonly DaprClient _dapr;
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, DaprClient dapr)
        {
            _logger = logger;
            _dapr = dapr;
        }

        [Topic("dapr-sb", "dapr")]
        [HttpPost]
        [Route("dapr")]
        public async Task<IActionResult> Process([FromBody] ObjectGroup og)
        {
            
            _logger.LogInformation($"Object Group with id {og.Id} processed");
            await PublishEvent(og.Id, ObjectEvent.EventType.Created);
            return Ok();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] ObjectGroup og, [FromServices] DaprClient daprClient)
        {
            _logger.LogInformation($"Object Group with id {og.Id} created");
            await _dapr.PublishEventAsync<ObjectGroup>("dapr-sb", "dapr", og);            
            return Ok();
        }

        private async Task<IActionResult> PublishEvent(Guid id, ObjectEvent.EventType type)
        {
            var ev = new ObjectEvent
            {
                Id = id,
                Name = "ObjectEvent",
                Type = type
            };
            await _dapr.PublishEventAsync<ObjectEvent>("dapr-eh", "CallBService", ev);
            return Ok();
        }     

    }
}
