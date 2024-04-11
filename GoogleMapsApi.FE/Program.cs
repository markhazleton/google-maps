using FastEndpointApi.endpoints.Geocode;
using FastEndpoints;
using FastEndpoints.Swagger;
using GoogleMapsApi.FE.endpoints.WichitaWisdom.Service;
using HttpClientUtility;
using HttpClientUtility.StringConverter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o =>
{
    o.ShortSchemaNames = true;
});

builder.Services.AddHttpClient();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<IStringConverter, NewtonsoftJsonStringConverter>();
builder.Services.AddTransient<IHttpClientService, HttpClientService>();
builder.Services.AddSingleton<IGeocodeService, GoogleMapsGeocodeService>();
builder.Services.AddSingleton<IWichitaWisdomService, WichitaWisdomService>();

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
