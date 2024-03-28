using GoogleMapsApi.Entities.Common;
using System.Collections.Generic;

namespace GoogleMapsApi.StaticMaps.Entities
{
    public class Path
    {
        public PathStyle Style { get; set; }

        public IList<ILocationString> Locations { get; set; }
    }
}