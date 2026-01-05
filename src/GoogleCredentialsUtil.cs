using Soenneker.Google.Credentials.Abstract;
using Soenneker.Utils.File.Abstract;
using Google.Apis.Auth.OAuth2;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Utils.SingletonDictionary;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Google.Credentials;

///<inheritdoc cref="IGoogleCredentialsUtil"/>
public sealed class GoogleCredentialsUtil : IGoogleCredentialsUtil
{
    private readonly SingletonDictionary<ICredential, string, string[]> _credentials;
    private readonly IFileUtil _fileUtil;

    public GoogleCredentialsUtil(IFileUtil fileUtil)
    {
        _fileUtil = fileUtil;
        _credentials = new SingletonDictionary<ICredential, string, string[]>(CreateCredential);
    }

    private async ValueTask<ICredential> CreateCredential(string key, CancellationToken token, string fileName, string[] scopes)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "LocalResources", fileName);

        await using MemoryStream stream = await _fileUtil.ReadToMemoryStream(path, true, token)
                                                         .NoSync();

        // Service account JSON -> ServiceAccountCredential via CredentialFactory, then to GoogleCredential, then scope.
        ServiceAccountCredential sa = await CredentialFactory.FromStreamAsync<ServiceAccountCredential>(stream, token)
                                                             .ConfigureAwait(false);

        GoogleCredential googleCredential = sa.ToGoogleCredential()
                                              .CreateScoped(scopes);

        return googleCredential.UnderlyingCredential;
    }

    public ValueTask<ICredential> Get(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        string key = BuildKey(fileName, scopes);
        return _credentials.Get(key, fileName, scopes, cancellationToken);
    }

    public ValueTask Remove(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        string key = BuildKey(fileName, scopes);
        return _credentials.Remove(key, cancellationToken);
    }

    public void RemoveSync(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        string key = BuildKey(fileName, scopes);
        _credentials.RemoveSync(key, cancellationToken);
    }

    public ValueTask DisposeAsync() => _credentials.DisposeAsync();

    public void Dispose() => _credentials.Dispose();

    private static string BuildKey(string fileName, string[] scopes) => $"{fileName}:{string.Join("|", scopes)}";
}