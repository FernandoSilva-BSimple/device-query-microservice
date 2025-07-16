using Application.DTO;
using Application.Interfaces;
using Contracts.Messages;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
namespace InterfaceAdapters.Tests.ConsumerTests;

public class DeviceConsumerTests
{
    [Fact]
    public async Task Consume_ShouldCallDeviceServiceAddDeviceAsync()
    {
        //arrange
        var deviceDouble = new Mock<IDeviceService>();
        var deviceConsumer = new DeviceCreatedConsumer(deviceDouble.Object);

        var message = new DeviceCreatedMessage(Guid.NewGuid(), "Work laptop", "Dell", "Latitude 14", "1234567890", null);

        var context = Mock.Of<ConsumeContext<DeviceCreatedMessage>>(c => c.Message == message);

        var deviceDTO = new DeviceDTO(message.Id, message.Description, message.Brand, message.Model, message.SerialNumber);

        //act
        await deviceConsumer.Consume(context);

        //assert
        deviceDouble.Verify(ds => ds.AddConsumedDeviceAsync(It.Is<DeviceDTO>(d =>
            d.Id == message.Id &&
            d.Description == message.Description &&
            d.Brand == message.Brand &&
            d.Model == message.Model &&
            d.SerialNumber == message.SerialNumber
        )), Times.Once);
    }
}