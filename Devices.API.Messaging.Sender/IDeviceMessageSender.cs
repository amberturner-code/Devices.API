using Devices.API.Domain.Models;

namespace Devices.API.Messaging.Sender
{
    public interface IDeviceMessageSender
    {       
        void SendMessage(Device device);
    }
}
