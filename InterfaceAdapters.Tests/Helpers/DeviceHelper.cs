using Application.DTO;
using Faker;

namespace InterfaceAdapters.Tests.Helpers;

public static class DeviceHelper
{
    private static readonly Random _random = new();

    public static DeviceDTO GenerateRandomDeviceDto()
    {
        var description = $"{Company.CatchPhrase()}";
        var brand = Company.Name();
        var model = $"Model-{_random.Next(100, 999)}";
        var serialNumber = Guid.NewGuid().ToString();

        return new DeviceDTO
        {
            Id = Guid.NewGuid(),
            Description = description,
            Brand = brand,
            Model = model,
            SerialNumber = serialNumber
        };
    }
}
