# Google Maps Web Services API Wrapper for .NET

## Overview

This project is a fork of the original Google Maps Web Services API wrapper for .NET (https://github.com/maximn/google-maps), extended to include a minimal API using Fast Endpoints and a refactor of the `HttpClient` usage. By introducing a standalone project named `HttpClientUtility`, we've employed generics to decouple `HttpClient` management from the Google API mapping, enhancing modularity and maintainability.

## Prerequisites

- .NET 10 SDK or later ([Download](https://dotnet.microsoft.com/download/dotnet/10.0))
- Visual Studio 2022 (17.10+) or JetBrains Rider 2024.1+
- Azure subscription (for deployment)

## Building

```powershell
dotnet build GoogleMapsApi.sln --configuration Release
```

## Testing

This project uses MSTest as the testing framework.

```powershell
dotnet test --configuration Release
```

**Note**: Integration tests run against the real Google API web servers and count towards your query limit. A working internet connection is required, and test run time may vary depending on network conditions and Google's server load.

## Changes in this Fork

### **Minimal API with Fast Endpoints**

Implemented a minimal API layer using Fast Endpoints to provide streamlined and efficient endpoint handling.

### **HttpClientUtility Refactor use of HttpClient**

Introduced `HttpClientUtility`, a separate project to manage `HttpClient` instances using generics. This abstraction facilitates better resource management and simplifies interactions with the Google Maps API.
The new HttpClientUtility introduces a structured way to handle HTTP requests and responses, encapsulating them in generic types for easier and more flexible management. It uses an HttpResponseContent<T> class to represent responses, with generics allowing for various content types. The utility includes methods for different HTTP verbs like GET, POST, PUT, and DELETE, simplifying client-server communication. Moreover, it employs an IHttpClientFactory for creating HttpClient instances, promoting efficient resource usage and lifecycle management. This design separates concerns between HTTP client management and Google API mapping, enhancing code maintainability.

**HttpResponseContent<T>**

HttpResponseContent<T>: A generic class that serves as a container for the response from an HTTP request. It includes information about the success or failure of the request, the status code, any error messages, and the response content itself.

- Purpose: Encapsulates the response from an HTTP request, including the status code, success status, content of the response, and any error messages.
- Usage: Instances of this class are created through the Success and Failure static methods, representing successful and failed HTTP responses, respectively.

**HttpClientService**

Acts as a wrapper around HttpClient, simplifying the process of sending HTTP requests and handling responses. It abstracts away some of the complexities associated with HttpClient, 
such as instance management, request configuration, and response parsing.

- Purpose: Provides a service for making HTTP requests using HttpClient, handling request execution, and processing responses.
- Usage: After instantiation with the necessary dependencies (IHttpClientFactory and IStringConverter), it offers methods for sending HTTP requests (GetAsync, PostAsync, PutAsync, DeleteAsync) and customizing the request timeout.
- Error Handling: Errors during request execution are captured and returned as part of the HttpResponseContent<T> failure instance, including HTTP request exceptions, timeout exceptions, and other unexpected exceptions.

**IStringConverter**

An interface expected to provide methods for converting between strings and objects. It's used for serializing request payloads and deserializing response content. The actual implementation must be provided by the user of the HttpClientService.

