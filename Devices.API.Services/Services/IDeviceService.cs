using Devices.API.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Devices.API.Services
{
    public interface IDeviceService
    {
        Task<List<Device>> ListAsync();
        void SetDeviceStatus(Device device);
        Task UpdateDeviceStatus(Device device);
    }
}
