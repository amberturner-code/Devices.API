using Devices.API.Database.Contexts;
using Devices.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Devices.API.Database.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        protected readonly DeviceContext _context;

        public DeviceRepository(DeviceContext context) 
        {
            _context = context;
        }

        public async Task<List<Device>> ListAsync()
        {
            return await _context.Devices.ToListAsync();
        }

        public async Task AssignDeviceAsync(Device device)
        {            
            _context.Attach(device);
            _context.Entry(device).Property(p => p.Status).IsModified = true;
            await this.SaveChanges();
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
