using GeoService.API.CoordinatesSearchService;
using GeoService.API.Infrastructure.CoordinatesSearchService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddScoped<ICoordinatesSearchService, OsmCoordinatesSearchService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();