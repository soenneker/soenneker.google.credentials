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

/// <inheritdoc cref="IGoogleCredentialsUtil"/>
public class GoogleCredentialsUtil: IGoogleCredentialsUtil
{
    private readonly SingletonDictionary<ICredential> _credentials;

    public GoogleCredentialsUtil(IFileUtil fileUtil)
    {
        _credentials = new SingletonDictionary<ICredential>(async (filename, token, args) =>
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Resources", filename);

            MemoryStream stream = await fileUtil.ReadFileToMemoryStream(path).NoSync();

            GoogleCredential credential = GoogleCredential.FromStream(stream).CreateScoped(["https://www.googleapis.com/auth/indexing"]);

            await stream.DisposeAsync().NoSync();

            return credential.UnderlyingCredential;
        });
    }

    public ValueTask<ICredential> Get(string fileName, CancellationToken cancellationToken = default)
    {
        return _credentials.Get(fileName, cancellationToken);
    }

    public ValueTask Remove(string fileName, CancellationToken cancellationToken = default)
    {
        return _credentials.Remove(fileName, cancellationToken);
    }

    public void RemoveSync(string fileName, CancellationToken cancellationToken = default)
    {
        _credentials.RemoveSync(fileName, cancellationToken);
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
