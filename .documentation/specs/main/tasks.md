# Tasks: Constitution Compliance - Critical Site Audit Issues

**Feature**: Constitution compliance migration addressing 5 critical violations  
**Input**: Design documents from `.documentation/specs/main/`  
**Prerequisites**: [plan.md](plan.md), [spec.md](spec.md), [research.md](research.md), [quickstart.md](quickstart.md), [contracts/](contracts/)

**Implementation Guide**: Follow [quickstart.md](quickstart.md) for detailed step-by-step instructions with validation checkpoints.

---

## Format: `- [ ] [ID] [P?] [Issue] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Issue]**: Which constitutional violation this task addresses
- Include exact file paths in descriptions

---

## Phase 1: Setup & Code Formatting (Non-Breaking Foundation)

**Purpose**: Establish code formatting baseline without affecting runtime behavior

**Issue Addressed**: VII-FORMAT-1 (No .editorconfig file)

- [ ] T001 Review .editorconfig specification in contracts/editorconfig.md
- [ ] T002 Create .editorconfig file at repository root with Microsoft .NET Foundation C# formatting rules
- [ ] T003 Verify formatting tool recognizes config: `dotnet format --verify-no-changes --verbosity diagnostic`
- [ ] T004 [P] Run constitution compliance check: Review .documentation/memory/constitution.md against current state
- [ ] T005 Commit .editorconfig: `git add .editorconfig && git commit -m "Add .editorconfig for constitutional compliance (VII-FORMAT-1)"`

**Checkpoint**: ✅ .editorconfig exists at repository root and is recognized by formatter

---

## Phase 2: .NET 10 Migration & Nullable Reference Types

**Purpose**: Upgrade all projects to .NET 10 LTS and enable nullable reference types

**Issues Addressed**: I-TECH-1 (.NET version), I-TECH-2 (Nullable reference types)

### Project Configuration Updates

- [ ] T006 [P] Update GoogleMapsApi/GoogleMapsApi.csproj: `<TargetFrameworks>net9.0</TargetFrameworks>` → `<TargetFrameworks>net10.0</TargetFrameworks>`
- [ ] T007 [P] Add nullable reference types to GoogleMapsApi/GoogleMapsApi.csproj: Insert `<Nullable>enable</Nullable>` in PropertyGroup
- [ ] T008 [P] Update GoogleMapsApi.FE/GoogleMapsApi.FE.csproj: `<TargetFramework>net9.0</TargetFramework>` → `<TargetFramework>net10.0</TargetFramework>`
- [ ] T009 [P] Update GoogleMapsApi.Test/GoogleMapsApi.Test.csproj: `<TargetFrameworks>net9.0</TargetFrameworks>` → `<TargetFrameworks>net10.0</TargetFrameworks>`
- [ ] T010 [P] Add nullable reference types to GoogleMapsApi.Test/GoogleMapsApi.Test.csproj: Insert `<Nullable>enable</Nullable>` in PropertyGroup

### Package Updates

- [ ] T011 [P] Update GoogleMapsApi/GoogleMapsApi.csproj: Microsoft.Extensions.Configuration.Abstractions 9.0.5 → 10.0.0
- [ ] T012 [P] Update GoogleMapsApi/GoogleMapsApi.csproj: Microsoft.Extensions.Diagnostics.HealthChecks 9.0.0 → 10.0.0
- [ ] T013 [P] Update GoogleMapsApi.Test/GoogleMapsApi.Test.csproj: Microsoft.Extensions.Configuration.UserSecrets 9.0.5 → 10.0.0
- [ ] T014 [P] Update GoogleMapsApi.Test/GoogleMapsApi.Test.csproj: System.Text.Json 9.0.5 → 10.0.0

### Build Validation

- [ ] T015 Clean previous build outputs: `dotnet clean`
- [ ] T016 Restore packages with .NET 10: `dotnet restore`
- [ ] T017 Build solution targeting .NET 10: `dotnet build --configuration Release`
- [ ] T018 Run existing tests (still NUnit) to verify no regressions: `dotnet test --configuration Release`
- [ ] T019 Commit .NET 10 migration: `git add **/*.csproj && git commit -m "Migrate to .NET 10 LTS and enable nullable reference types (I-TECH-1, I-TECH-2)"`

**Checkpoint**: ✅ All projects target net10.0, nullable reference types enabled, build succeeds, tests pass

---

## Phase 3: Test Framework Migration (NUnit → MSTest)

**Purpose**: Migrate all test projects from NUnit to MSTest per constitutional requirements

**Issue Addressed**: VIII-TEST-1 (Using NUnit instead of MSTest)

### Package Migration

- [ ] T020 Update GoogleMapsApi.Test/GoogleMapsApi.Test.csproj: Remove NUnit package reference (version 4.3.2)
- [ ] T021 Update GoogleMapsApi.Test/GoogleMapsApi.Test.csproj: Remove NUnit3TestAdapter package reference (version 5.0.0)
- [ ] T022 Update GoogleMapsApi.Test/GoogleMapsApi.Test.csproj: Add MSTest.TestFramework package reference (version 3.7.0)
- [ ] T023 Update GoogleMapsApi.Test/GoogleMapsApi.Test.csproj: Add MSTest.TestAdapter package reference (version 3.7.0)
- [ ] T024 Restore test packages: `dotnet restore GoogleMapsApi.Test/GoogleMapsApi.Test.csproj`

### Test File Migration - Using Statements

- [ ] T025 [P] Update GlobalUsings.cs: Replace `using NUnit.Framework;` with `using Microsoft.VisualStudio.TestTools.UnitTesting;`
- [ ] T026 [P] Update all test files in GoogleMapsApi.Test/: Replace NUnit using statements with MSTest equivalents
- [ ] T027 [P] Update all test files in GoogleMapsApi.Test/IntegrationTests/: Replace NUnit using statements with MSTest equivalents

### Test File Migration - Attributes & Assertions (Unit Tests)

- [ ] T028 [P] Update GoogleMapsApi.Test/LocationToStringTest.cs: Convert NUnit attributes to MSTest ([Test]→[TestMethod], [TestFixture]→[TestClass])
- [ ] T029 [P] Update GoogleMapsApi.Test/LocationToStringTest.cs: Convert assertions (Assert.That→Assert.AreEqual, parameter order!)
- [ ] T030 [P] Update GoogleMapsApi.Test/StaticMaps.cs: Convert NUnit attributes to MSTest
- [ ] T031 [P] Update GoogleMapsApi.Test/StaticMaps.cs: Convert assertions
- [ ] T032 [P] Update GoogleMapsApi.Test/UnixTimeConverterTest.cs: Convert NUnit attributes to MSTest
- [ ] T033 [P] Update GoogleMapsApi.Test/UnixTimeConverterTest.cs: Convert assertions

### Test File Migration - Integration Tests

- [ ] T034 [P] Update GoogleMapsApi.Test/IntegrationTests/BaseTestIntegration.cs: Convert [SetUp]→[TestInitialize], [OneTimeSetUp]→[ClassInitialize] (static + TestContext param)
- [ ] T035 [P] Update GoogleMapsApi.Test/IntegrationTests/DirectionsTests.cs: Convert NUnit to MSTest (attributes + assertions)
- [ ] T036 [P] Update GoogleMapsApi.Test/IntegrationTests/DistanceMatrixTests.cs: Convert NUnit to MSTest (attributes + assertions)
- [ ] T037 [P] Update GoogleMapsApi.Test/IntegrationTests/ElevationTests.cs: Convert NUnit to MSTest (attributes + assertions)
- [ ] T038 [P] Update GoogleMapsApi.Test/IntegrationTests/GeocodingTests.cs: Convert NUnit to MSTest (attributes + assertions)
- [ ] T039 [P] Update GoogleMapsApi.Test/IntegrationTests/PlaceAutocompleteTests.cs: Convert NUnit to MSTest (attributes + assertions)
- [ ] T040 [P] Update GoogleMapsApi.Test/IntegrationTests/PlacesDetailsTests.cs: Convert NUnit to MSTest (attributes + assertions)
- [ ] T041 [P] Update GoogleMapsApi.Test/IntegrationTests/PlacesFindTests.cs: Convert NUnit to MSTest (attributes + assertions)
- [ ] T042 [P] Update GoogleMapsApi.Test/IntegrationTests/PlacesNearByTests.cs: Convert NUnit to MSTest (attributes + assertions)
- [ ] T043 [P] Update GoogleMapsApi.Test/IntegrationTests/PlacesSearchTests.cs: Convert NUnit to MSTest (attributes + assertions)
- [ ] T044 [P] Update GoogleMapsApi.Test/IntegrationTests/PlacesTextTests.cs: Convert NUnit to MSTest (attributes + assertions)
- [ ] T045 [P] Update GoogleMapsApi.Test/IntegrationTests/TimeZoneTests.cs: Convert NUnit to MSTest (attributes + assertions)

### Test Validation

- [ ] T046 Build test project: `dotnet build GoogleMapsApi.Test/GoogleMapsApi.Test.csproj --configuration Release`
- [ ] T047 List discovered tests: `dotnet test GoogleMapsApi.Test/GoogleMapsApi.Test.csproj --list-tests`
- [ ] T048 Run all tests with MSTest: `dotnet test GoogleMapsApi.Test/GoogleMapsApi.Test.csproj --configuration Release --verbosity normal`
- [ ] T049 Verify all tests pass (no behavioral changes)
- [ ] T050 Run full solution test suite: `dotnet test --configuration Release`
- [ ] T051 Commit test migration: `git add GoogleMapsApi.Test/ && git commit -m "Migrate test framework from NUnit to MSTest (VIII-TEST-1)"`

**Checkpoint**: ✅ All tests migrated to MSTest, zero NUnit references, all tests passing

---

## Phase 4: CI/CD Pipeline Update

**Purpose**: Update GitHub Actions workflow to use .NET 10 SDK and enforce constitutional compliance

**Issue Addressed**: I-TECH-3 (CI/CD using .NET 8 instead of .NET 10)

### Workflow Configuration Updates

- [ ] T052 Update .github/workflows/main_mapsintegration-fe.yml: Change `actions/setup-dotnet@v1` to `@v4`
- [ ] T053 Update .github/workflows/main_mapsintegration-fe.yml: Change `dotnet-version: '8.x'` to `'10.x'`
- [ ] T054 Update .github/workflows/main_mapsintegration-fe.yml: Remove `include-prerelease: true` (no longer needed)
- [ ] T055 Update .github/workflows/main_mapsintegration-fe.yml: Update publish step: `-f net8.0` → `-f net10.0` for GoogleMapsApi
- [ ] T056 Update .github/workflows/main_mapsintegration-fe.yml: Update publish step: `-f net8.0` → `-f net10.0` for GoogleMapsApi.FE
- [ ] T057 Update .github/workflows/main_mapsintegration-fe.yml: Change `actions/upload-artifact@v3` to `@v4`
- [ ] T058 Update .github/workflows/main_mapsintegration-fe.yml: Change `actions/download-artifact@v3` to `@v4`
- [ ] T059 Update .github/workflows/main_mapsintegration-fe.yml: Add `retention-days: 1` to upload-artifact step
- [ ] T060 Update .github/workflows/main_mapsintegration-fe.yml: Change `azure/login@v1` to `@v2`
- [ ] T061 Update .github/workflows/main_mapsintegration-fe.yml: Change `azure/webapps-deploy@v2` to `@v3`

### Optional: Add Test & Format Enforcement

- [ ] T062 [P] Add test step to .github/workflows/main_mapsintegration-fe.yml: Insert `dotnet test --configuration Release --no-build` after build
- [ ] T063 [P] Add format verification to .github/workflows/main_mapsintegration-fe.yml: Insert `dotnet format --verify-no-changes` (continue-on-error: true initially)

### CI/CD Validation

- [ ] T064 Commit workflow changes: `git add .github/workflows/ && git commit -m "Update CI/CD pipeline to .NET 10 (I-TECH-3)"`
- [ ] T065 Push to feature branch: `git push origin constitution-compliance-migration`
- [ ] T066 Monitor GitHub Actions build at https://github.com/MarkHazleton/google-maps/actions
- [ ] T067 Verify workflow uses .NET 10 SDK in build logs
- [ ] T068 Verify tests execute successfully in CI
- [ ] T069 Verify artifacts are published with net10.0 target

**Checkpoint**: ✅ CI/CD workflow uses .NET 10, tests run in pipeline, build succeeds

---

## Phase 5: Documentation & Final Validation

**Purpose**: Update developer documentation and perform comprehensive validation

### Documentation Updates

- [ ] T070 Update README.md: Add/update Prerequisites section with .NET 10 SDK requirement
- [ ] T071 Update README.md: Add/update Building section with `dotnet build` command
- [ ] T072 Update README.md: Add/update Testing section mentioning MSTest framework
- [ ] T073 [P] Review and update any other developer documentation referencing .NET version or test framework
- [ ] T074 Commit documentation: `git add README.md && git commit -m "Update documentation for .NET 10 and MSTest"`

### Comprehensive Validation

- [ ] T075 Clean all build outputs: `Remove-Item -Recurse -Force .\*\bin, .\*\obj, .\TestResults`
- [ ] T076 Full clean build from scratch: `dotnet restore && dotnet build --configuration Release`
- [ ] T077 Run complete test suite: `dotnet test --configuration Release`
- [ ] T078 Verify NuGet package builds: `dotnet pack GoogleMapsApi/GoogleMapsApi.csproj --configuration Release`
- [ ] T079 Check package targets net10.0: Inspect GoogleMapsApi/bin/Release/GoogleMapsApi.*.nupkg
- [ ] T079a Test NuGet package consumer compatibility: Create temporary sample consumer project, reference built package, verify no breaking changes
- [ ] T080 [P] Run code formatting check: `dotnet format --verify-no-changes`
- [ ] T081 [P] Verify no NUnit references remain: `Get-ChildItem -Recurse -Filter *.csproj | Select-String "NUnit"`

### Optional: Local Runtime Testing

- [ ] T082 Start GoogleMapsApi.FE locally: `cd GoogleMapsApi.FE && dotnet run`
- [ ] T083 Test health check endpoint: `curl https://localhost:5001/health` (should return Healthy)
- [ ] T084 Test sample API endpoints to verify runtime behavior unchanged

### Constitution Compliance Verification

- [ ] T085 Review constitution at .documentation/memory/constitution.md
- [ ] T086 Verify I. Technology Stack: All projects target net10.0 ✓
- [ ] T087 Verify I. Technology Stack (Nullable): GoogleMapsApi and GoogleMapsApi.Test have nullable reference types enabled ✓
- [ ] T088 Verify VII. Code Formatting: .editorconfig exists at root ✓
- [ ] T089 Verify VIII. Testing Standards: All tests use MSTest framework ✓
- [ ] T090 Verify IX. Azure Platform: CI/CD uses .NET 10 SDK ✓

**Checkpoint**: ✅ All 5 critical violations resolved, full validation complete

---

## Phase 6: Pull Request & Deployment

**Purpose**: Create PR for review and merge to main branch

### Pull Request Creation

- [ ] T091 Create PR from constitution-compliance-migration to main
- [ ] T092 Fill PR description with changes summary (see quickstart.md Phase 7.1)
- [ ] T093 Reference issues addressed: I-TECH-1, I-TECH-2, I-TECH-3, VII-FORMAT-1, VIII-TEST-1
- [ ] T094 Attach test results and validation evidence
- [ ] T095 Request code review from maintainers

### Post-Merge Tasks

- [ ] T096 Monitor Azure deployment after merge to main
- [ ] T097 Verify production health check endpoint responds
- [ ] T098 Run smoke tests against production environment
- [ ] T099 Document any nullable reference type warnings for follow-up work (create GitHub issues)
- [ ] T100 Close original site audit issues as resolved

**Checkpoint**: ✅ Constitutional compliance achieved, deployed to production

---

## Dependencies & Execution Order

### Phase Dependencies

```
Phase 1 (Setup & EditorConfig)
    ↓
Phase 2 (.NET 10 Migration)
    ↓
Phase 3 (Test Migration)
    ↓
Phase 4 (CI/CD Update)
    ↓
Phase 5 (Documentation & Validation)
    ↓
Phase 6 (Pull Request & Deployment)
```

**Critical Path**: Phases must be completed sequentially. Each phase has validation checkpoints that must pass before proceeding.

### Parallel Opportunities within Phases

**Phase 1**: T003, T004 can run in parallel  
**Phase 2**: T006-T014 (all project/package updates) can run in parallel  
**Phase 3**: T025-T027 (using statements) can run in parallel; T028-T045 (test file migrations) can run in parallel  
**Phase 4**: T062, T063 (optional CI steps) can run in parallel  
**Phase 5**: T073, T080, T081 can run in parallel

### Rollback Strategy

If issues discovered at any checkpoint:
- Phase 1-2: `git reset --hard` to previous commit
- Phase 3-4: Revert specific commits, CI will redeploy previous version
- Phase 5-6: Azure App Service rollback to previous deployment slot

---

## Implementation Strategy

### Sequential Execution (Recommended)

Execute phases 1→2→3→4→5→6 in order with validation at each checkpoint. Estimated time: **4-6 hours** for complete migration.

### MVP Approach

Minimum viable migration to unblock constitutional compliance:
1. Phase 1: EditorConfig (10 min)
2. Phase 2: .NET 10 + Nullable Reference Types (20 min)
3. Phase 3: MSTest Migration (90 min)
4. Phase 4: CI/CD Update (15 min)

Skip Phase 5 optional tasks and go straight to PR.

### Parallel Team Strategy

Not recommended for this migration - tasks are tightly coupled and sequential execution reduces risk.

---

## Task Statistics

- **Total Tasks**: 101
- **Phase 1 (Setup)**: 5 tasks (~10 minutes)
- **Phase 2 (.NET 10)**: 14 tasks (~20 minutes)
- **Phase 3 (MSTest)**: 32 tasks (~90 minutes)
- **Phase 4 (CI/CD)**: 18 tasks (~30 minutes)
- **Phase 5 (Validation)**: 27 tasks (~50 minutes)
- **Phase 6 (PR)**: 5 tasks (~30 minutes)

**Parallel Opportunities**: 35 tasks marked [P] can run concurrently within their phase  
**Critical Path Length**: ~3-4 hours (with parallelization)

---

## Notes

- All file paths are relative to repository root: `C:\GitHub\MarkHazleton\google-maps\`
- Follow [quickstart.md](quickstart.md) for detailed implementation instructions for each task
- Each checkpoint must pass before proceeding to next phase
- Commit frequently (suggested commit points are indicated)
- Nullable reference type warnings are acceptable initially - track for follow-up work
- Test behavioral equivalence is critical - no test logic should change, only syntax
- NUnit Assert.That parameter order differs from MSTest Assert.AreEqual - be careful!

---

## Success Criteria

✅ All 101 tasks completed  
✅ All 5 critical constitutional violations resolved  
✅ All automated tests passing  
✅ .NET 10 SDK used throughout (local + CI/CD)  
✅ NuGet package builds successfully  
✅ Zero NUnit references remaining  
✅ NuGet package consumer compatibility verified  
✅ Production deployment successful  
✅ Health checks green  

**Constitutional Compliance Status**: ACHIEVED 🎉
