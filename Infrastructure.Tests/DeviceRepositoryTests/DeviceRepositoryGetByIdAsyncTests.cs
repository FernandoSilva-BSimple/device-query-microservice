using Domain.Factory;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.DeviceRepositoryTests;

public class DeviceRepositoryGetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenSearchingById_ThenReturnsdevice()
    {
        // Arrange
        var id = Guid.NewGuid();
        var description = "description1";
        var brand = "brand1";
        var model = "model1";
        var serial = "serial1";

        var deviceDM = new DeviceDataModel
        {
            Id = id,
            Description = description,
            Brand = brand,
            Model = model,
            SerialNumber = serial
        };
        context.Devices.Add(deviceDM);
        await context.SaveChangesAsync();

        var deviceDouble = new Mock<IDevice>();
        deviceDouble.Setup(d => d.Id).Returns(id);
        deviceDouble.Setup(d => d.Description).Returns(description);
        deviceDouble.Setup(d => d.Brand).Returns(brand);
        deviceDouble.Setup(d => d.Model).Returns(model);
        deviceDouble.Setup(d => d.SerialNumber).Returns(serial);

        var factoryMock = new Mock<IDeviceFactory>();
        factoryMock.Setup(f => f.CreateDevice(It.Is<DeviceDataModel>(dm => dm.Id == id)))
                    .Returns(deviceDouble.Object);

        var repo = new DeviceRepository(context, factoryMock.Object);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
    }


    [Fact]
    public async Task WhenSearchingByNonExistentId_ThenReturnsNull()
    {
        // Arrange
        var deviceDM = new DeviceDataModel
        {
            Id = Guid.NewGuid(),
            Description = "description1",
            Brand = "brand1",
            Model = "model1",
            SerialNumber = "serialNumber1"
        };
        context.Devices.Add(deviceDM);
        await context.SaveChangesAsync();

        var factoryMock = new Mock<IDeviceFactory>();

        var repository = new DeviceRepository(context, factoryMock.Object);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }


}
