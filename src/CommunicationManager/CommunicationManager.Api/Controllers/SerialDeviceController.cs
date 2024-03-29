﻿using CommunicationManager.Api.SerialComm.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerialDeviceController : ControllerBase
    {
        private readonly ILogger<SerialDeviceController> _logger;
        private readonly ISerialPortSender _connector;

        public SerialDeviceController(
            ILogger<SerialDeviceController> logger,
            ISerialPortSender connector)
        {
            _logger = logger;
            _connector = connector;
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
    }
}
