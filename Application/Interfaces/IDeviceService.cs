using Application.DTO;

namespace Application.Interfaces;

public interface IDeviceService
{
    Task<Result<DeviceDTO>> AddConsumedDeviceAsync(DeviceDTO deviceDTO);
}