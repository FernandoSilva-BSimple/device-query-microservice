using Domain.Interfaces;
using Domain.Visitors;

namespace Domain.Factory;

public interface IDeviceFactory
{
    IDevice CreateDevice(Guid id, string description, string brand, string model, string serialNumber);
    IDevice CreateDevice(IDeviceVisitor deviceVisitor);
}