using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;

namespace Application.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;

    public DeviceService(IDeviceRepository deviceRepository, IMapper mapper)
    {
        _deviceRepository = deviceRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<DeviceDTO>>> GetAllAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();

        var devicesDTO = devices.Select(_mapper.Map<DeviceDTO>);

        return Result<IEnumerable<DeviceDTO>>.Success(devicesDTO);
    }

    public async Task<Result<DeviceDTO>> GetByIdAsync(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);

        if (device is null)
            return Result<DeviceDTO>.Failure(Error.NotFound("Device not found."));

        var deviceDTO = _mapper.Map<DeviceDTO>(device);

        return Result<DeviceDTO>.Success(deviceDTO);
    }

    public async Task<Result<DeviceDTO>> AddConsumedDeviceAsync(DeviceDTO deviceDTO)
    {
        var device = _mapper.Map<IDevice>(deviceDTO);

        var deviceCreated = await _deviceRepository.AddAsync(device);

        var createdDeviceDTO = _mapper.Map<DeviceDTO>(deviceCreated);

        return Result<DeviceDTO>.Success(createdDeviceDTO);
    }
}
