# Project Configuration Contract

**Date**: 2026-02-15 | **Type**: Build Configuration

## Overview

This contract defines the required `.csproj` properties for all projects in the Google Maps API repository to achieve constitutional compliance.

---

## GoogleMapsApi.csproj Contract

**Type**: Class Library (NuGet Package)  
**Purpose**: Core Google Maps API wrapper library

### Required Properties

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- CONSTITUTION REQUIREMENT: .NET 10 LTS -->
    <TargetFrameworks>net10.0</TargetFrameworks>
    
    <!-- CONSTITUTION REQUIREMENT: Nullable reference types enabled -->
    <Nullable>enable</Nullable>
    
    <!-- CONSTITUTION REQUIREMENT: Explicit usings (already compliant) -->
    <ImplicitUsings>disable</ImplicitUsings>
    
    <!-- CONSTITUTION REQUIREMENT: XML documentation for public APIs -->
    <GenerateDocumentationFile>True</GenerateDocumentationFile>
    
    <!-- Package metadata (unchanged) -->
    <GeneratePackageOnBuild>True</GeneratePackageOnBuild>
    <Authors>Maxim Novak, Mark Hazleton</Authors>
    <Description>Google Maps Web Services API wrapper for .NET</Description>
    <!-- ... other package properties ... -->
  </PropertyGroup>
  
  <ItemGroup>
    <!-- Package versions must align with target framework -->
    <PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="10.0.*" />
    <PackageReference Include="Microsoft.Extensions.Diagnostics.HealthChecks" Version="10.0.*" />
    <PackageReference Include="WebSpark.HttpClientUtility" Version="1.0.10" />
  </ItemGroup>
</Project>
```

### Migration Impact

- `<TargetFrameworks>net9.0</TargetFrameworks>` → `<TargetFrameworks>net10.0</TargetFrameworks>`
- Add `<Nullable>enable</Nullable>` (new property)
- Update Microsoft.Extensions.* packages from 9.0.x to 10.0.x

---

## GoogleMapsApi.FE.csproj Contract

**Type**: ASP.NET Core Web Application  
**Purpose**: Sample FastEndpoints API demonstrating library usage

### Required Properties

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <!-- CONSTITUTION REQUIREMENT: .NET 10 LTS -->
    <TargetFramework>net10.0</TargetFramework>
    
    <!-- CONSTITUTION REQUIREMENT: Nullable reference types (already compliant) -->
    <Nullable>enable</Nullable>
    
    <!-- ImplicitUsings enabled for web projects (acceptable) -->
    <ImplicitUsings>enable</ImplicitUsings>
    
    <!-- CONSTITUTION REQUIREMENT: XML documentation -->
    <GenerateDocumentationFile>True</GenerateDocumentationFile>
    
    <!-- User Secrets for local development (already compliant) -->
    <UserSecretsId>10d9b312-219d-45ab-b486-f8a74e738cad</UserSecretsId>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="FastEndpoints" Version="6.1.0" />
    <PackageReference Include="FastEndpoints.Swagger" Version="6.1.0" />
  </ItemGroup>
  
  <ItemGroup>
    <ProjectReference Include="..\GoogleMapsApi\GoogleMapsApi.csproj" />
  </ItemGroup>
</Project>
```

### Migration Impact

- `<TargetFramework>net9.0</TargetFramework>` → `<TargetFramework>net10.0</TargetFramework>`
- Already compliant with nullable reference types (no change)

---

## GoogleMapsApi.Test.csproj Contract

**Type**: Test Project  
**Purpose**: Unit and integration tests for GoogleMapsApi library

### Required Properties

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- CONSTITUTION REQUIREMENT: .NET 10 LTS -->
    <TargetFrameworks>net10.0</TargetFrameworks>
    
    <!-- CONSTITUTION REQUIREMENT: Nullable reference types enabled -->
    <Nullable>enable</Nullable>
    
    <LangVersion>latest</LangVersion>
    <OutputType>Library</OutputType>
    
    <!-- User Secrets for integration tests (already compliant) -->
    <UserSecretsId>3f95bb96-2fb2-4433-9bbb-ce0f3dbd3cec</UserSecretsId>
  </PropertyGroup>
  
  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
    <PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="10.0.*" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.0" />
    
    <!-- CONSTITUTION REQUIREMENT: MSTest (not NUnit) -->
    <PackageReference Include="MSTest.TestFramework" Version="3.7.0" />
    <PackageReference Include="MSTest.TestAdapter" Version="3.7.0" />
    
    <!-- REMOVED: NUnit packages -->
    <!-- <PackageReference Include="NUnit" Version="4.3.2" /> -->
    <!-- <PackageReference Include="NUnit3TestAdapter" Version="5.0.0" /> -->
    
    <PackageReference Include="System.Text.Json" Version="10.0.*" />
  </ItemGroup>
  
  <ItemGroup>
    <ProjectReference Include="..\GoogleMapsApi\GoogleMapsApi.csproj" />
  </ItemGroup>
  
  <ItemGroup>
    <None Update="appsettings.json">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
  </ItemGroup>
</Project>
```

### Migration Impact

- `<TargetFrameworks>net9.0</TargetFrameworks>` → `<TargetFrameworks>net10.0</TargetFrameworks>`
- Add `<Nullable>enable</Nullable>` (new property)
- Remove NUnit packages, add MSTest packages
- Update Microsoft.Extensions.* and System.Text.Json to 10.0.x

---

## Validation

**Build Validation**:
```powershell
dotnet build GoogleMapsApi.sln --configuration Release
```

**Test Validation**:
```powershell
dotnet test GoogleMapsApi.Test/GoogleMapsApi.Test.csproj --configuration Release
```

**Package Validation**:
```powershell
dotnet pack GoogleMapsApi/GoogleMapsApi.csproj --configuration Release
```

All commands must complete successfully with exit code 0.

---

## Breaking Changes

**For Library Consumers**: None - .NET 10 is backward compatible with .NET 9 compiled assemblies

**For Contributors**: 
- Must install .NET 10 SDK (download from https://dotnet.microsoft.com/download/dotnet/10.0)
- Nullable warnings may appear in existing code (non-blocking initially)
- MSTest required for new tests

---

## Compliance Mapping

| Constitutional Principle | Contract Clause | Status |
|-------------------------|-----------------|--------|
| I. Technology Stack | `<TargetFramework(s)>net10.0</TargetFramework(s)>` | ✅ Enforced |
| I. Technology Stack (Nullable) | `<Nullable>enable</Nullable>` | ✅ Enforced |
| III. API Documentation | `<GenerateDocumentationFile>True</GenerateDocumentationFile>` | ✅ Enforced |
| VII. Code Formatting | Enforced via .editorconfig (separate contract) | ✅ Enforced |
| VIII. Testing Standards | MSTest packages only | ✅ Enforced |
