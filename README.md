[![](https://img.shields.io/nuget/v/soenneker.google.credentials.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.google.credentials/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.google.credentials/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.google.credentials/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.google.credentials.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.google.credentials/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.google.credentials/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.google.credentials/actions/workflows/codeql.yml)

# Soenneker.Google.Credentials

A utility for retrieving and caching Google credentials with dynamic scopes.

## Install

```bash
dotnet add package Soenneker.Google.Credentials
```

## Quick start

```csharp
using Soenneker.Google.Credentials.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddGoogleCredentialsUtilAsSingleton();
```

Adds `IGoogleCredentialsUtil` as a singleton service.

## What you get

- `IGoogleCredentialsUtil` — A utility for retrieving and caching Google credentials with dynamic scopes.
- `GoogleCredentialsUtilRegistrar` — An async thread-safe singleton for Google OAuth credentials.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IGoogleCredentialsUtil.Get(fileName, scopes, cancellationToken)` | Gets a scoped Google credential from a specified service account file. | The scoped `ICredential`. |
| `IGoogleCredentialsUtil.Remove(fileName, scopes, cancellationToken)` | Removes a cached credential for a specific file and scope set. | true if removes a cached credential for a specific file and scope set; otherwise, false. |
| `IGoogleCredentialsUtil.RemoveSync(fileName, scopes, cancellationToken)` | Removes a cached credential for a specific file and scope set (synchronous). | Returns no value; the requested change is complete when the method returns. |
| `GoogleCredentialsUtilRegistrar.AddGoogleCredentialsUtilAsSingleton(services)` | Adds `IGoogleCredentialsUtil` as a singleton service. | The same service collection, so additional registrations can be chained. |
| `GoogleCredentialsUtilRegistrar.AddGoogleCredentialsUtilAsScoped(services)` | Adds `IGoogleCredentialsUtil` as a scoped service. | The same service collection, so additional registrations can be chained. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
- Calls that return a cached or singleton value reuse the same instance until the owning service is disposed.
- Dispose instances you own when their scope ends so held resources can be released.
