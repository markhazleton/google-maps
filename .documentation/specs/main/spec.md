# Feature Specification: Constitution Compliance - Critical Site Audit Issues

**Date**: 2026-02-15 | **Status**: Planning | **Priority**: Critical

## Overview

Address five critical constitution violations identified in the site audit that prevent the repository from meeting established governance standards. These violations affect core infrastructure requirements including target framework, testing framework, type safety, code formatting, and CI/CD configuration.

## Problem Statement

The site audit has identified critical misalignments between the codebase and the constitution:

1. **I-TECH-1**: Projects targeting .NET 9 instead of .NET 10 LTS (violates Technology Stack principle)
2. **I-TECH-2**: Nullable reference types not enabled in GoogleMapsApi.csproj (violates Technology Stack principle)
3. **I-TECH-3**: CI/CD pipeline using .NET 8 instead of .NET 10 (violates Technology Stack + Azure Platform principles)
4. **VII-FORMAT-1**: No .editorconfig file at repository root (violates Code Formatting & Style principle)
5. **VIII-TEST-1**: Using NUnit instead of MSTest (violates Testing Standards principle)

These violations are **MANDATORY** principle violations per the constitution and must be resolved before new feature work can proceed.

## Requirements

### Functional Requirements

**FR-1: Framework Migration**
- Migrate all .csproj files from .NET 9 to .NET 10 LTS
- Projects affected: GoogleMapsApi, GoogleMapsApi.FE, GoogleMapsApi.Test
- Update all `<TargetFramework>` elements to `net10.0`

**FR-2: Nullable Reference Types**
- Enable nullable reference types in GoogleMapsApi.csproj and GoogleMapsApi.Test.csproj
- Add `<Nullable>enable</Nullable>` to project files
- Address any nullable reference type warnings that emerge (may be deferred to follow-up issues)

**FR-3: Testing Framework Migration**
- Migrate all test projects from NUnit to MSTest
- Replace NUnit package references with MSTest equivalents
- Convert all test attributes and assertions to MSTest syntax
- Preserve all existing test coverage and behavior

**FR-4: EditorConfig Creation**
- Create .editorconfig file at repository root
- Include C# formatting rules aligned with .NET foundation guidelines
- Configure for consistency with existing codebase patterns
- Enable enforcement in CI/CD pipeline

**FR-5: CI/CD Pipeline Update**
- Update GitHub Actions workflow from .NET 8 to .NET 10
- Ensure all CI/CD steps use .NET 10 SDK
- Add .editorconfig validation to CI pipeline
- Verify all tests pass after migration

### Non-Functional Requirements

**NFR-1: Zero Downtime**
- Migrations must not break existing functionality
- All tests must pass after each change
- NuGet package consumers must not be affected

**NFR-2: Documentation**
- Update README.md with new .NET version requirements
- Document any breaking changes for contributors
- Update developer setup instructions

**NFR-3: Validation**
- Run full test suite after each migration step
- Verify NuGet package builds successfully
- Validate health check endpoints still respond

### Constitution Compliance

This feature directly addresses violations of the following MANDATORY constitutional principles:

- **I. Technology Stack**: .NET 10 LTS + MSTest requirement
- **VII. Code Formatting & Style**: .editorconfig requirement
- **VIII. Testing Standards**: MSTest requirement
- **IX. Azure Platform Standards**: CI/CD alignment with .NET 10

## Success Criteria

1. All projects target .NET 10 LTS (`<TargetFramework>net10.0</TargetFramework>`)
2. Nullable reference types enabled in GoogleMapsApi and GoogleMapsApi.Test
3. All tests use MSTest framework (zero NUnit references remaining)
4. .editorconfig file exists at repository root with C# rules
5. GitHub Actions workflow uses .NET 10 SDK
6. All automated tests pass
7. `/speckit.site-audit` shows zero critical violations for addressed items

## Out of Scope

- Addressing nullable reference type warnings in existing code (deferred to follow-up)
- Code coverage enforcement (requires separate tooling setup)
- Migration of existing .NET 8/9 documentation beyond developer setup
- Performance optimization unrelated to framework migration

## Dependencies

- .NET 10 SDK must be available in GitHub Actions
- MSTest packages must be compatible with .NET 10
- Development team must have .NET 10 SDK installed locally

## Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Breaking changes in .NET 10 | High | Thorough testing after migration; staged rollout |
| NUnit→MSTest conversion errors | Medium | Side-by-side validation; manual test execution |
| CI/CD pipeline failures | Medium | Test locally before committing; rollback plan ready |
| Nullable reference type warnings | Low | Address incrementally; warnings are non-blocking initially |
| NuGet package consumer compatibility | Medium | Test with sample consumer project; verify no breaking changes |

## Timeline

- **Phase 0 (Research)**: 1 hour - Review .NET 10 breaking changes, MSTest migration patterns
- **Phase 1 (Design)**: 2 hours - Plan migration sequence, create .editorconfig template
- **Phase 2 (Implementation)**: 4-6 hours - Execute migrations, test, validate
- **Total Estimated Effort**: 1 day

## Notes

This is a technical debt remediation effort required by constitution compliance. It unblocks future feature development and ensures the project adheres to established governance standards.
