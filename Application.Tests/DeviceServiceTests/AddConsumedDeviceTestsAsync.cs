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

        var deviceId = Guid.NewGuid();
        var description = "Work laptop";
        var brand = "Dell";
        var model = "Latitude 14";
        var serialNumber = "1234567890";

        var deviceDTO = new DeviceDTO(deviceId, description, brand, model, serialNumber);
        var mappedDeviceMock = new Mock<Device>(deviceId, description, brand, model, serialNumber);

        mapperDouble.Setup(m => m.Map<Device>(deviceDTO)).Returns(mappedDeviceMock.Object);

        var createdDeviceMock = new Mock<Device>(deviceId, description, brand, model, serialNumber);
        deviceRepoDouble.Setup(dr => dr.AddAsync(mappedDeviceMock.Object)).ReturnsAsync(createdDeviceMock.Object);

        var expectedDTO = new DeviceDTO(createdDeviceMock.Object.Id, createdDeviceMock.Object.Description, createdDeviceMock.Object.Brand, createdDeviceMock.Object.Model, createdDeviceMock.Object.SerialNumber);
        mapperDouble.Setup(m => m.Map<DeviceDTO>(createdDeviceMock.Object)).Returns(expectedDTO);

        var service = new DeviceService(deviceRepoDouble.Object, mapperDouble.Object);

        // Act
        var result = await service.AddConsumedDeviceAsync(deviceDTO);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedDTO.Id, result.Value.Id);
        Assert.Equal(expectedDTO.Description, result.Value.Description);
        Assert.Equal(expectedDTO.Brand, result.Value.Brand);
        Assert.Equal(expectedDTO.Model, result.Value.Model);
        Assert.Equal(expectedDTO.SerialNumber, result.Value.SerialNumber);

        mapperDouble.Verify(m => m.Map<Device>(deviceDTO), Times.Once);
        mapperDouble.Verify(m => m.Map<DeviceDTO>(createdDeviceMock.Object), Times.Once);
        deviceRepoDouble.Verify(dr => dr.AddAsync(mappedDeviceMock.Object), Times.Once);
    }
}