using System.Net;

namespace GoogleMapsApi.HttpClientUtility;

public class HttpResponseContent<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
    public T Content { get; set; }

    // Constructor for a successful response
    public HttpResponseContent(T content, HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        IsSuccess = true;
        Content = content;
    }

    // Constructor for an unsuccessful response
    public HttpResponseContent(string errorMessage, HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }
}