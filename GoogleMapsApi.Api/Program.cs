using GoogleMapsApi;
using GoogleMapsApi.Api;
using HttpClientUtility;
using Serilog;
using Serilog.Extensions.Logging;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog Logging
ConfigureLogging(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Add Configuration with User Secrets
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

// Register HttpClientFactory
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IStringConverter, NewtonsoftJsonStringConverter>();
builder.Services.AddTransient<IHttpClientService, HttpClientService>();
builder.Services.AddSingleton<IMapsService, GoogleMapsServiceApi>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseMiddleware<LoggingMiddleware>();

app.MapGet("/geocoding/address", async (IMapsService googleMapsService, string address) =>
{
    return Results.Ok(await googleMapsService.GetGeoCodingFromAddress(address));
});

app.MapGet("/geocoding/latlong", async (IMapsService googleMapsService, double latitude, double longitude) =>
{
    return Results.Ok(await googleMapsService.GetGeocodingFromLatLong(latitude, longitude));
});

app.Run();

static void ConfigureLogging(WebApplicationBuilder builder)
{
    builder.Logging.ClearProviders();
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.File(@"C:\temp\googlemapsapi-log-.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
    builder.Logging.AddProvider(new SerilogLoggerProvider(Log.Logger));
    Log.Information("Serilog Logger setup complete");
}