using System;
using System.Linq;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesFind.Request
{
    public enum InputType
    {
        [EnumMember(Value = "textquery")]
        TextQuery,
        [EnumMember(Value = "phonenumber")]
        PhoneNumber
    }
}
