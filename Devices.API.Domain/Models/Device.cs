using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Devices.API.Domain.Models.Enums;

namespace Devices.API.Domain.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
