using Application.DTO;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.Extensions.DependencyInjection;

namespace InterfaceAdapters.Tests.Helpers;

public static class TestSeeder
{
    public static async Task SeedDeviceAsync(DeviceDTO device, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DeviceContext>();

        db.Devices.Add(new DeviceDataModel
        {
            Id = device.Id,
            Description = device.Description,
            Brand = device.Brand,
            Model = device.Model,
            SerialNumber = device.SerialNumber
        });

        await db.SaveChangesAsync();
    }
}
