# Data Model: Constitution Compliance Migration

**Date**: 2026-02-15 | **Plan**: [plan.md](plan.md)

## Overview

This migration is an infrastructure update with no changes to domain entities or data models. No new classes, properties, or relationships are introduced. This document captures the configuration model changes required for constitution compliance.

## Configuration Model Changes

### Project Configuration (.csproj files)

**Entity**: Project Configuration  
**Type**: XML metadata  
**Location**: `*.csproj` files

#### Properties Modified

| Property | Current Value | Target Value | Applies To |
|----------|--------------|--------------|------------|
| `<TargetFramework>` | `net9.0` | `net10.0` | All 3 projects |
| `<Nullable>` | Not set (disabled) | `enable` | GoogleMapsApi, GoogleMapsApi.Test |
| Package: Microsoft.Extensions.* | `9.0.x` | `10.0.x` | GoogleMapsApi |
| Package: MSTest.* | N/A (NUnit) | `3.7.0` | GoogleMapsApi.Test |
| Package: NUnit* | `4.3.2` / `5.0.0` | Remove | GoogleMapsApi.Test |

**Impact**: These changes affect build output but do not modify runtime data structures or domain models.

---

### CI/CD Workflow Configuration

**Entity**: GitHub Actions Workflow  
**Type**: YAML configuration  
**Location**: `.github/workflows/main_mapsintegration-fe.yml`

#### Properties Modified

| Property | Current Value | Target Value | Purpose |
|----------|--------------|--------------|---------|
| `dotnet-version` | `'8.x'` | `'10.x'` | SDK version for build agent |
| `actions/setup-dotnet` version | `@v1` | `@v4` | Update to latest action version |
| Publish framework target | `net8.0` | `net10.0` | Match project target framework |
| `actions/upload-artifact` | `@v3` | `@v4` | Update to latest action version |
| `actions/download-artifact` | `@v3` | `@v4` | Update to latest action version |

**Impact**: Ensures CI/CD pipeline builds and tests with correct .NET version.

---

### EditorConfig Configuration (New)

**Entity**: EditorConfig Rules  
**Type**: INI-style configuration  
**Location**: `.editorconfig` (repository root)

#### Structure

```
[*]                    # Global rules (charset, line endings, whitespace)
[*.cs]                 # C# specific rules (formatting, style, naming)
[*.{csproj,json}]      # Project/config file indentation
```

**Key Sections**:
1. **Global Settings**: Character encoding, newline style, trailing whitespace
2. **C# Code Style**: Brace placement, indentation, spacing, var usage
3. **Naming Conventions**: PascalCase for types, interface prefix, method naming
4. **Code Quality**: Unused parameter warnings

**Impact**: Non-functional - enforces consistent code formatting across contributors.

---

### Test Framework Migration Model

**Entity**: Test Attribute Metadata  
**Type**: Code annotations (C# attributes)  
**Location**: `GoogleMapsApi.Test/**/*.cs`

#### Attribute Mapping

| NUnit Attribute | MSTest Attribute | Semantic Difference |
|----------------|------------------|---------------------|
| `[Test]` | `[TestMethod]` | None - direct equivalent |
| `[TestFixture]` | `[TestClass]` | None - direct equivalent |
| `[SetUp]` | `[TestInitialize]` | None - runs before each test |
| `[TearDown]` | `[TestCleanup]` | None - runs after each test |
| `[OneTimeSetUp]` | `[ClassInitialize]` | **Method must be static** + accept `TestContext` |
| `[OneTimeTearDown]` | `[ClassCleanup]` | **Method must be static** |
| `[TestCase(...)]` | `[DataRow(...)]` + `[DataTestMethod]` | DataTestMethod replaces TestMethod |
| `[Ignore("...")]` | `[Ignore]` or `Assert.Inconclusive` | Similar behavior |

**Impact**: Code changes required to test files, but test logic and behavior remain identical.

---

## Domain Model (Unchanged)

The following existing domain models are **NOT** modified by this migration:

- **GoogleMapsApiOptions** (Configuration/GoogleMapsApiOptions.cs)
- **MapsEngine** entities (Engine/*.cs)
- **Direction, Geocoding, Places entities** (Entities/*/*.cs)
- **GoogleMapsApiException** (Engine/GoogleMapsApiException.cs)

**Rationale**: This is a pure infrastructure/framework migration. Domain logic, API contracts, and data structures remain stable to maintain backward compatibility for NuGet package consumers.

---

## Validation Model

**Entity**: Health Check Status  
**Type**: Runtime diagnostics  
**Location**: `GoogleMapsApiHealthCheck.cs`

**Validation Points**:
- Health check endpoint must return HTTP 200 after migration
- All integration tests must pass (behavior unchanged)
- NuGet package build must succeed
- Nullable reference type warnings are acceptable (non-blocking) but should be tracked

---

## State Transitions

```
┌─────────────────┐
│ Current State   │
│ .NET 9 + NUnit  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Phase 1:         │
│ Update configs   │  ← .editorconfig created
│ (.csproj files)  │  ← net10.0, nullable reference types enabled
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Phase 2:         │
│ Migrate tests    │  ← NUnit → MSTest
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Phase 3:         │
│ Update CI/CD     │  ← GitHub Actions → .NET 10
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Target State    │
│ .NET 10 + MSTest│  ✅ Constitution compliant
└─────────────────┘
```

---

## Summary

This migration involves **zero domain model changes**. All modifications are to:
1. Build configuration (project files)
2. Test metadata (attribute annotations)
3. Infrastructure configuration (CI/CD, .editorconfig)

No API contracts, entity schemas, or data structures are modified, ensuring seamless migration for package consumers.
