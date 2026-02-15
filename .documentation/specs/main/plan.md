# Implementation Plan: Constitution Compliance - Critical Site Audit Issues

**Branch**: `main` | **Date**: 2026-02-15 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `.documentation/specs/main/spec.md`

**Note**: This plan addresses five critical constitution violations identified in the site audit.

## Summary

Migrate the GoogleMapsApi repository to constitutional compliance by upgrading all projects from .NET 9 to .NET 10 LTS, migrating tests from NUnit to MSTest, enabling nullable reference types, creating a repository-wide .editorconfig, and updating CI/CD pipelines to use .NET 10. This resolves five critical violations of MANDATORY constitutional principles and unblocks future feature development.

## Technical Context

**Language/Version**: C# / .NET 9.0 (MUST migrate to .NET 10 LTS per constitution)  
**Primary Dependencies**: Microsoft.Extensions.*, WebSpark.HttpClientUtility, FastEndpoints  
**Storage**: N/A (API wrapper library, no direct persistence)  
**Testing**: NUnit (MUST migrate to MSTest per constitution)  
**Target Platform**: Cross-platform (.NET library, Azure App Service for sample app)  
**Project Type**: Multi-project solution (library + web API + test)  
**Performance Goals**: HTTP client wrapper - minimize overhead on Google Maps API calls  
**Constraints**: Must maintain backward compatibility for NuGet package consumers  
**Scale/Scope**: 3 projects, ~150 source files, public NuGet package with existing consumers

**Current State Analysis**:
- GoogleMapsApi.csproj: net9.0, nullable reference types not enabled, XML docs enabled, ImplicitUsings disabled ✅
- GoogleMapsApi.FE.csproj: net9.0, nullable reference types enabled ✅, ImplicitUsings enabled
- GoogleMapsApi.Test.csproj: net9.0, nullable reference types not enabled, using NUnit 4.3.2
- CI/CD: GitHub Actions using .NET 8.x, publishing to net8.0
- No .editorconfig file at repository root

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

**Reference**: See `.documentation/memory/constitution.md` for full principles

### MUST Principles Verification

- [ ] **Technology Stack**: ❌ Projects using .NET 9 instead of .NET 10 LTS (I-TECH-1)
- [ ] **Technology Stack - Nullable Reference Types**: ❌ GoogleMapsApi.csproj and GoogleMapsApi.Test.csproj missing `<Nullable>enable</Nullable>` (I-TECH-2)
- [x] **Async Programming**: ✅ All I/O operations use async/await patterns
- [x] **API Documentation**: ✅ XML documentation enabled for GoogleMapsApi.csproj
- [x] **Structured Logging**: ✅ ILogger used throughout (GoogleMapsApiLogger.cs)
- [x] **Dependency Injection**: ✅ Constructor injection pattern used (ServiceCollectionExtensions.cs)
- [x] **Security**: ✅ User Secrets configured (GoogleMapsApi.Test and GoogleMapsApi.FE)
- [ ] **Code Formatting**: ❌ No .editorconfig file at repository root (VII-FORMAT-1)
- [ ] **Testing Standards**: ❌ Using NUnit instead of MSTest (VIII-TEST-1)
- [ ] **Azure Platform**: ❌ CI/CD using .NET 8 instead of .NET 10 (I-TECH-3)

### SHOULD Principles Verification

- [x] **Error Handling**: ✅ Custom exception class exists (GoogleMapsApiException.cs)
- [x] **HTTP Clients**: ✅ IHttpClientFactory pattern used via WebSpark.HttpClientUtility
- [x] **File Size**: ✅ Source files are reasonably sized (under 300 lines)

**Violations Requiring Justification**:

This plan explicitly addresses the 5 critical MUST principle violations:

1. **I-TECH-1** (.NET version): Justified as technical debt to be remediated by this plan
2. **I-TECH-2** (nullable reference types): Justified as technical debt to be remediated by this plan  
3. **I-TECH-3** (CI/CD version): Justified as technical debt to be remediated by this plan
4. **VII-FORMAT-1** (.editorconfig): Justified as missing infrastructure to be added by this plan
5. **VIII-TEST-1** (MSTest): Justified as technical debt to be remediated by this plan

**GATE STATUS**: ⚠️ BLOCKED - Critical violations present, but this plan exists specifically to remediate them. Proceeding with remediation workflow.

---

### Post-Phase 1 Re-evaluation

After completing research and design phases, remediation strategy is fully defined:

**Remediation Coverage**:
- ✅ **I-TECH-1** (.NET 10 migration): Complete migration path documented in [research.md](research.md) and [quickstart.md](quickstart.md)
- ✅ **I-TECH-2** (Nullable reference types): Phased enablement strategy defined, warnings accepted initially
- ✅ **I-TECH-3** (CI/CD .NET 10): Step-by-step workflow update documented in [contracts/cicd-workflow.md](contracts/cicd-workflow.md)
- ✅ **VII-FORMAT-1** (.editorconfig): Complete configuration documented in [contracts/editorconfig.md](contracts/editorconfig.md)
- ✅ **VIII-TEST-1** (MSTest migration): Comprehensive attribute mapping and migration process documented in [research.md](research.md)

**Implementation Readiness**: All violations have concrete, actionable remediation plans. Implementation can proceed to Phase 2 using [quickstart.md](quickstart.md) as the execution guide.

**Risk Assessment**: LOW - All breaking changes analyzed, rollback strategies defined, incremental validation checkpoints established.

**GATE STATUS UPDATE**: ⚠️ REMEDIATION PLAN APPROVED - Ready for Phase 2 implementation (via `/speckit.tasks` command, not part of `/speckit.plan` scope).

## Project Structure

### Documentation (this feature)

```text
.documentation/specs/main/
├── spec.md             # Feature specification (created)
├── plan.md             # This file (in progress)
├── research.md         # Phase 0 output (to be created)
├── data-model.md       # Phase 1 output (to be created)
├── quickstart.md       # Phase 1 output (to be created)
├── contracts/          # Phase 1 output (to be created)
└── tasks.md            # Phase 2 output (/speckit.tasks command)
```

### Source Code (repository root)

```text
/
├── .editorconfig                      # TO BE CREATED (VII-FORMAT-1)
├── .github/
│   └── workflows/
│       └── main_mapsintegration-fe.yml  # TO BE UPDATED (I-TECH-3)
├── GoogleMapsApi/
│   ├── GoogleMapsApi.csproj           # TO BE UPDATED (I-TECH-1, I-TECH-2)
│   ├── Configuration/
│   ├── Engine/
│   ├── Entities/
│   ├── Extensions/
│   ├── HealthChecks/
│   ├── Monitoring/
│   └── StaticMaps/
├── GoogleMapsApi.FE/
│   ├── GoogleMapsApi.FE.csproj        # TO BE UPDATED (I-TECH-1)
│   ├── Program.cs
│   └── endpoints/
├── GoogleMapsApi.Test/
│   ├── GoogleMapsApi.Test.csproj      # TO BE UPDATED (I-TECH-1, I-TECH-2, VIII-TEST-1)
│   ├── IntegrationTests/              # TO BE MIGRATED (VIII-TEST-1)
│   └── Utils/
└── README.md                          # TO BE UPDATED (documentation)
```

**Structure Decision**: Multi-project .NET solution with library, web API sample, and test projects. This structure aligns with constitution requirements for separation of concerns and supports both NuGet package distribution and Azure App Service deployment.

## Complexity Tracking

> **Constitution violations identified - this plan exists to remediate them**

| Violation | Why It Exists | Remediation Plan |
|-----------|--------------|------------------|
| I-TECH-1: .NET 9 instead of .NET 10 LTS | Projects created before .NET 10 release | Migrate all `<TargetFramework>` to net10.0 |
| I-TECH-2: Nullable reference types not enabled | Legacy codebase predating constitution | Add `<Nullable>enable</Nullable>` to GoogleMapsApi and GoogleMapsApi.Test |
| I-TECH-3: CI/CD using .NET 8 | GitHub Actions workflow not updated | Update workflow to use .NET 10 SDK and publish targets |
| VII-FORMAT-1: No .editorconfig | Missing infrastructure artifact | Create .editorconfig with Microsoft .NET Foundation formatting rules |
| VIII-TEST-1: NUnit instead of MSTest | Test framework chosen before constitution | Migrate all tests from NUnit to MSTest (attributes, assertions, packages) |

**Migration Approach**: Incremental - each violation will be addressed systematically with validation at each step to ensure no functionality is broken.
