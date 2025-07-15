using Application.DTO;
using Application.Services;
using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.DeviceServiceTests;

public class AddConsumedDeviceTestsAsync
{

    [Fact]
    public async Task AddConsumedDeviceAsync_ShouldCreateAndAddDevice_WhenDeviceDoesNotExist()
    {
        // Arrange
        var mapperDouble = new Mock<IMapper>();
        var deviceRepoDouble = new Mock<IDeviceRepository>();
        var deviceFactoryDouble = new Mock<IDeviceFactory>();

        var deviceId = Guid.NewGuid();
        var description = "Work laptop";
        var brand = "Dell";
        var model = "Latitude 14";
        var serialNumber = "1234567890";

        var deviceDTO = new DeviceDTO(deviceId, description, brand, model, serialNumber);

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(d => d.Id).Returns(deviceId);
        deviceMock.Setup(d => d.Description).Returns(description);
        deviceMock.Setup(d => d.Brand).Returns(brand);
        deviceMock.Setup(d => d.Model).Returns(model);
        deviceMock.Setup(d => d.SerialNumber).Returns(serialNumber);

        mapperDouble.Setup(m => m.Map<IDevice>(deviceDTO)).Returns(deviceMock.Object);

        var deviceCreatedMock = new Mock<IDevice>();
        deviceCreatedMock.Setup(d => d.Id).Returns(deviceId);
        deviceCreatedMock.Setup(d => d.Description).Returns(description);
        deviceCreatedMock.Setup(d => d.Brand).Returns(brand);
        deviceCreatedMock.Setup(d => d.Model).Returns(model);
        deviceCreatedMock.Setup(d => d.SerialNumber).Returns(serialNumber);

        deviceRepoDouble
            .Setup(dr => dr.AddAsync(deviceMock.Object))
            .ReturnsAsync(deviceCreatedMock.Object);

        var createdDeviceDTO = new DeviceDTO(deviceCreatedMock.Object.Id, deviceCreatedMock.Object.Description, deviceCreatedMock.Object.Brand, deviceCreatedMock.Object.Model, deviceCreatedMock.Object.SerialNumber);

        mapperDouble.Setup(m => m.Map<DeviceDTO>(deviceCreatedMock.Object)).Returns(createdDeviceDTO);

        var service = new DeviceService(
            deviceRepoDouble.Object,
            mapperDouble.Object
        );

        // Act
        var result = await service.AddConsumedDeviceAsync(deviceDTO);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(createdDeviceDTO.Id, result.Value.Id);
        Assert.Equal(createdDeviceDTO.Description, result.Value.Description);
        Assert.Equal(createdDeviceDTO.Brand, result.Value.Brand);
        Assert.Equal(createdDeviceDTO.Model, result.Value.Model);
        Assert.Equal(createdDeviceDTO.SerialNumber, result.Value.SerialNumber);

        deviceRepoDouble.Verify(dr => dr.AddAsync(deviceMock.Object), Times.Once);
        mapperDouble.Verify(m => m.Map<DeviceDTO>(deviceCreatedMock.Object), Times.Once);
        mapperDouble.Verify(m => m.Map<IDevice>(deviceDTO), Times.Once);

    }
}