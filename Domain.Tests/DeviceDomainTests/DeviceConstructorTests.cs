namespace Domain.Tests.DeviceDomainTests;

using Domain.Models;

public class DeviceConstructorTests
{

    [Fact]
    public void WhenCreatingDeviceWithId_ThenDeviceIsCreated()
    {
        //arrange
        Guid id = Guid.NewGuid(); ;
        string description = "Work laptop";
        string brand = "Dell";
        string model = "Latitude 14";
        string serialNumber = "1234567890";

        //act & assert
        new Device(id, description, brand, model, serialNumber);

    }


}