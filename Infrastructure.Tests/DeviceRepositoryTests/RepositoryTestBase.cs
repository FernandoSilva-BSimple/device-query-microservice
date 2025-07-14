using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.DeviceRepositoryTests;

public class RepositoryTestBase
{
    protected readonly DeviceContext context;

    protected RepositoryTestBase()
    {
        // Configure in-memory database
        var options = new DbContextOptionsBuilder<DeviceContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
            .Options;

        context = new DeviceContext(options);
    }
}
