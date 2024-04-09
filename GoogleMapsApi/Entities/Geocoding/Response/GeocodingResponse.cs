using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Geocoding.Response
{
    /// <summary>
    /// Represents the response from the Geocoding API.
    /// </summary>
    [DataContract]
    public class GeocodingResponse : IResponseFor<GeocodingRequest>
    {
        /// <summary>
        /// Gets or sets the status of the geocoding request.
        /// </summary>
        [DataMember(Name = "status")]
        internal string StatusStr
        {
            get
            {
                return Status.ToString();
            }
            set
            {
                Status = (Status)Enum.Parse(typeof(Status), value);
            }
        }

        /// <summary>
        /// Gets or sets the status of the geocoding request.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the list of geocoding results.
        /// </summary>
        [DataMember(Name = "results")]
        public IEnumerable<Result> Results { get; set; }

        /// <summary>
        /// Returns a string representation of the GeocodingResponse object.
        /// </summary>
        /// <returns>A string representation of the GeocodingResponse object.</returns>
        public override string ToString()
        {
            return string.Format("GeocodingResponse - Status: {0}, Results count: {1}", Status, Results != null ? Results.Count() : 0);
        }
    }
}