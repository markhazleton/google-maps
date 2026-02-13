using System;
using WebSpark.HttpClientUtility;

namespace GoogleMapsApi.Entities.Common;

/// <summary>
/// Base class for Google Maps API requests.
/// </summary>
public abstract class MapsBaseRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MapsBaseRequest"/> class.
    /// </summary>
    public MapsBaseRequest()
    {
        this.isSsl = true;
        ApiKey = null;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the request comes from a device with a location sensor.
    /// </summary>
    /// <remarks>
    /// The Google Maps API previously required that you include the sensor parameter to indicate whether your application used a sensor to determine the user's location.
    /// This parameter is no longer required.
    /// See the geocoding API reference at https://developers.google.com/maps/documentation/geocoding/.
    /// </remarks>
    [Obsolete]
    public bool Sensor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use the HTTPS protocol.
    /// </summary>
    /// <value>
    ///   <c>true</c> to use HTTPS; otherwise, <c>false</c> to use HTTP.
    /// </value>
    public virtual bool IsSSL
    {
        get { return isSsl; }
        set { isSsl = value; }
    }
    private bool isSsl;

    /// <summary>
    /// Gets the base URL for the Google Maps API.
    /// </summary>
    protected internal virtual string BaseUrl
    {
        get
        {
            return "maps.googleapis.com/maps/api/";
        }
    }

    /// <summary>
    /// Gets or sets the API key for the application.
    /// </summary>
    /// <value>
    /// The API key.
    /// </value>
    public string ApiKey { get; set; }

    /// <summary>
    /// Gets the query string parameters for the request.
    /// </summary>
    /// <returns>A <see cref="QueryStringParametersList"/> containing the query string parameters.</returns>
    protected virtual QueryStringParametersList GetQueryStringParameters()
    {
        QueryStringParametersList parametersList = new();

        if (!string.IsNullOrWhiteSpace(ApiKey))
        {
            if (!this.IsSSL)
            {
                throw new ArgumentException("When using an ApiKey MUST send the request over SSL [IsSSL = true]");
            }
            parametersList.Add("key", ApiKey);
        }

        return parametersList;
    }

    /// <summary>
    /// Gets the URI for the request.
    /// </summary>
    /// <returns>The URI for the request.</returns>
    public virtual Uri GetUri()
    {
        string scheme = IsSSL ? "https://" : "http://";

        var queryString = GetQueryStringParameters().GetQueryStringPostfix();
        return new Uri(scheme + BaseUrl + "json?" + queryString);
    }
}
