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
        _credentials = new SingletonDictionary<ICredential>(async (fileName, token, args) =>
        {
            if (args.Length < 1 || args[0] is not string[] scopes)
                throw new ArgumentException("Scopes must be passed as string[] in args[0]");

            string path = Path.Combine(Environment.CurrentDirectory, "Resources", fileName);

            await using MemoryStream stream = await fileUtil.ReadToMemoryStream(path).NoSync();

            GoogleCredential credential = GoogleCredential
                .FromStream(stream)
                .CreateScoped(scopes);

            return credential.UnderlyingCredential;
        });
    }

    public ValueTask<ICredential> Get(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        var key = $"{fileName}:{string.Join(",", scopes)}";
        return _credentials.Get(key, cancellationToken, scopes);
    }

    public ValueTask Remove(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        var key = $"{fileName}:{string.Join(",", scopes)}";
        return _credentials.Remove(key, cancellationToken);
    }

    public void RemoveSync(string fileName, string[] scopes, CancellationToken cancellationToken = default)
    {
        var key = $"{fileName}:{string.Join(",", scopes)}";
        _credentials.RemoveSync(key, cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _credentials.DisposeAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _credentials.Dispose();
    }
}
