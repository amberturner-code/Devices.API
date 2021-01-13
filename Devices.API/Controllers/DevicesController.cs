using Devices.API.Domain.Models;
using Devices.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Devices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;        

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }
                
        [HttpGet]        
        public async Task<IActionResult> GetDevicesAsync()
        {
            try
            {
                var devices = await _deviceService.ListAsync();
                return Ok(devices);
            }
            catch (Exception ex)
            {                
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult SetDeviceStatus([FromBody] Device device)
        {
            try
            {
                _deviceService.SetDeviceStatus(device);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }           
    }
}
