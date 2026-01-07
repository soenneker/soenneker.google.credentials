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

///<inheritdoc cref="IGoogleCredentialsUtil"/>
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
        string path = Path.Combine(AppContext.BaseDirectory, "LocalResources", fileName);

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
        var key = new CredentialKey(fileName, scopes);
        return _credentials.Get(key, fileName, scopes, cancellationToken);
    }

    public ValueTask Remove(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        var key = new CredentialKey(fileName, scopes);
        return _credentials.Remove(key, cancellationToken);
    }

    public void RemoveSync(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        var key = new CredentialKey(fileName, scopes);
        _credentials.RemoveSync(key, cancellationToken);
    }

    public ValueTask DisposeAsync() => _credentials.DisposeAsync();

    public void Dispose() => _credentials.Dispose();
}