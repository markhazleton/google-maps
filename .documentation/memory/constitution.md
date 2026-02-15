<!--
═══════════════════════════════════════════════════════════════════════════════
SYNC IMPACT REPORT - Constitution Formalization
═══════════════════════════════════════════════════════════════════════════════

VERSION CHANGE: Template (unpopulated) → 1.0.0 (initial ratification)

PRINCIPLES ADDED:
  1. Technology Stack (MANDATORY) - .NET 10 LTS + MSTest
  2. Asynchronous Programming (MANDATORY) - async/await for all I/O
  3. API Documentation (MANDATORY) - XML documentation required
  4. Structured Logging (MANDATORY) - ILogger only, no Console
  5. Dependency Injection (MANDATORY) - Constructor injection pattern
  6. Security & Secret Management (MANDATORY) - User Secrets/Key Vault
  7. Code Formatting & Style (MANDATORY) - .editorconfig enforcement
  8. Testing Standards (MANDATORY) - MSTest + 80% coverage
  9. Azure Platform Standards (MANDATORY) - Azure-first architecture

ADDITIONAL STANDARDS ADDED:
  - Error Handling (custom exceptions, correlation IDs)
  - HTTP Client Management (IHttpClientFactory pattern)

TEMPLATES REQUIRING UPDATES:
  ✅ plan-template.md - Added Constitution Check section with all 9 MUST principles
  ✅ spec-template.md - Added constitution compliance notes in Requirements section
  ✅ tasks-template.md - Added constitution compliance verification task in Phase 1
  ⚠️  commands/*.md - Review for any agent-specific references (recommended follow-up)

FOLLOW-UP REQUIRED:
  1. Create .editorconfig file with C# formatting rules
  2. Migrate projects from .NET 9 → .NET 10 LTS
  3. Migrate test framework from NUnit → MSTest
  4. Enable nullable reference types in GoogleMapsApi and GoogleMapsApi.Test
  5. Add code coverage tooling with 80% threshold to CI/CD
  6. Update GitHub Actions workflow to .NET 10

DISCOVERY SOURCE:
  - Method: Automated pattern discovery via speckit.discover-constitution
  - Files Analyzed: 145 source files + 25 test files
  - Interactive Questions: 10 principle decisions confirmed by user
  - Date: 2026-02-15

═══════════════════════════════════════════════════════════════════════════════
-->

# Google Maps API .NET Constitution

## Core Principles

### I. Technology Stack (MANDATORY)

All projects in this repository MUST adhere to the following technology standards:

- **Target Framework**: .NET 10 (LTS) or later
- **Testing Framework**: MSTest for all unit and integration tests
- **Language Version**: Latest C# language features enabled
- **Nullable Reference Types**: Enabled in all projects for type safety

**Rationale**: .NET 10 LTS provides long-term support and stability. MSTest is the Microsoft standard testing framework with first-class Visual Studio integration. Nullable reference types catch null reference bugs at compile time, significantly reducing production NullReferenceExceptions.

### II. Asynchronous Programming (MANDATORY)

All I/O operations MUST use async/await patterns:

- All API calls, database operations, and file I/O MUST be asynchronous
- Public methods performing I/O MUST return `Task` or `Task<T>`
- Library code MUST use `ConfigureAwait(false)` to avoid deadlocks
- Long-running operations MUST support `CancellationToken`

**Rationale**: Async/await maximizes scalability in ASP.NET applications by preventing thread pool starvation. This is critical for APIs that may handle hundreds of concurrent requests.

### III. API Documentation (MANDATORY)

All public APIs MUST be fully documented with XML documentation comments:

- Public classes MUST have XML `<summary>` documentation
- Public methods MUST document `<summary>`, `<param>`, and `<returns>` tags
- Public properties MUST have `<summary>` documentation
- All library projects MUST enable `GenerateDocumentationFile` in .csproj

**Rationale**: XML documentation enables IntelliSense for consumers, improves developer experience, and generates API documentation automatically. This is essential for a public NuGet package.

### IV. Structured Logging (MANDATORY)

All logging MUST use structured logging via Microsoft.Extensions.Logging:

- Use `ILogger` or `ILogger<T>` for all logging
- NO `Console.WriteLine`, `Debug.WriteLine`, or `Trace.WriteLine` in production code
- Log with appropriate levels: Trace, Debug, Information, Warning, Error, Critical
- Include structured data in log messages using log scopes where applicable

**Rationale**: Structured logging enables proper observability, log aggregation, and troubleshooting in distributed systems. Console logging prevents proper integration with Azure Application Insights and other monitoring solutions.

### V. Dependency Injection (MANDATORY)

All dependencies MUST be managed through dependency injection:

- Use constructor injection for all dependencies
- NO static dependencies, singletons, or service locator patterns
- Register services using `IServiceCollection` extension methods
- Use appropriate service lifetimes: Singleton, Scoped, or Transient

**Rationale**: Dependency injection enables testability, loose coupling, and follows .NET Core best practices. It allows consumers to substitute implementations (e.g., mocking IHttpClientFactory in tests).

### VI. Security & Secret Management (MANDATORY)

Secrets and sensitive data MUST never be committed to source control:

- API keys, passwords, and connection strings MUST use User Secrets (local dev) or Azure Key Vault (production)
- NO secrets in appsettings.json, code, or any committed files
- Environment variables MAY be used as fallback for CI/CD pipelines
- Configuration loading order MUST be: appsettings.json → User Secrets → Environment Variables → Azure Key Vault

**Rationale**: Prevents credential leaks and follows security best practices. Complies with OWASP guidelines and Azure security baselines. GitHub secret scanning will flag committed credentials.

### VII. Code Formatting & Style (MANDATORY)

Code formatting MUST be consistent and enforced:

- `.editorconfig` MUST exist at repository root with C# formatting rules
- Code formatting violations MUST be caught in CI/CD pipelines
- `ImplicitUsings` MUST be disabled (explicit using statements required)
- Namespace MUST match folder structure
- Files SHOULD be focused and small (target: under 300 lines)

**Rationale**: Consistent formatting reduces code review friction and prevents merge conflicts. Explicit usings improve code clarity and make dependencies obvious.

### VIII. Testing Standards (MANDATORY)

All code MUST be thoroughly tested with automated tests:

- All new code MUST have MSTest unit tests
- Minimum 80% code coverage required for all new code
- Coverage violations MUST block PR merges in CI/CD
- Integration tests MUST exist for all public API endpoints
- Tests MUST follow Arrange-Act-Assert pattern

**Rationale**: High test coverage catches regressions early, documents expected behavior, and enables confident refactoring. The 80% threshold balances thoroughness with development velocity.

### IX. Azure Platform Standards (MANDATORY)

All code MUST be designed for Azure as the primary deployment platform:

- Use Azure Key Vault for secrets management in production
- Implement health check endpoints for Azure App Service monitoring
- Follow Azure best practices for configuration, logging, and telemetry
- Design for deployment to Azure App Service, Azure Functions, or Azure Container Apps
- SHOULD integrate with Azure Application Insights for observability

**Rationale**: Azure is the organization's chosen cloud platform. Standardizing on Azure enables reusable infrastructure patterns, operational consistency, and leverages Azure-specific optimizations.

## Additional Standards

### Error Handling

- Custom exception classes SHOULD derive from `Exception` with meaningful properties (e.g., StatusCode, CorrelationId)
- Include correlation IDs SHOULD be included for tracing across distributed calls
- Exceptions MUST be logged with full context before re-throwing

### HTTP Client Management

- `IHttpClientFactory` MUST be used for all HTTP clients (prevents socket exhaustion)
- Resilience patterns (timeouts, retries) SHOULD be configured via HttpClient configuration
- Compression and keep-alive SHOULD be enabled for performance

## Migration Requirements

Based on this constitution, the following migration tasks are required to bring the codebase into compliance:

1. **Framework Migration**: Upgrade all projects from .NET 9 to .NET 10 LTS
2. **Testing Migration**: Migrate from NUnit to MSTest across all test projects
3. **Nullable Migration**: Enable `<Nullable>enable</Nullable>` in GoogleMapsApi and GoogleMapsApi.Test projects
4. **EditorConfig**: Create `.editorconfig` file at repository root with C# formatting rules
5. **Coverage Tooling**: Add code coverage measurement and enforcement (80% threshold) to CI/CD pipeline
6. **CI/CD Update**: Update GitHub Actions workflow from .NET 8 to .NET 10

These tasks SHOULD be tracked as GitHub issues and prioritized based on impact and effort.

## Governance

### Constitution Authority

This constitution supersedes informal practices and tribal knowledge. It represents the codified architectural decisions and quality standards for this project.

### Pull Request Compliance

- All pull requests MUST be reviewed for compliance with these principles
- Violations of MUST principles are blocking and require remediation before merge
- SHOULD principles require justification if not followed
- Reviewers MUST verify that new code includes tests and documentation

### Amendment Process

Constitution amendments require:

1. Written proposal documenting the change rationale and impact
2. Team discussion and consensus (or maintainer approval for solo projects)
3. Migration plan for existing code if the amendment affects existing patterns
4. Update to this document with semantic version bump:
   - **MAJOR**: Breaking governance changes (e.g., removing a MUST principle)
   - **MINOR**: New principles added or existing principles expanded
   - **PATCH**: Clarifications, typos, non-semantic improvements

### Compliance Validation

- Run `/speckit.site-audit` to check codebase compliance with constitution
- Technical debt for SHOULD principles SHOULD be tracked in GitHub Issues
- Constitution compliance SHOULD be reviewed quarterly or before major releases

### Enforcement

Automated tooling SHOULD enforce these principles where possible:

- .editorconfig for code formatting
- Code coverage tools for test coverage thresholds
- Static analysis tools for nullable reference type violations
- PR checks for documentation generation warnings

**Version**: 1.0.0 | **Ratified**: 2026-02-15 | **Last Amended**: 2026-02-15
