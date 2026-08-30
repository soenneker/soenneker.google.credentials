using Google.Apis.Auth.OAuth2;
using Soenneker.Dictionaries.SingletonKeys;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Google.Credentials.Abstract;
using Soenneker.Utils.File.Abstract;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Google.Credentials.Utils;

namespace Soenneker.Google.Credentials;

public sealed class GoogleCredentialsUtil : IGoogleCredentialsUtil
{
    private readonly SingletonKeyDictionary<CredentialKey, ICredential, string, string[]> _credentials;
    private readonly IFileUtil _fileUtil;

    public GoogleCredentialsUtil(IFileUtil fileUtil)
    {
        _fileUtil = fileUtil;
        _credentials = new SingletonKeyDictionary<CredentialKey, ICredential, string, string[]>(CreateCredential);
    }

    private async ValueTask<ICredential> CreateCredential(CredentialKey _, string fileName, string[] scopes, CancellationToken token)
    {
        string resourcesDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "LocalResources"));
        string path = Path.GetFullPath(Path.Combine(resourcesDirectory, fileName));
        string relativePath = Path.GetRelativePath(resourcesDirectory, path);

        if (Path.IsPathRooted(relativePath) || relativePath == ".." || relativePath.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal) ||
            relativePath.StartsWith($"..{Path.AltDirectorySeparatorChar}", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("The credential file must be located beneath the application's LocalResources directory.");
        }

        await using MemoryStream stream = await _fileUtil.ReadToMemoryStream(path, true, token)
                                                         .NoSync();

        ServiceAccountCredential sa = await CredentialFactory.FromStreamAsync<ServiceAccountCredential>(stream, token)
                                                             .NoSync();

        GoogleCredential googleCredential = sa.ToGoogleCredential()
                                              .CreateScoped(scopes);

        return googleCredential.UnderlyingCredential;
    }

    public ValueTask<ICredential> Get(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        string[] scopeSnapshot = [.. scopes];
        var key = new CredentialKey(fileName, scopeSnapshot);
        return _credentials.Get(key, fileName, scopeSnapshot, cancellationToken);
    }

    public ValueTask<bool> Remove(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        var key = new CredentialKey(fileName, scopes);
        return _credentials.Remove(key, cancellationToken);
    }

    public void RemoveSync(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        var key = new CredentialKey(fileName, scopes);
        _credentials.RemoveSync(key, cancellationToken);
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public ValueTask DisposeAsync() => _credentials.DisposeAsync();

    /// <summary>
    /// Releases resources used by the current instance.
    /// </summary>
    public void Dispose() => _credentials.Dispose();
}
