using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly DeviceContext _context;
    private readonly IDeviceFactory _deviceFactory;
    public DeviceRepository(DeviceContext context, IDeviceFactory deviceFactory)
    {
        _context = context;
        _deviceFactory = deviceFactory;
    }

    public async Task<IDevice?> GetByIdAsync(Guid id)
    {
        var deviceDM = await _context.Set<DeviceDataModel>().FirstOrDefaultAsync(d => d.Id == id);
        if (deviceDM == null) return null;

        var device = _deviceFactory.CreateDevice(deviceDM);
        return device;
    }

    public async Task<IEnumerable<IDevice>> GetAllAsync()
    {
        var devicesDM = await _context.Set<DeviceDataModel>().ToListAsync();
        var devices = devicesDM.Select(_deviceFactory.CreateDevice);
        return devices;
    }

}