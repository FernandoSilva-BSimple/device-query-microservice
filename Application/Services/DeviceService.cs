/*using Application.Interfaces;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<Result<IEnumerable<IDevice>>> GetAllAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();
        return Result<IEnumerable<IDevice>>.Success(devices);
    }

    public async Task<Result<IDevice>> GetByIdAsync(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);

        if (device is null)
            return Result<IDevice>.Failure(Error.NotFound("Device not found."));

        return Result<IDevice>.Success(device);
    }
}
*/