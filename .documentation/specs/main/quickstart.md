# Quick Start: Constitution Compliance Migration

**Date**: 2026-02-15 | **Plan**: [plan.md](plan.md) | **Estimated Time**: 4-6 hours

## Overview

This guide provides step-by-step instructions to migrate the Google Maps API repository from its current state (.NET 9, NUnit) to constitutional compliance (.NET 10 LTS, MSTest). Follow these phases in order to minimize risk and validate at each step.

---

## Prerequisites

### Install .NET 10 SDK

**Windows/macOS/Linux**:
```powershell
# Download and install from:
https://dotnet.microsoft.com/download/dotnet/10.0

# Verify installation
dotnet --version
# Should output: 10.0.x
```

### Verify Git Clean State

```powershell
cd C:\GitHub\MarkHazleton\google-maps
git status
# Ensure no uncommitted changes before starting
```

### Create Feature Branch (Optional but Recommended)

```powershell
git checkout -b constitution-compliance-migration
```

---

## Phase 1: Create EditorConfig (10 minutes)

### Step 1.1: Create .editorconfig File

See [contracts/editorconfig.md](contracts/editorconfig.md) for full content.

```powershell
# Create file at repository root
New-Item -ItemType File -Path .\.editorconfig
```

Copy the full `.editorconfig` content from the contract document into this file.

### Step 1.2: Verify Formatting

```powershell
# Check current formatting status
dotnet format --verify-no-changes --verbosity diagnostic

# Auto-format all files (optional - can be done later)
dotnet format
```

**Expected Result**: File exists at repository root, formatter recognizes it.

### Step 1.3: Commit

```powershell
git add .editorconfig
git commit -m "Add .editorconfig for constitutional compliance (VII-FORMAT-1)"
```

**✅ CHECKPOINT**: .editorconfig created and committed.

---

## Phase 2: Update Project Files to .NET 10 (20 minutes)

### Step 2.1: Update GoogleMapsApi.csproj

**File**: `GoogleMapsApi/GoogleMapsApi.csproj`

**Changes**:
```xml
<!-- Change from: -->
<TargetFrameworks>net9.0</TargetFrameworks>

<!-- Change to: -->
<TargetFrameworks>net10.0</TargetFrameworks>

<!-- Add after <ImplicitUsings>: -->
<Nullable>enable</Nullable>

<!-- Update package versions: -->
<PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="10.0.0" />
<PackageReference Include="Microsoft.Extensions.Diagnostics.HealthChecks" Version="10.0.0" />
```

### Step 2.2: Update GoogleMapsApi.FE.csproj

**File**: `GoogleMapsApi.FE/GoogleMapsApi.FE.csproj`

**Changes**:
```xml
<!-- Change from: -->
<TargetFramework>net9.0</TargetFramework>

<!-- Change to: -->
<TargetFramework>net10.0</TargetFramework>

<!-- Nullable reference types already enabled ✅ - no change needed -->
```

### Step 2.3: Update GoogleMapsApi.Test.csproj

**File**: `GoogleMapsApi.Test/GoogleMapsApi.Test.csproj`

**Changes**:
```xml
<!-- Change from: -->
<TargetFrameworks>net9.0</TargetFrameworks>

<!-- Change to: -->
<TargetFrameworks>net10.0</TargetFrameworks>

<!-- Add after <OutputType>: -->
<Nullable>enable</Nullable>

<!-- Update package versions: -->
<PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="10.0.0" />
<PackageReference Include="System.Text.Json" Version="10.0.0" />
```

**DO NOT** change test framework packages yet - that's Phase 3.

### Step 2.4: Build and Validate

```powershell
# Clean previous build outputs
dotnet clean

# Restore packages
dotnet restore

# Build solution
dotnet build --configuration Release
```

**Expected Result**: Build succeeds. Nullable reference type warnings may appear (acceptable).

### Step 2.5: Run Tests (Still NUnit)

```powershell
dotnet test --configuration Release
```

**Expected Result**: All tests pass (still using NUnit at this stage).

### Step 2.6: Commit

```powershell
git add GoogleMapsApi/*.csproj GoogleMapsApi.FE/*.csproj GoogleMapsApi.Test/*.csproj
git commit -m "Migrate to .NET 10 LTS and enable nullable reference types (I-TECH-1, I-TECH-2)"
```

**✅ CHECKPOINT**: All projects targeting .NET 10, nullable reference types enabled, tests passing.

---

## Phase 3: Migrate Tests to MSTest (60-90 minutes)

### Step 3.1: Update Test Project Packages

**File**: `GoogleMapsApi.Test/GoogleMapsApi.Test.csproj`

**Changes**:
```xml
<!-- REMOVE these lines: -->
<PackageReference Include="NUnit" Version="4.3.2" />
<PackageReference Include="NUnit3TestAdapter" Version="5.0.0" />

<!-- ADD these lines: -->
<PackageReference Include="MSTest.TestFramework" Version="3.7.0" />
<PackageReference Include="MSTest.TestAdapter" Version="3.7.0" />

<!-- KEEP: -->
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.0" />
```

```powershell
# Restore with new packages
dotnet restore GoogleMapsApi.Test/GoogleMapsApi.Test.csproj
```

### Step 3.2: Update Test File Attributes

**For each test file** in `GoogleMapsApi.Test/` and `GoogleMapsApi.Test/IntegrationTests/`:

**Find and replace** (use IDE or PowerShell):

```powershell
# Using statements
[NUnit.Framework] → [Microsoft.VisualStudio.TestTools.UnitTesting]

# Test attributes
[Test] → [TestMethod]
[TestFixture] → [TestClass]
[SetUp] → [TestInitialize]
[TearDown] → [TestCleanup]

# Assertions (CAREFUL - parameter order changes!)
Assert.That(actual, Is.EqualTo(expected)) → Assert.AreEqual(expected, actual)
Assert.That(value, Is.Not.Null) → Assert.IsNotNull(value)
Assert.That(value, Is.Null) → Assert.IsNull(value)
Assert.That(condition, Is.True) → Assert.IsTrue(condition)
Assert.That(condition, Is.False) → Assert.IsFalse(condition)
```

**Special Cases**:

**[OneTimeSetUp]**:
```csharp
// NUnit:
[OneTimeSetUp]
public void OneTimeSetup()
{
    // ...
}

// MSTest:
[ClassInitialize]
public static void ClassInitialize(TestContext context)
{
    // ...
}
```

**[TestCase]** parameterized tests:
```csharp
// NUnit:
[TestCase("input1", "expected1")]
[TestCase("input2", "expected2")]
[Test]
public void TestMethod(string input, string expected)
{
    // ...
}

// MSTest:
[DataRow("input1", "expected1")]
[DataRow("input2", "expected2")]
[DataTestMethod]
public void TestMethod(string input, string expected)
{
    // ...
}
```

### Step 3.3: Build Tests

```powershell
dotnet build GoogleMapsApi.Test/GoogleMapsApi.Test.csproj --configuration Release
```

**Expected Result**: Build succeeds with no NUnit references remaining.

### Step 3.4: Run Tests

```powershell
dotnet test GoogleMapsApi.Test/GoogleMapsApi.Test.csproj --configuration Release --verbosity normal
```

**Expected Result**: All tests discovered by MSTest and pass.

**Troubleshooting**:
- If tests not discovered: Verify `[TestClass]` and `[TestMethod]` attributes present
- If tests fail: Check assertion parameter order (reversed from NUnit!)
- If async tests fail: Ensure `async Task` return type (not `async void`)

### Step 3.5: Full Solution Test

```powershell
dotnet test --configuration Release
```

**Expected Result**: All tests pass across entire solution.

### Step 3.6: Commit

```powershell
git add GoogleMapsApi.Test/
git commit -m "Migrate test framework from NUnit to MSTest (VIII-TEST-1)"
```

**✅ CHECKPOINT**: Tests migrated to MSTest and passing.

---

## Phase 4: Update CI/CD Workflow (15 minutes)

### Step 4.1: Update GitHub Actions Workflow

**File**: `.github/workflows/main_mapsintegration-fe.yml`

See [contracts/cicd-workflow.md](contracts/cicd-workflow.md) for full content.

**Key Changes**:

```yaml
# Change from:
- name: Set up .NET Core
  uses: actions/setup-dotnet@v1
  with:
    dotnet-version: '8.x'
    include-prerelease: true

# Change to:
- name: Set up .NET Core
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: '10.x'
```

```yaml
# Update publish step from:
dotnet publish ./GoogleMapsApi/GoogleMapsApi.csproj -c Release -f net8.0 -o ${{env.DOTNET_ROOT}}/myapp

# Change to:
dotnet publish ./GoogleMapsApi/GoogleMapsApi.csproj -c Release -f net10.0 -o ${{env.DOTNET_ROOT}}/myapp
dotnet publish ./GoogleMapsApi.FE/GoogleMapsApi.FE.csproj -c Release -f net10.0 -o ${{env.DOTNET_ROOT}}/myapp
```

```yaml
# Update artifact actions:
- uses: actions/upload-artifact@v4   # was @v3
- uses: actions/download-artifact@v4  # was @v3
```

**Optional but recommended** - Add test step:
```yaml
- name: Run tests
  run: dotnet test --configuration Release --no-build --verbosity normal
```

### Step 4.2: Commit

```powershell
git add .github/workflows/main_mapsintegration-fe.yml
git commit -m "Update CI/CD pipeline to .NET 10 (I-TECH-3)"
```

**✅ CHECKPOINT**: CI/CD workflow updated.

---

## Phase 5: Update Documentation (10 minutes)

### Step 5.1: Update README.md

**File**: `README.md`

Add/update prerequisites section:

```markdown
## Prerequisites

- .NET 10 SDK or later ([Download](https://dotnet.microsoft.com/download/dotnet/10.0))
- Visual Studio 2022 (17.10+) or JetBrains Rider 2024.1+
- Azure subscription (for deployment)

## Building

```powershell
dotnet build GoogleMapsApi.sln --configuration Release
```

## Testing

```powershell
dotnet test --configuration Release
```

Test framework: MSTest
```

### Step 5.2: Commit

```powershell
git add README.md
git commit -m "Update documentation for .NET 10 and MSTest"
```

**✅ CHECKPOINT**: Documentation updated.

---

## Phase 6: Final Validation (15 minutes)

### Step 6.1: Full Clean Build

```powershell
# Clean all build outputs
Remove-Item -Recurse -Force .\*\bin, .\*\obj, .\TestResults

# Restore, build, test
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
```

**Expected Result**: Clean build, all tests pass.

### Step 6.2: Verify NuGet Package

```powershell
dotnet pack GoogleMapsApi/GoogleMapsApi.csproj --configuration Release

# Check output in GoogleMapsApi/bin/Release/
# Verify package targets net10.0
```

### Step 6.3: Test Health Check Endpoint (if running locally)

```powershell
# Start the app
cd GoogleMapsApi.FE
dotnet run

# In another terminal:
curl https://localhost:5001/health
# Should return: Healthy
```

### Step 6.4: Push to GitHub

```powershell
git push origin constitution-compliance-migration
```

### Step 6.5: Verify GitHub Actions

- Navigate to: https://github.com/MarkHazleton/google-maps/actions
- Verify workflow runs successfully with .NET 10
- Check that tests are executed and pass

**✅ CHECKPOINT**: All automated checks passing on GitHub.

---

## Phase 7: Create Pull Request

### Step 7.1: Create PR

**Title**: `Constitution Compliance: Migrate to .NET 10 LTS + MSTest`

**Description**:
```markdown
## Overview
Addresses critical site audit violations (I-TECH-1, I-TECH-2, I-TECH-3, VII-FORMAT-1, VIII-TEST-1) to achieve constitutional compliance.

## Changes
- ✅ Migrated all projects from .NET 9 to .NET 10 LTS
- ✅ Enabled nullable reference types in GoogleMapsApi and GoogleMapsApi.Test
- ✅ Migrated test framework from NUnit to MSTest
- ✅ Created .editorconfig with .NET Foundation C# rules
- ✅ Updated CI/CD pipeline to use .NET 10 SDK

## Testing
- All existing tests pass (behavior unchanged)
- Full integration test suite executed
- NuGet package builds successfully
- Health check endpoint validated

## Breaking Changes
None for library consumers. Contributors must install .NET 10 SDK.

## Documentation
- Updated README.md with new prerequisites
- See `.documentation/specs/main/` for full design documentation

## Constitutional Compliance
All 5 critical violations resolved. See `/speckit.site-audit` results.
```

### Step 7.2: Request Review

Assign reviewers and await approval.

---

## Rollback Plan

If critical issues discovered after deployment:

```powershell
# Revert to previous commit
git revert HEAD~7..HEAD

# Or reset branch
git reset --hard <commit-before-migration>
git push --force
```

Azure App Service will automatically redeploy previous version.

---

## Post-Migration Tasks (Future)

1. **Address Nullable Warnings**: Incrementally fix nullable reference type warnings file by file
2. **Code Coverage**: Add code coverage enforcement with 80% threshold
3. **Performance Baseline**: Establish performance benchmarks for .NET 10
4. **Update Dependencies**: Check for updates to FastEndpoints and other packages compatible with .NET 10

---

## Troubleshooting

### Build fails with "SDK not found"
```powershell
# Verify .NET 10 SDK installed
dotnet --list-sdks

# If missing, reinstall from:
https://dotnet.microsoft.com/download/dotnet/10.0
```

### Tests not discovered by MSTest
- Verify `[TestClass]` on class, `[TestMethod]` on methods
- Check test project references `MSTest.TestFramework` and `MSTest.TestAdapter`
- Run: `dotnet test --list-tests` to see test discovery output

### Nullable Reference Type Warnings Overwhelming
- Accept initially - warnings are non-blocking
- Create follow-up GitHub issue to address incrementally
- Can suppress with `#nullable disable` temporarily if needed

### CI/CD fails with .NET 10
- Check GitHub Actions logs for specific error
- Verify runner supports .NET 10 (ubuntu-latest does)
- Check for typos in workflow file (yaml syntax)

---

## Success Criteria

✅ All projects target net10.0  
✅ Nullable reference types enabled in GoogleMapsApi and GoogleMapsApi.Test  
✅ All tests use MSTest framework (zero NUnit references)  
✅ .editorconfig exists at repository root  
✅ GitHub Actions uses .NET 10 SDK  
✅ All automated tests pass  
✅ NuGet package builds successfully  
✅ Health check endpoint responds  
✅ `/speckit.site-audit` shows zero critical violations for addressed items

---

**Total Estimated Time**: 4-6 hours (including testing and validation)  
**Complexity**: Medium (systematic but requires careful validation at each step)  
**Risk**: Low (backward compatible, incremental approach with checkpoints)
