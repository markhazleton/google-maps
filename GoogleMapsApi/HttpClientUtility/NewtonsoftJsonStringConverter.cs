using Newtonsoft.Json;
using System;

namespace GoogleMapsApi.HttpClientUtility;
public class NewtonsoftJsonStringConverter : IStringConverter
{
    public T ConvertFromString<T>(string value)
    {
        try
        {
            // Using Newtonsoft.Json to deserialize the string to the specified type T
            return JsonConvert.DeserializeObject<T>(value);
        }
        catch (Newtonsoft.Json.JsonException ex)
        {
            // Catching and rethrowing exceptions specific to Newtonsoft.Json
            throw new InvalidOperationException("Conversion failed", ex);
        }
    }
}