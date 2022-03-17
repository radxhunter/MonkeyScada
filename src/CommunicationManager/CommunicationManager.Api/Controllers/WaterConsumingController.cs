using CommunicationManager.Api.Models;
using CommunicationManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommunicationManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterConsumingController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;

        public WaterConsumingController(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        // GET: api/<WaterConsumingController>
        [HttpGet]
        public IEnumerable<Measurement> Get()
        {
            return _measurementService.Measurements;
        }

        // POST api/<WaterConsumingController>
        [HttpPost]
        public ActionResult Post([FromBody] Measurement measurement)
        {
            _measurementService.AddMeasurement(measurement);
            return Ok(_measurementService.Measurements);
        }
    }
}
