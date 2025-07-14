using Domain.Factory;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.DeviceRepositoryTests;

public class DeviceRepositoryGetAllAsyncTests : RepositoryTestBase
{

    [Fact]
    public async Task WhenGettingAllDevices_ThenReturnsAll()
    {
        // Arrange
        var deviceDM1 = new DeviceDataModel
        {
            Id = Guid.NewGuid(),
            Description = "description1",
            Brand = "brand1",
            Model = "model1",
            SerialNumber = "serial1"
        };

        var deviceDM2 = new DeviceDataModel
        {
            Id = Guid.NewGuid(),
            Description = "description2",
            Brand = "brand2",
            Model = "model2",
            SerialNumber = "serial2"
        };

        context.Devices.AddRange(deviceDM1, deviceDM2);
        await context.SaveChangesAsync();

        var expected1 = new Mock<IDevice>().Object;
        var expected2 = new Mock<IDevice>().Object;

        var factoryMock = new Mock<IDeviceFactory>();
        factoryMock.Setup(f => f.CreateDevice(deviceDM1)).Returns(expected1);
        factoryMock.Setup(f => f.CreateDevice(deviceDM2)).Returns(expected2);

        var repository = new DeviceRepository(context, factoryMock.Object);

        // Act
        var result = (await repository.GetAllAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(expected1, result);
        Assert.Contains(expected2, result);
    }

    [Fact]
    public async Task WhenNoDevicesExist_ThenReturnsEmptyList()
    {
        // Arrange
        var factoryMock = new Mock<IDeviceFactory>();
        var repository = new DeviceRepository(context, factoryMock.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        factoryMock.Verify(f => f.CreateDevice(It.IsAny<DeviceDataModel>()), Times.Never);
    }
}
