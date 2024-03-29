using System.Net;

namespace HttpClientUtility;

/// <summary>
/// HttpResponseContent
/// </summary>
/// <typeparam name="T"></typeparam>
public class HttpResponseContent<T>
{
    /// <summary>
    /// StatusCode from request
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }
    /// <summary>
    /// Is request successfull or not
    /// </summary>
    public bool IsSuccess { get; set; }
    /// <summary>
    /// If not successfull, error message
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// Return content
    /// </summary>
    public T? Content { get; set; }

    /// <summary>
    /// Constructor for a successful response
    /// </summary>
    /// <param name="content"></param>
    /// <param name="statusCode"></param>
    public HttpResponseContent(T content, HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        IsSuccess = true;
        Content = content;
    }

    /// <summary>
    /// constructor for a failed response
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <param name="statusCode"></param>
    public HttpResponseContent(string errorMessage, HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }
}