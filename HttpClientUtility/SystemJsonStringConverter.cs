using System;

namespace HttpClientUtility;
/// <summary>
/// String converter for System.Text.Json
/// </summary>
public class SystemJsonStringConverter : HttpClientUtility.IStringConverter
{
    /// <summary>
    /// Convert model to string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string ConvertFromModel<T>(T model)
    {
        try
        {
            return System.Text.Json.JsonSerializer.Serialize(model);
        }
        catch (System.Text.Json.JsonException ex)
        {
            throw new InvalidOperationException("Conversion failed", ex);
        }

    }

    /// <summary>
    /// convert string to model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T ConvertFromString<T>(string value)
    {
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(value) ?? throw new ArgumentNullException(value);
        }
        catch (System.Text.Json.JsonException ex)
        {
            throw new InvalidOperationException("Conversion failed", ex);
        }
    }
}
