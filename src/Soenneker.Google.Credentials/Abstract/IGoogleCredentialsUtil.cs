using Google.Apis.Auth.OAuth2;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Google.Credentials.Abstract;

/// <summary>
/// A utility for retrieving and caching Google credentials with dynamic scopes
/// </summary>
public interface IGoogleCredentialsUtil : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Gets a scoped Google credential from a specified service account file.
    /// </summary>
    /// <param name="fileName">The name of the credential file (e.g., vertex-ai.json)</param>
    /// <param name="scopes">The OAuth scopes to request</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>The scoped <see cref="ICredential"/></returns>
    ValueTask<ICredential> Get(string fileName, string[] scopes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cached credential for a specific file and scope set.
    /// </summary>
    /// <param name="fileName">Name of the target file.</param>
    /// <param name="scopes">scopes to process.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>true if removes a cached credential for a specific file and scope set; otherwise, false.</returns>
    ValueTask<bool> Remove(string fileName, string[] scopes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cached credential for a specific file and scope set (synchronous).
    /// </summary>
    /// <param name="fileName">Name of the target file.</param>
    /// <param name="scopes">scopes to process.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    void RemoveSync(string fileName, string[] scopes, CancellationToken cancellationToken = default);
}
