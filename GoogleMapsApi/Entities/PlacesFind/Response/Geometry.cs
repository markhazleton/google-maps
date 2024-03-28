using GoogleMapsApi.Entities.Common;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesFind.Response
{
    /// <summary>
    /// Contains the location
    /// </summary>
    [DataContract]
    public class Geometry
    {
        [DataMember(Name = "location")]
        public Location Location { get; set; }
    }
}
