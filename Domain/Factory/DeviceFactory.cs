using Domain.Interfaces;
using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory;

public class DeviceFactory : IDeviceFactory
{
    public DeviceFactory() { }

    public IDevice CreateDevice(Guid id, string description, string brand, string model, string serialNumber)
    {
        return new Device(id, description, brand, model, serialNumber);
    }

    public IDevice CreateDevice(IDeviceVisitor deviceVisitor)
    {
        return new Device(deviceVisitor.Id, deviceVisitor.Description, deviceVisitor.Brand, deviceVisitor.Model, deviceVisitor.SerialNumber);
    }
}