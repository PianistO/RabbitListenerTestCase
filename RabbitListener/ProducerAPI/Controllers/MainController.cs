using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitListener.Core.Commands;

namespace ProducerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public MainController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> PublishMessage(UrlCommand urlCommand)
        {
            await _publishEndpoint.Publish(urlCommand);

            return Ok();
        }
    }
}
