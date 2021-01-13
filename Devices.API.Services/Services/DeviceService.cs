using Devices.API.Database.Repositories;
using Devices.API.Domain.Models;
using Devices.API.Logging;
using Devices.API.Messaging.Sender;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Devices.API.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceMessageSender _deviceMessageSender;
        private readonly ILoggerService _logger;

        public DeviceService(IDeviceRepository deviceRepository, IDeviceMessageSender deviceMessageSender, ILoggerService logger)
        {
            _logger = logger;
            _deviceRepository = deviceRepository;
            _deviceMessageSender = deviceMessageSender;
        }

        public async Task<List<Device>> ListAsync()
        {
            try
            {
                return await _deviceRepository.ListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving devices from repository. Message: {ex.Message}");
                return new List<Device>();
            }
        }

        public void SetDeviceStatus(Device device)
        {
            try
            {
                _deviceMessageSender.SendMessage(device);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending status message for device: {device.Id}. Message: {ex.Message}");
            }
        }

        public async Task UpdateDeviceStatus(Device device)
        {
            try
            {
                await _deviceRepository.AssignDeviceAsync(device);
                _logger.LogInfo($"Status updated for device {device.Id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating status for device in repository: {device.Id}. Message: {ex.Message}");
            }
        }
    }
}
