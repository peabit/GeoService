using Serilog;
using GeoService.API.CoordinatesSearchService;
using GeoService.API.Infrastructure.CoordinatesSearchService;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Exceptions;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.WithExceptionDetails()
    .WriteTo.Console()
    .WriteTo.File(new CompactJsonFormatter(), "log.txt")
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllers();

    builder.Services.AddSwaggerGen();

    builder.Services.AddHttpClient();

    builder.Services.AddScoped<ICoordinatesSearchService, OsmCoordinatesSearchService>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

