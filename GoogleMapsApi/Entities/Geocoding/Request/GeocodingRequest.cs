﻿using GoogleMapsApi.Entities.Common;
using HttpClientUtility.Models;
using System;
using System.Linq;

namespace GoogleMapsApi.Entities.Geocoding.Request
{
    public class GeocodingRequest : SignableRequest
    {
        protected internal override string BaseUrl
        {
            get
            {
                return base.BaseUrl + "geocode/";
            }
        }

        /// <summary>
        /// address (required) — The address that you want to geocode.*
        /// </summary>
        public string Address { get; set; } //Required *or Location or PlaceId

        /// <summary>
        /// latlng (required) — The textual latitude/longitude value for which you wish to obtain the closest, human-readable address.*
        /// If you pass a latlng, the geocoder performs what is known as a reverse geocode. See Reverse Geocoding for more information.
        /// </summary>
        public Location Location { get; set; } //Required *or Address or PlaceId

        /// <summary>
        /// placeId (required) — The place ID of the place for which you wish to obtain the closest, human-readable address.*
        /// </summary>
        public string PlaceId { get; set; } //Required *or Address or Location

        /// <summary>
        /// bounds (optional) — The bounding box of the viewport within which to bias geocode results more prominently. (For more information see Viewport Biasing below.)
        /// The bounds and region parameters will only influence, not fully restrict, results from the geocoder.
        /// </summary>
        public Location[] Bounds { get; set; }

        /// <summary>
        /// region (optional) — The region code, specified as a ccTLD ("top-level domain") two-character value. (For more information see Region Biasing below.)
        /// The bounds and region parameters will only influence, not fully restrict, results from the geocoder.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// language (optional) — The language in which to return results. See the supported list of domain languages. Note that we often update supported languages so this list may not be exhaustive. If language is not supplied, the geocoder will attempt to use the native language of the domain from which the request is sent wherever possible.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// A component filter for which you wish to obtain a geocode. The components filter will also be accepted as an optional parameter if an address is provided.
        /// See more info: https://developers.google.com/maps/documentation/geocoding/intro#ComponentFiltering
        /// </summary>
        public GeocodingComponents Components { get; set; } = new GeocodingComponents();

        protected override QueryStringParametersList GetQueryStringParameters()
        {
            bool hasPlaceId = !string.IsNullOrWhiteSpace(PlaceId);
            bool hasLocation = Location is not null;
            bool hasAddress = !string.IsNullOrWhiteSpace(Address);

            if (!(hasPlaceId || hasLocation || hasAddress || Components.Exists))
                throw new ArgumentException("PlaceId, Location, Address or Components is required");

            var parameters = base.GetQueryStringParameters();

            if (hasPlaceId)
            {
                parameters.Add(_placeId, PlaceId);
            }
            else if (hasLocation)
            {
                parameters.Add(_latlng, Location.ToString());
            }
            else if (hasAddress)
            {
                parameters.Add(_address, Address);
            }

            if (Components.Exists && !hasPlaceId)
            {
                string components = Components.Build();
                parameters.Add(_components, components);
            }

            if (Bounds != null && Bounds.Any() && !hasPlaceId)
                parameters.Add(_bounds, string.Join("|", Bounds.AsEnumerable()));

            if (!string.IsNullOrWhiteSpace(Region) && !hasPlaceId)
                parameters.Add(_region, Region);

            if (!string.IsNullOrWhiteSpace(Language))
                parameters.Add(_language, Language);

            return parameters;
        }

        private const string _latlng = "latlng";
        private const string _address = "address";
        private const string _placeId = "place_id";
        private const string _bounds = "bounds";
        private const string _region = "region";
        private const string _language = "language";
        private const string _components = "components";
    }
}