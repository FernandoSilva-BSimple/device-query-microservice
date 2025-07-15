using Application.DTO;
using InterfaceAdapters.Tests.Helpers;

namespace InterfaceAdapters.Tests.ControllerTests;

public class DeviceControllerTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public DeviceControllerTests(IntegrationTestsWebApplicationFactory<Program> factory)
        : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetDevices_ReturnsSeededDevice()
    {
        // Arrange
        var device = DeviceHelper.GenerateRandomDeviceDto();
        await TestSeeder.SeedDeviceAsync(device, _factory.Services);

        // Act
        var result = await GetAndDeserializeAsync<List<DeviceDTO>>("api/devices");

        // Assert
        Assert.Contains(result, d => d.Id == device.Id);
    }

    [Fact]
    public async Task GetDeviceById_ReturnsCorrectDevice()
    {
        // Arrange
        var device = DeviceHelper.GenerateRandomDeviceDto();
        await TestSeeder.SeedDeviceAsync(device, _factory.Services);

        // Act
        var result = await GetAndDeserializeAsync<DeviceDTO>($"api/devices/{device.Id}");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(device.Id, result.Id);
        Assert.Equal(device.Description, result.Description);
        Assert.Equal(device.Brand, result.Brand);
        Assert.Equal(device.Model, result.Model);
        Assert.Equal(device.SerialNumber, result.SerialNumber);
    }

}
