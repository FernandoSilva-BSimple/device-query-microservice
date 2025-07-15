using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi;

namespace InterfaceAdapters.Controllers;

[Route("api/devices")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceDTO>> GetById(Guid id)
    {
        var device = await _deviceService.GetByIdAsync(id);

        return device.ToActionResult();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceDTO>>> GetAll()
    {
        var devices = await _deviceService.GetAllAsync();

        return devices.ToActionResult();
    }
}