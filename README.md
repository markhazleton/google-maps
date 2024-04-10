# Google Maps Web Services API Wrapper for .NET

## Overview

This project is a fork of the original Google Maps Web Services API wrapper for .NET, extended to include a minimal API using Fast Endpoints and a refactor of the `HttpClient` usage. By introducing a standalone project named `HttpClientUtility`, we've employed generics to decouple `HttpClient` management from the Google API mapping, enhancing modularity and maintainability.

## Changes in this Fork

- **Minimal API with Fast Endpoints**: Implemented a minimal API layer using Fast Endpoints to provide streamlined and efficient endpoint handling.
- **HttpClientUtility Refactor**: Introduced `HttpClientUtility`, a separate project to manage `HttpClient` instances using generics. This abstraction facilitates better resource management and simplifies interactions with the Google Maps API.

## Getting Started


## Features


## Contribution


## Acknowledgements

