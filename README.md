# Rocket.Libraries.HttpRequestDecompression
A library that decompresses  HTTP requests, supports gzip and deflate

![Tests](https://github.com/rocket-libs/Rocket.Libraries.HttpRequestDecompression/workflows/NuGet%20Generation/badge.svg) ![Building](https://github.com/rocket-libs/Rocket.Libraries.HttpRequestDecompression/workflows/NuGet%20Generation/badge.svg) ![Publishing To Nuget](https://github.com/rocket-libs/Rocket.Libraries.HttpRequestDecompression/workflows/NuGet%20Generation/badge.svg) [![GitHub release](https://img.shields.io/github/release/Naereen/StrapDown.js.svg)](https://github.com/rocket-libs/Rocket.Libraries.HttpRequestDecompression/releases/) [![GitHub issues](https://img.shields.io/github/issues/Naereen/StrapDown.js.svg)](https://github.com/rocket-libs/Rocket.Libraries.HttpRequestDecompression/issues/)

## Getting started.
### 1. Installation
Grab the nuget package from https://www.nuget.org/packages/Rocket.Libraries.HttpRequestDecompression

### 2. ASP.Net Core Web App

1. In the **ConfigureServices** method of your **Startup.cs** file, register decompression services for dependancy injection.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddHttpRequestDecompressionSupport();
    // Add you other services
}
```

2. Then in the **Configure** method of your **Startup.cs** file, add the decompression middleware.
```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Be sure to add the middleware before any other middleware that may require an uncompressed request.
    // Infact it is probably best to add the decompression middleware before all others.
    app.UseHttpRequestDecompression();
}
```

3. Then in the **Configure** method of your **Startup.cs** file, add the decompression middleware.
Your compressed requests from the client must include the header **Content-Encoding**, and based on the compression method you're using the should be either ***gzip*** or ***deflate***.

## Important
### Scope of Operation
This middleware shall only decompress requests with **Content-Encoding** ***gzip*** or ***deflate***.
- If you don't include the **Content-Encoding** header, then the request shall be assumed to be uncompressed and the middleware shall pass it along the pipe line.
- If **Content-Encoding** header is anything else besides ***gzip*** or ***deflate***, an exception is thrown.

### Tests
- Included are the tests for the library, the codebase is fairly small and coverage is currently at ~ 92%.

