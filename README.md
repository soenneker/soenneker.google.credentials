[![](https://img.shields.io/nuget/v/soenneker.google.credentials.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.google.credentials/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.google.credentials/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.google.credentials/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.google.credentials.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.google.credentials/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.google.credentials/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.google.credentials/actions/workflows/codeql.yml)

# Soenneker.Google.Credentials

Loads Google service-account JSON files from an application's output directory and caches scoped credentials for reuse.

## Install

```bash
dotnet add package Soenneker.Google.Credentials
```

## Add the credential file

Keep the service-account JSON outside source control and copy it to `LocalResources` in the application output:

```xml
<ItemGroup>
  <Content Include="LocalResources\google-service-account.json"
           CopyToOutputDirectory="PreserveNewest" />
</ItemGroup>
```

`Get()` accepts a path relative to that `LocalResources` directory. Absolute paths and paths that escape it are rejected.

## Register

```csharp
using Soenneker.Google.Credentials.Registrars;
using Microsoft.Extensions.DependencyInjection;

services.AddGoogleCredentialsUtilAsSingleton();
```

Singleton registration is recommended when scoped consumers should share the credential cache. Disposing a scoped consumer does not destroy this singleton. `AddGoogleCredentialsUtilAsScoped()` remains available when each scope deliberately needs an independent cache.

## Get a scoped credential

```csharp
ICredential credential = await credentials.Get(
    "google-service-account.json",
    ["https://www.googleapis.com/auth/indexing"],
    cancellationToken);
```

The file must contain a Google service-account credential. Application-default credentials and interactive user OAuth flows are not loaded by this package.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `Get(fileName, scopes)` | Loads and scopes a credential, or returns its cached instance. | Filename and ordered scope values form the cache key. |
| `Remove(fileName, scopes)` | Asynchronously removes the matching cached credential. | Returns whether an entry existed. |
| `RemoveSync(fileName, scopes)` | Synchronously removes the matching cached credential. | Useful only when asynchronous removal is not available to the caller. |

## Practical notes

- Scope arrays are snapshotted when a credential is cached, so callers may safely reuse or modify their original arrays afterward.
- Scope order is significant: the same scopes in a different order create a different cache entry.
- Let the DI container dispose registered instances. Manually constructed instances must be disposed to release cached credentials.
