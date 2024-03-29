using System;

namespace GoogleMapsApi.HttpClientUtility;

public class SystemJsonStringConverter : IStringConverter
{
    public T ConvertFromString<T>(string value)
    {
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
        catch (System.Text.Json.JsonException ex)
        {
            throw new InvalidOperationException("Conversion failed", ex);
        }
    }
}
