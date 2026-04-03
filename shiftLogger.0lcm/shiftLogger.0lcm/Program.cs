using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using shiftLogger._0lcm.Configuration;
using shiftLogger._0lcm.ServiceContracts;
using shiftLogger._0lcm.Services;
using shiftLogger._0lcm.UserInterface;

var builder = Host.CreateApplicationBuilder(args);

var config = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Configuration"))
    .AddJsonFile("appSettings.json", false, true)
    .Build();
builder.Services.AddSingleton<IConfiguration>(config);

builder.Services.AddTransient<DateFormatSettings>();
builder.Services.AddTransient<IValidationService, ValidationService>();

builder.Services.AddTransient<IEmployeeApiService, EmployeeApiService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<EmployeeUi>();

builder.Services.AddTransient<IShiftApiService, ShiftApiService>();
builder.Services.AddTransient<IShiftService, ShiftService>();
builder.Services.AddSingleton<ShiftUi>();

builder.Services.AddSingleton<ConsoleUi>();

builder.Services.AddHttpClient(ApiSettings.BaseUrl,
    client => client.BaseAddress = new Uri(ApiSettings.BaseUrl));
builder.Services.AddHostedService<Worker>();

builder.Logging.SetMinimumLevel(LogLevel.Warning);

var app = builder.Build();

await app.RunAsync();

internal class Worker : BackgroundService
{
    private readonly ConsoleUi _console;

    public Worker(ConsoleUi console)
    {
        _console = console;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _console.MainMenu();
    }
}