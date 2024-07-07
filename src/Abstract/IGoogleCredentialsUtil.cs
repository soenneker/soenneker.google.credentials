using Google.Apis.Auth.OAuth2;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Google.Credentials.Abstract;

/// <summary>
/// An async thread-safe singleton for Google OAuth credentials
/// </summary>
public interface IGoogleCredentialsUtil : IDisposable, IAsyncDisposable
{
    ValueTask<ICredential> Get(string fileName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Should be used if the component using is disposed (unless the entire app is being disposed).
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    ValueTask Remove(string fileName, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="Remove(string, CancellationToken)"/>"/>
    void RemoveSync(string fileName, CancellationToken cancellationToken = default);
}