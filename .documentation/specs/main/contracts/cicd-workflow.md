# CI/CD Workflow Contract

**Date**: 2026-02-15 | **Type**: GitHub Actions Configuration

## Overview

This contract defines the required GitHub Actions workflow configuration to ensure continuous integration and deployment pipelines use .NET 10 and enforce constitutional compliance.

---

## Workflow File Location

**Path**: `.github/workflows/main_mapsintegration-fe.yml`  
**Trigger**: Push to `main` branch or manual dispatch

---

## Required Workflow Structure

### 1. Build Job

```yaml
jobs:
  build:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4

      # CONSTITUTION REQUIREMENT: .NET 10 SDK
      - name: Set up .NET Core
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.x'

      - name: Build with dotnet
        run: dotnet build --configuration Release

      # CONSTITUTION REQUIREMENT: Format validation (optional but recommended)
      - name: Verify code formatting
        run: dotnet format --verify-no-changes --verbosity diagnostic
        continue-on-error: true  # Warning only initially

      # CONSTITUTION REQUIREMENT: Run tests
      - name: Run tests
        run: dotnet test --configuration Release --no-build --verbosity normal

      # CONSTITUTION REQUIREMENT: Publish with .NET 10 target
      - name: Publish Projects
        run: |
          dotnet publish ./GoogleMapsApi/GoogleMapsApi.csproj -c Release -f net10.0 -o ${{env.DOTNET_ROOT}}/myapp
          dotnet publish ./GoogleMapsApi.FE/GoogleMapsApi.FE.csproj -c Release -f net10.0 -o ${{env.DOTNET_ROOT}}/myapp

      - name: Upload artifact for deployment job
        uses: actions/upload-artifact@v4
        with:
          name: .net-app
          path: ${{env.DOTNET_ROOT}}/myapp
          retention-days: 1
```

### 2. Deploy Job

```yaml
  deploy:
    runs-on: ubuntu-latest
    needs: build
    environment:
      name: 'Production'
      url: ${{ steps.deploy-to-webapp.outputs.webapp-url }}
    permissions:
      id-token: write

    steps:
      - name: Download artifact from build job
        uses: actions/download-artifact@v4
        with:
          name: .net-app
      
      - name: Login to Azure
        uses: azure/login@v2
        with:
          client-id: ${{ secrets.AZUREAPPSERVICE_CLIENTID_F3D35930EEF544B8930DAB6B61311596 }}
          tenant-id: ${{ secrets.AZUREAPPSERVICE_TENANTID_9B57AF66B39045A69868C8E50CC09A0A }}
          subscription-id: ${{ secrets.AZUREAPPSERVICE_SUBSCRIPTIONID_D6D64C73C00749C8A03BFF5FBA07E602 }}

      - name: Deploy to Azure Web App
        id: deploy-to-webapp
        uses: azure/webapps-deploy@v3
        with:
          app-name: 'MapsIntegration-fe'
          slot-name: 'Production'
          package: .
```

---

## Changes from Current State

| Component | Current | Required | Rationale |
|-----------|---------|----------|-----------|
| `actions/setup-dotnet` version | `@v1` | `@v4` | Latest stable version with .NET 10 support |
| `dotnet-version` | `'8.x'` | `'10.x'` | Constitution requires .NET 10 LTS |
| `include-prerelease` | `true` | Remove | .NET 10 is stable LTS, not prerelease |
| Publish framework target | `-f net8.0` | `-f net10.0` | Match project target framework |
| `actions/upload-artifact` | `@v3` | `@v4` | Latest stable version |
| `actions/download-artifact` | `@v3` | `@v4` | Latest stable version |
| `azure/login` | `@v1` | `@v2` | Latest stable version |
| `azure/webapps-deploy` | `@v2` | `@v3` | Latest stable version |
| Format validation | Not present | Add (optional) | Enforce .editorconfig compliance |
| Test execution | Not present | Add | Validate tests before deployment |

---

## Validation Steps

The workflow MUST enforce these validation gates:

1. **Build Gate**: `dotnet build` must succeed (exit code 0)
2. **Test Gate**: `dotnet test` must pass all tests
3. **Format Gate** (recommended): `dotnet format --verify-no-changes` should warn on violations
4. **Publish Gate**: `dotnet publish` must succeed for all projects

**Failure Handling**: Any gate failure must block deployment to production.

---

## Environment Requirements

### GitHub Actions Runner

- **OS**: ubuntu-latest (Linux)
- **SDK**: .NET 10 SDK installed via actions/setup-dotnet@v4
- **Secrets**: Azure service principal credentials configured

### Azure App Service

- **Runtime**: .NET 10
- **Configuration**: 
  - `DOTNET_VERSION`: 10.0
  - `WEBSITE_RUN_FROM_PACKAGE`: 1 (recommended)

---

## Security Considerations

**Secrets Management**:
- Azure credentials stored in GitHub Secrets (already compliant ✅)
- No secrets in workflow file (already compliant ✅)
- Uses Azure federated identity with OIDC (already compliant ✅)

**Permissions**:
- `id-token: write` for OIDC authentication (already compliant ✅)
- Minimal permissions principle followed (already compliant ✅)

---

## Compliance Mapping

| Constitutional Principle | Workflow Clause | Status |
|-------------------------|-----------------|--------|
| I. Technology Stack | `dotnet-version: '10.x'` | ✅ Enforced |
| VII. Code Formatting | `dotnet format --verify-no-changes` | ⚠️ Optional (recommended) |
| VIII. Testing Standards | `dotnet test` step | ✅ Enforced |
| IX. Azure Platform | Azure deployment steps | ✅ Already compliant |

---

## Rollback Plan

If .NET 10 deployment causes issues:

1. Revert workflow to use `dotnet-version: '9.x'` and `-f net9.0`
2. Redeploy previous artifact from GitHub Actions history
3. Investigate compatibility issues
4. Re-attempt migration with targeted fixes

**Note**: This rollback should only be temporary. Constitution compliance requires .NET 10.

---

## Future Enhancements

- **Code Coverage**: Add `dotnet test --collect:"XPlat Code Coverage"` with 80% threshold enforcement
- **NuGet Publishing**: Automate NuGet package push on tag creation
- **Multi-Environment**: Add staging slot deployments before production
- **Performance Testing**: Add basic load testing before production deployment
