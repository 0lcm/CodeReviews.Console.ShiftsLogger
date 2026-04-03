using Microsoft.EntityFrameworkCore;
using shiftLoggerApi._0lcm.Data;
using shiftLoggerApi._0lcm.ServiceContracts;
using shiftLoggerApi._0lcm.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite(DbConfig.GetConnectionString()));
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IShiftService, ShiftService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

    var dbDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "shiftLogger.0lcm");
    Directory.CreateDirectory(dbDirectory);

    await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler(error => error.Run(async context =>
{
    context.Response.StatusCode = 500;
    await context.Response.WriteAsJsonAsync("An unexpected exception occurred during runtime.");
}));

app.Run();