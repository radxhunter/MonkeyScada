using CommunicationManager.Api.SerialPortConnector.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerialDeviceController : ControllerBase
    {
        private readonly ILogger<SerialDeviceController> _logger;
        private readonly ISerialPortConnectorService _connector;

        public SerialDeviceController(
            ILogger<SerialDeviceController> logger,
            ISerialPortConnectorService connector)
        {
            _logger = logger;
            _connector = connector;
        }

        // GET: api/<SerialDeviceController>
        [HttpGet]
        public IActionResult Get()
        {
            _connector.Read();
            return Ok();
        }

        // GET api/<SerialDeviceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public IActionResult Send([FromBody] string command, string roomNumber)
        {
            try
            {
                _connector.Send(command + roomNumber);
                return Ok("success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("failed");
            }
        }

        // PUT api/<SerialDeviceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SerialDeviceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
