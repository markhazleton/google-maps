using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Geocoding.Response
{    /// <summary>
     /// The "status" field within the Geocoding response object contains the status of the request, and may contain debugging information to help you track down why Geocoding is not working. The "status" field may contain the following values:
     /// </summary>
    [DataContract]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {        /// <summary>
             /// Indicates that no errors occurred; the address was successfully parsed and at least one geocode was returned.
             /// </summary>
        [EnumMember]
        [JsonPropertyName("OK")]
        OK,

        /// <summary>
        /// Indicates that the geocode was successful but returned no results. This may occur if the geocode was passed a non-existent address or a latlng in a remote location.
        /// </summary>
        [EnumMember]
        [JsonPropertyName("ZERO_RESULTS")]
        ZERO_RESULTS,

        /// <summary>
        /// Indicates that you are over your quota.
        /// </summary>
		[EnumMember]
        [JsonPropertyName("OVER_QUERY_LIMIT")]
        OVER_QUERY_LIMIT,

        /// <summary>
        /// Indicates that your request was denied.
        /// </summary>
		[EnumMember]
        [JsonPropertyName("REQUEST_DENIED")]
        REQUEST_DENIED,

        /// <summary>
        /// Generally indicates that the query (address or latlng) is missing.
        /// </summary>
		[EnumMember]
        [JsonPropertyName("INVALID_REQUEST")]
        INVALID_REQUEST
    }
}
