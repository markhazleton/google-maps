using Newtonsoft.Json;
using System;

namespace HttpClientUtility;

/// <summary>
/// NewtonsoftJsonStringConverter is a class that implements the IStringConverter interface.
/// </summary>
public class NewtonsoftJsonStringConverter : HttpClientUtility.IStringConverter
{
    /// <summary>
    /// ConvertFromString is a method that converts a string to the specified type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public T ConvertFromString<T>(string value)
    {
        try
        {
            // Using Newtonsoft.Json to deserialize the string to the specified type T
            // check for null value and throw ArgumentNullException
            return JsonConvert.DeserializeObject<T>(value) ?? throw new ArgumentNullException(value);
        }
        catch (Newtonsoft.Json.JsonException ex)
        {
            // Catching and rethrowing exceptions specific to Newtonsoft.Json
            throw new InvalidOperationException("Conversion failed", ex);
        }
    }
    /// <summary>
    /// ConvertFromModel is a method that converts a model to a string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string ConvertFromModel<T>(T model)
    {
        try
        {
            // Using Newtonsoft.Json to serialize the model to a string
            return JsonConvert.SerializeObject(model);
        }
        catch (Newtonsoft.Json.JsonException ex)
        {
            // Catching and rethrowing exceptions specific to Newtonsoft.Json
            throw new InvalidOperationException("Conversion failed", ex);
        }
    }
}