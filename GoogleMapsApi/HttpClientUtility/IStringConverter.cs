using System;

namespace GoogleMapsApi.HttpClientUtility
{
    public interface IStringConverter
    {
        T ConvertFromString<T>(string value);
    }
}