using Application.DTO;
using Application.Interfaces;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers;

public class DeviceCreatedConsumer : IConsumer<DeviceCreatedMessage>
{
    private readonly IDeviceService _deviceService;

    public DeviceCreatedConsumer(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    public async Task Consume(ConsumeContext<DeviceCreatedMessage> context)
    {
        var message = context.Message;
        var deviceDTO = new DeviceDTO(message.Id, message.Description, message.Brand, message.Model, message.SerialNumber);

        await _deviceService.AddConsumedDeviceAsync(deviceDTO);
    }
}