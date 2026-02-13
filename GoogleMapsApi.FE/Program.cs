using FastEndpointApi.endpoints.Geocode;
using FastEndpoints;
using FastEndpoints.Swagger;
using WebSpark.HttpClientUtility.RequestResult;
using WebSpark.HttpClientUtility.StringConverter;

var builder = WebApplication.CreateBuilder(args);

// Configure FastEndpoints
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o =>
{
    o.ShortSchemaNames = true;
});

// Configure HttpClient with resilience patterns
builder.Services.AddHttpClient("GoogleMapsClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "GoogleMapsApi.NET/1.0");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
{
    // Enable compression for better performance
    AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
});

// Add health checks
builder.Services.AddHealthChecks();

// Configure WebSpark HttpClientUtility services
builder.Services.AddSingleton<IStringConverter, NewtonsoftJsonStringConverter>();
builder.Services.AddTransient<IHttpRequestResultService, HttpRequestResultService>();

// Configure application services
builder.Services.AddSingleton<IGeocodeService, GoogleMapsGeocodeService>();

// Add structured logging
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();
app.UseFastEndpoints(c =>
{
    c.Endpoints.ShortNames = true;
});
app.UseSwaggerGen();
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.Run();
