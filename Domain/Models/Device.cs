using Domain.Interfaces;

namespace Domain.Models;

public class Device : IDevice
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }

    public Device(Guid id, string description, string brand, string model, string serialNumber)
    {
        Id = id;
        Description = description;
        Brand = brand;
        Model = model;
        SerialNumber = serialNumber;
    }

    public Device() { }

}