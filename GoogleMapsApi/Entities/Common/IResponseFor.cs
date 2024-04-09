namespace GoogleMapsApi.Entities.Common;

/// <summary>
/// Generic interface for a response to a request.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResponseFor<T> where T : MapsBaseRequest { }