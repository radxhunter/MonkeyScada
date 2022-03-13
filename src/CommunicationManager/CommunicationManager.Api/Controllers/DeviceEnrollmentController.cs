using CommunicationManager.Api.Models;
using CommunicationManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommunicationManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceEnrollmentController : ControllerBase
    {
        private readonly IDeviceEnrollmentService _deviceEnrollmentService;

        public DeviceEnrollmentController(IDeviceEnrollmentService deviceEnrollmentService)
        {
            _deviceEnrollmentService = deviceEnrollmentService;
        }

        // GET: api/<WaterConsumingController>
        [HttpGet]
        public IEnumerable<WaterConsumer> Get()
        {
            return _deviceEnrollmentService.WaterConsumers;
        }

        // POST api/<WaterConsumingController>
        [HttpPost]
        public ActionResult Post([FromBody] WaterConsumer waterConsumer)
        {
            _deviceEnrollmentService.AddMeasurement(waterConsumer);
            return Ok(_deviceEnrollmentService.WaterConsumers);
        }
    }
}
