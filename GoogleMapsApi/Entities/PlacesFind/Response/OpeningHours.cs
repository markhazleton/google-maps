using System;
using System.Linq;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesFind.Response
{
    [DataContract]
    public class OpeningHours
    {
        /// <summary>
        ///  is a boolean value indicating if the Place is open at the current time.
        /// </summary>
        [DataMember(Name = "open_now")]
        public bool OpenNow { get; set; }
    }
}
