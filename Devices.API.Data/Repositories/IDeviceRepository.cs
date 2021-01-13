using Devices.API.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Devices.API.Database.Repositories
{
    public interface IDeviceRepository
    {
        Task<List<Device>> ListAsync();

        Task AssignDeviceAsync(Device device);
    }
}
