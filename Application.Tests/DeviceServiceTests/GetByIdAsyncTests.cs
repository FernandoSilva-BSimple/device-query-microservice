using Application.DTO;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.DeviceServiceTests;

public class GetByIdAsyncTests
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnDeviceDTO_WhenDeviceExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        var deviceMock = new Mock<IDevice>();
        deviceMock.SetupGet(d => d.Id).Returns(id);
        deviceMock.SetupGet(d => d.Description).Returns("Work laptop");
        deviceMock.SetupGet(d => d.Brand).Returns("Dell");
        deviceMock.SetupGet(d => d.Model).Returns("Latitude 14");
        deviceMock.SetupGet(d => d.SerialNumber).Returns("1234567890");

        var deviceDTO = new DeviceDTO(
            id,
            "Work laptop",
            "Dell",
            "Latitude 14",
            "1234567890"
        );

        var repoDouble = new Mock<IDeviceRepository>();
        repoDouble.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(deviceMock.Object);

        var mapperDouble = new Mock<IMapper>();
        mapperDouble.Setup(m => m.Map<DeviceDTO>(deviceMock.Object)).Returns(deviceDTO);

        var service = new DeviceService(repoDouble.Object, mapperDouble.Object);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(deviceDTO.Id, result.Value.Id);
        Assert.Equal(deviceDTO.Description, result.Value.Description);
        Assert.Equal(deviceDTO.Brand, result.Value.Brand);
        Assert.Equal(deviceDTO.Model, result.Value.Model);
        Assert.Equal(deviceDTO.SerialNumber, result.Value.SerialNumber);

        repoDouble.Verify(r => r.GetByIdAsync(id), Times.Once);
        mapperDouble.Verify(m => m.Map<DeviceDTO>(deviceMock.Object), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotFoundError_WhenDeviceDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repoDouble = new Mock<IDeviceRepository>();
        repoDouble.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((IDevice?)null);

        var mapperDouble = new Mock<IMapper>();

        var service = new DeviceService(repoDouble.Object, mapperDouble.Object);

        // Act
        var errorMessage = Error.NotFound("Device not found.").Message;
        var errorStatusCode = Error.NotFound("Device not found.").StatusCode;

        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.Equal(errorMessage, result.Error!.Message);
        Assert.Equal(errorStatusCode, result.Error.StatusCode);

        repoDouble.Verify(r => r.GetByIdAsync(id), Times.Once);
        mapperDouble.Verify(m => m.Map<DeviceDTO>(It.IsAny<IDevice>()), Times.Never);
    }


}