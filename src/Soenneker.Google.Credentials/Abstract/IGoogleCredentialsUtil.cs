using Google.Apis.Auth.OAuth2;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Google.Credentials.Abstract;

/// <summary>
/// Loads service-account credentials from <c>LocalResources</c> and caches them by filename and ordered scope set.
/// </summary>
public interface IGoogleCredentialsUtil : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Gets or creates a scoped credential from a service-account JSON file beneath the application's <c>LocalResources</c> directory.
    /// </summary>
    /// <param name="fileName">The resource-relative credential filename, such as <c>vertex-ai.json</c>.</param>
    /// <param name="scopes">The OAuth scopes to request. Scope order is part of the cache key.</param>
    /// <param name="cancellationToken">Token used to cancel credential loading.</param>
    /// <returns>The cached scoped <see cref="ICredential"/>.</returns>
    ValueTask<ICredential> Get(string fileName, string[] scopes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cached credential for a specific file and scope set.
    /// </summary>
    /// <param name="fileName">Name of the target file.</param>
    /// <param name="scopes">scopes to process.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><see langword="true"/> when a cached credential was removed; otherwise, <see langword="false"/>.</returns>
    ValueTask<bool> Remove(string fileName, string[] scopes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cached credential for a specific file and scope set (synchronous).
    /// </summary>
    /// <param name="fileName">Name of the target file.</param>
    /// <param name="scopes">scopes to process.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    void RemoveSync(string fileName, string[] scopes, CancellationToken cancellationToken = default);
}
