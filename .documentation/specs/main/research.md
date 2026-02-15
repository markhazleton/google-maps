# Research: Constitution Compliance Migration

**Date**: 2026-02-15 | **Plan**: [plan.md](plan.md)

## Research Questions

Based on Technical Context analysis, the following unknowns require resolution:

1. What are the breaking changes between .NET 9 and .NET 10 LTS?
2. What is the migration path from NUnit to MSTest?
3. What .editorconfig rules align with .NET Foundation standards?
4. What is the strategy for enabling nullable reference types in existing codebases?
5. How to update GitHub Actions workflow for .NET 10?

## Research Findings

### 1. .NET 10 LTS Migration

**Decision**: Migrate from `net9.0` to `net10.0` in all `<TargetFramework>` elements

**Key Changes .NET 9 → .NET 10**:
- **LTS Status**: .NET 10 is a Long-Term Support (LTS) release with 3 years of support
- **Breaking Changes**: Minimal - strong backward compatibility from .NET 9
- **Performance**: Improved JIT compilation and GC performance
- **API Surface**: New APIs for collections, networking, and diagnostics
- **SDK Version**: Requires .NET 10 SDK (10.0.x)

**Migration Steps**:
1. Update `<TargetFramework>net9.0</TargetFramework>` → `<TargetFramework>net10.0</TargetFramework>`
2. Update package references (Microsoft.Extensions.* should use version 10.x)
3. Test all functionality - no code changes expected
4. Update developer documentation with .NET 10 SDK requirement

**Rationale**: Constitution mandates LTS releases for long-term stability. .NET 10 provides extended support lifecycle compared to .NET 9 (18 months).

**Alternatives Considered**:
- Stay on .NET 9: Rejected - violates constitution, shorter support window
- Skip to .NET 11: Rejected - not yet released, .NET 10 is current LTS

**Risk**: Low - .NET 10 maintains high compatibility with .NET 9

---

### 2. NUnit to MSTest Migration

**Decision**: Migrate all test projects from NUnit 4.x to MSTest (Microsoft.VisualStudio.TestTools)

**Mapping Table**:

| NUnit | MSTest | Notes |
|-------|--------|-------|
| `[Test]` | `[TestMethod]` | Test method attribute |
| `[TestFixture]` | `[TestClass]` | Test class attribute |
| `[SetUp]` | `[TestInitialize]` | Before each test |
| `[TearDown]` | `[TestCleanup]` | After each test |
| `[OneTimeSetUp]` | `[ClassInitialize]` | Before all tests in class (requires static method + TestContext parameter) |
| `[OneTimeTearDown]` | `[ClassCleanup]` | After all tests in class (requires static method) |
| `[TestCase(...)]` | `[DataRow(...)]` + `[DataTestMethod]` | Parameterized tests |
| `Assert.That(x, Is.EqualTo(y))` | `Assert.AreEqual(y, x)` | Note parameter order reversed! |
| `Assert.That(x, Is.Not.Null)` | `Assert.IsNotNull(x)` | Null checks |
| `Assert.That(x, Is.True)` | `Assert.IsTrue(x)` | Boolean checks |
| `Assert.That(() => x, Throws.Exception)` | `Assert.ThrowsException<TException>(() => x)` | Exception assertions |
| `Assert.Ignore("reason")` | `Assert.Inconclusive("reason")` | Skip test |

**Package Changes**:
```xml
<!-- REMOVE -->
<PackageReference Include="NUnit" Version="4.3.2" />
<PackageReference Include="NUnit3TestAdapter" Version="5.0.0" />

<!-- ADD -->
<PackageReference Include="MSTest.TestFramework" Version="3.7.0" />
<PackageReference Include="MSTest.TestAdapter" Version="3.7.0" />
<!-- Microsoft.NET.Test.Sdk already present - keep at 17.14.0+ -->
```

**Migration Process**:
1. Update package references in GoogleMapsApi.Test.csproj
2. Global find/replace for attributes (`[Test]` → `[TestMethod]`, etc.)
3. Convert assertions (carefully - parameter order changes!)
4. Handle `[TestCase]` → `[DataRow]` + `[DataTestMethod]` conversions manually
5. Update `[OneTimeSetUp]`/`[OneTimeTearDown]` to static methods with correct signatures
6. Run all tests to verify behavior unchanged
7. Remove NUnit using statements, add `using Microsoft.VisualStudio.TestTools.UnitTesting;`

**Rationale**: Constitution mandates MSTest as the standard testing framework. MSTest has first-class Visual Studio integration and is Microsoft's recommended framework for .NET projects.

**Alternatives Considered**:
- Keep NUnit: Rejected - violates constitution (VIII. Testing Standards)
- Migrate to xUnit: Rejected - constitution specifies MSTest specifically

**Risk**: Medium - Manual assertion conversion required, potential for behavioral differences

---

### 3. EditorConfig for C# .NET Projects

**Decision**: Create `.editorconfig` at repository root with Microsoft .NET Foundation formatting rules

**Recommended Rules** (subset for this project):

```ini
# EditorConfig is awesome: https://EditorConfig.org

# Top-most EditorConfig file
root = true

# All files
[*]
charset = utf-8
insert_final_newline = true
trim_trailing_whitespace = true
indent_style = space
indent_size = 4

# Code files
[*.{cs,csx,vb,vbx}]
indent_size = 4

# XML project files
[*.{csproj,vbproj,vcxproj,vcxproj.filters,proj,projitems,shproj}]
indent_size = 2

# JSON files
[*.json]
indent_size = 2

# C# files
[*.cs]

# New line preferences
csharp_new_line_before_open_brace = all
csharp_new_line_before_else = true
csharp_new_line_before_catch = true
csharp_new_line_before_finally = true

# Indentation preferences
csharp_indent_case_contents = true
csharp_indent_switch_labels = true

# Space preferences
csharp_space_after_cast = false
csharp_space_after_keywords_in_control_flow_statements = true
csharp_space_between_method_declaration_parameter_list_parentheses = false

# var preferences
csharp_style_var_for_built_in_types = false:suggestion
csharp_style_var_when_type_is_apparent = true:suggestion
csharp_style_var_elsewhere = false:suggestion

# Expression-bodied members
csharp_style_expression_bodied_methods = false:silent
csharp_style_expression_bodied_properties = true:suggestion

# Null-checking preferences
csharp_style_conditional_delegate_call = true:suggestion
csharp_style_throw_expression = true:suggestion

# Code quality
dotnet_code_quality_unused_parameters = all:warning

# Naming conventions
dotnet_naming_rule.interface_should_be_begins_with_i.severity = warning
dotnet_naming_rule.interface_should_be_begins_with_i.symbols = interface
dotnet_naming_rule.interface_should_be_begins_with_i.style = begins_with_i

dotnet_naming_rule.types_should_be_pascal_case.severity = warning
dotnet_naming_rule.types_should_be_pascal_case.symbols = types
dotnet_naming_rule.types_should_be_pascal_case.style = pascal_case

dotnet_naming_rule.non_field_members_should_be_pascal_case.severity = warning
dotnet_naming_rule.non_field_members_should_be_pascal_case.symbols = non_field_members
dotnet_naming_rule.non_field_members_should_be_pascal_case.style = pascal_case

# Symbol specifications
dotnet_naming_symbols.interface.applicable_kinds = interface
dotnet_naming_symbols.interface.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected

dotnet_naming_symbols.types.applicable_kinds = class, struct, interface, enum
dotnet_naming_symbols.types.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected

dotnet_naming_symbols.non_field_members.applicable_kinds = property, event, method
dotnet_naming_symbols.non_field_members.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected

# Naming styles
dotnet_naming_style.begins_with_i.required_prefix = I
dotnet_naming_style.begins_with_i.capitalization = pascal_case

dotnet_naming_style.pascal_case.capitalization = pascal_case
```

**CI/CD Integration**: Add `dotnet format --verify-no-changes` to GitHub Actions workflow to enforce formatting in PRs.

**Rationale**: Consistent formatting reduces merge conflicts, improves code review efficiency, and aligns with .NET Foundation standards.

**Alternatives Considered**:
- No .editorconfig: Rejected - violates constitution (VII. Code Formatting & Style)
- Custom rules: Rejected - Microsoft .NET Foundation rules are industry standard

**Risk**: Low - .editorconfig is non-breaking, only enforces formatting consistency

---

### 4. Nullable Reference Types Migration

**Decision**: Enable `<Nullable>enable</Nullable>` in GoogleMapsApi.csproj and GoogleMapsApi.Test.csproj

**Migration Strategy**:

1. **Initial Enablement**:
   - Add `<Nullable>enable</Nullable>` to `<PropertyGroup>`
   - Build and collect all nullable reference type warnings

2. **Warning Handling Approach** (phased):
   - **Phase 1 (this plan)**: Enable nullable, accept warnings (non-blocking)
   - **Phase 2 (future)**: Incrementally fix warnings file by file
   - **Phase 3 (future)**: Treat nullable warnings as errors (`<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`)

3. **Common Fixes**:
   - Add `?` to reference type properties/parameters that can be null
   - Use `!` (null-forgiving operator) for validated non-null scenarios
   - Use `?.` (null-conditional) for safe member access
   - Initialize non-nullable properties in constructor or with `= null!;` if guaranteed initialization elsewhere

**Best Practices**:
- Public API parameters: Be explicit with nullability (`string?` vs `string`)
- Private fields: Use nullable annotations where actual nullability exists
- POCO entities: Mark all reference types as nullable unless constraint exists
- Validated scenarios: Use `ArgumentNullException.ThrowIfNull(param)` to prove to compiler

**Rationale**: Nullable reference types catch ~80% of NullReferenceExceptions at compile time, significantly improving code quality. Constitution mandates this for type safety.

**Alternatives Considered**:
- Stay with nullable disabled: Rejected - violates constitution, leaves code vulnerable to NRE
- Enable with `<TreatWarningsAsErrors>`: Rejected for initial migration - too disruptive, phase in gradually

**Risk**: Medium - Will generate warnings; requires discipline to address incrementally without degrading quality

---

### 5. GitHub Actions for .NET 10

**Decision**: Update `.github/workflows/main_mapsintegration-fe.yml` to use .NET 10 SDK

**Required Changes**:

```yaml
# CHANGE FROM:
- name: Set up .NET Core
  uses: actions/setup-dotnet@v1
  with:
    dotnet-version: '8.x'
    include-prerelease: true

# CHANGE TO:
- name: Set up .NET Core
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: '10.x'
```

```yaml
# CHANGE FROM:
dotnet publish ./GoogleMapsApi/GoogleMapsApi.csproj -c Release -f net8.0 -o ${{env.DOTNET_ROOT}}/myapp

# CHANGE TO:
dotnet publish ./GoogleMapsApi/GoogleMapsApi.csproj -c Release -f net10.0 -o ${{env.DOTNET_ROOT}}/myapp
```

**Additional Improvements**:
- Update `actions/checkout@v4` (already at v4 ✅)
- Update `actions/upload-artifact@v4` (currently v3)
- Update `actions/download-artifact@v4` (currently v3)
- Consider adding `dotnet format --verify-no-changes` for .editorconfig enforcement

**Rationale**: CI/CD must match target framework to ensure builds test the correct runtime version. Constitution requires .NET 10 LTS for production deployments.

**Alternatives Considered**:
- Multi-target workflow testing .NET 8/9/10: Rejected - adds complexity, constitution mandates .NET 10 only
- Keep .NET 8 in CI: Rejected - violates constitution and creates CI/prod mismatch

**Risk**: Low - GitHub Actions runners support .NET 10, straightforward update

---

## Technology Stack Summary

**Selected Technologies**:

| Component | Technology | Version | Rationale |
|-----------|-----------|---------|-----------|
| Framework | .NET | 10.0 (LTS) | Constitution mandated LTS, long-term support |
| Testing | MSTest | 3.7.0+ | Constitution mandated, first-class VS integration |
| SDK | .NET SDK | 10.0.x | Required for .NET 10 compilation |
| CI/CD | GitHub Actions | actions/setup-dotnet@v4 | Existing infrastructure, .NET 10 support |
| Formatting | EditorConfig | Core spec | Constitutional requirement, industry standard |
| Type Safety | Nullable Reference Types | Enabled | Constitutional requirement, NRE prevention |

**Dependencies to Update**:
- Microsoft.Extensions.Configuration.Abstractions: 9.0.5 → 10.0.x
- Microsoft.Extensions.Diagnostics.HealthChecks: 9.0.0 → 10.0.x

---

## Migration Sequence

Based on research findings, the recommended implementation order:

1. **Update .editorconfig** (VII-FORMAT-1) - Non-breaking, establish baseline
2. **Update .csproj files** (I-TECH-1, I-TECH-2) - net9.0→net10.0, enable nullable
3. **Update package references** - Microsoft.Extensions.* to 10.x versions
4. **Migrate tests to MSTest** (VIII-TEST-1) - Validate all tests pass
5. **Update CI/CD workflow** (I-TECH-3) - Ensure automated builds use .NET 10
6. **Update documentation** - README.md, developer setup instructions
7. **Validate** - Full test suite, health checks, NuGet package build

---

## Risks and Mitigations

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| .NET 10 breaking changes affect runtime | Low | High | Thorough integration testing, stage rollout |
| NUnit→MSTest assertion differences | Medium | Medium | Manual verification of each test, side-by-side execution |
| Nullable warnings overwhelming | Medium | Low | Accept warnings initially, fix incrementally in follow-up |
| CI/CD failures after .NET 10 update | Low | High | Test workflow in feature branch first, rollback plan ready |
| NuGet package consumer breakage | Low | High | Semantic versioning, maintain backward compatibility |

---

## Open Questions

None - All NEEDS CLARIFICATION items from Technical Context have been resolved.
