using Soenneker.Google.Credentials.Abstract;
using Soenneker.Utils.File.Abstract;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Utils.SingletonDictionary;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Google.Credentials;

///<inheritdoc cref="IGoogleCredentialsUtil"/>
public sealed class GoogleCredentialsUtil : IGoogleCredentialsUtil
{
    private readonly SingletonDictionary<ICredential> _credentials;

    public GoogleCredentialsUtil(IFileUtil fileUtil)
    {
        _credentials = new SingletonDictionary<ICredential>(async (key, token, args) =>
        {
            if (args.Length < 2 || args[0] is not string fileName || args[1] is not string[] scopes)
                throw new ArgumentException("Expected args[0] as string fileName and args[1] as string[] scopes");

            string path = Path.Combine(AppContext.BaseDirectory, "LocalResources", fileName);

            await using MemoryStream stream = await fileUtil.ReadToMemoryStream(path, true, token).NoSync();

            GoogleCredential credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);

            return credential.UnderlyingCredential;
        });
    }

    public ValueTask<ICredential> Get(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        var key = $"{fileName}:{string.Join("|", scopes)}";
        return _credentials.Get(key, cancellationToken, fileName, scopes);
    }

    public ValueTask Remove(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        var key = $"{fileName}:{string.Join("|", scopes)}";
        return _credentials.Remove(key, cancellationToken);
    }

    public void RemoveSync(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        var key = $"{fileName}:{string.Join("|", scopes)}";
        _credentials.RemoveSync(key, cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        return _credentials.DisposeAsync();
    }

    public void Dispose()
    {
        _credentials.Dispose();
    }
}
