using Domain.Interfaces;

namespace Domain.IRepository;

public interface IDeviceRepository
{
    Task<IDevice?> GetByIdAsync(Guid id);
    Task<IEnumerable<IDevice>> GetAllAsync();
    Task<IDevice> AddAsync(IDevice device);

}