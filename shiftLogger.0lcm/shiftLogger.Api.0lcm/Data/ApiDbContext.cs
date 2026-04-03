using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using shiftLoggerApi._0lcm.Models;

namespace shiftLoggerApi._0lcm.Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Shift> Shifts { get; set; }
}

public class ApiDbContextFactory : IDesignTimeDbContextFactory<ApiDbContext>
{
    public ApiDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>();
        optionsBuilder.UseSqlite(DbConfig.GetConnectionString());
        return new ApiDbContext(optionsBuilder.Options);
    }
}