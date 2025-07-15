using System.Threading.Tasks;
using Application.DTO;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.DeviceServiceTests;

public class GetAllAsyncTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfDevices_WhenDevicesExist()
    {
        // Arrange
        var mapperDouble = new Mock<IMapper>();
        var repoDouble = new Mock<IDeviceRepository>();

        var deviceMocks = new List<Mock<IDevice>>
    {
        new Mock<IDevice>(),
        new Mock<IDevice>(),
        new Mock<IDevice>()
    };

        var deviceList = deviceMocks.Select(dm =>
        {
            var id = Guid.NewGuid();
            dm.Setup(d => d.Id).Returns(id);
            dm.Setup(d => d.Description).Returns(It.IsAny<string>());
            dm.Setup(d => d.Brand).Returns(It.IsAny<string>());
            dm.Setup(d => d.Model).Returns(It.IsAny<string>());
            dm.Setup(d => d.SerialNumber).Returns(It.IsAny<string>());

            return dm.Object;
        }).ToList();

        var deviceDTOs = deviceList.Select(d =>
            new DeviceDTO(d.Id, d.Description, d.Brand, d.Model, d.SerialNumber)
        ).ToList();

        repoDouble.Setup(r => r.GetAllAsync()).ReturnsAsync(deviceList);

        mapperDouble.Setup(m => m.Map<DeviceDTO>(It.IsAny<IDevice>()))
                    .Returns<IDevice>(d => new DeviceDTO(d.Id, d.Description, d.Brand, d.Model, d.SerialNumber));

        var service = new DeviceService(repoDouble.Object, mapperDouble.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(3, result.Value.Count());

        repoDouble.Verify(r => r.GetAllAsync(), Times.Once);
        mapperDouble.Verify(m => m.Map<DeviceDTO>(It.IsAny<IDevice>()), Times.Exactly(3));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoDevicesExist()
    {
        // Arrange
        var mapperDouble = new Mock<IMapper>();
        var repoDouble = new Mock<IDeviceRepository>();

        var emptyDeviceList = new List<IDevice>();

        repoDouble.Setup(r => r.GetAllAsync()).ReturnsAsync(emptyDeviceList);

        var service = new DeviceService(repoDouble.Object, mapperDouble.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);

        repoDouble.Verify(r => r.GetAllAsync(), Times.Once);
        mapperDouble.Verify(m => m.Map<DeviceDTO>(It.IsAny<IDevice>()), Times.Never);
    }
}