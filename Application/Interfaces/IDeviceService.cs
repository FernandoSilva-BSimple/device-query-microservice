using Application.DTO;

namespace Application.Interfaces;

public interface IDeviceService
{
    Task<Result<DeviceDTO>> AddConsumedDeviceAsync(DeviceDTO deviceDTO);
    Task<Result<DeviceDTO>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<DeviceDTO>>> GetAllAsync();
}