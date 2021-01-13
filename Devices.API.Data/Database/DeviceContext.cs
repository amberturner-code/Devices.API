using Devices.API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Devices.API.Database.Contexts
{
    public class DeviceContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }

        public DeviceContext(DbContextOptions<DeviceContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Device>().ToTable("Devices");
            builder.Entity<Device>().HasKey(p => p.Id);
            builder.Entity<Device>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Device>().Property(p => p.Name).HasMaxLength(50);
            builder.Entity<Device>().Property(p => p.Status);

            builder.Entity<Device>().HasData
            (
                new Device { Id = 100, Name = "Device 1", Status = "Unassigned" }, 
                new Device { Id = 101, Name = "Device 2", Status = "Unassigned" },
                new Device { Id = 102, Name = "Device 3", Status = "Unassigned" }
            );            
        }
    }
}
