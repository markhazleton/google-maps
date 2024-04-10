# Google Maps Web Services API Wrapper for .NET

## Overview

This project is a fork of the original Google Maps Web Services API wrapper for .NET, extended to include a minimal API using Fast Endpoints and a refactor of the `HttpClient` usage. By introducing a standalone project named `HttpClientUtility`, we've employed generics to decouple `HttpClient` management from the Google API mapping, enhancing modularity and maintainability.

## Changes in this Fork

### **Minimal API with Fast Endpoints**

Implemented a minimal API layer using Fast Endpoints to provide streamlined and efficient endpoint handling.


### **HttpClientUtility Refactor**

Introduced `HttpClientUtility`, a separate project to manage `HttpClient` instances using generics. This abstraction facilitates better resource management and simplifies interactions with the Google Maps API.
The new HttpClientUtility introduces a structured way to handle HTTP requests and responses, encapsulating them in generic types for easier and more flexible management. It uses an HttpResponseContent<T> class to represent responses, with generics allowing for various content types. The utility includes methods for different HTTP verbs like GET, POST, PUT, and DELETE, simplifying client-server communication. Moreover, it employs an IHttpClientFactory for creating HttpClient instances, promoting efficient resource usage and lifecycle management. This design separates concerns between HTTP client management and Google API mapping, enhancing code maintainability.

## Getting Started


## Features


## Contribution


## Acknowledgements

