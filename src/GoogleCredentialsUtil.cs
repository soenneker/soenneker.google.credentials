using Soenneker.Google.Credentials.Abstract;
using Soenneker.Utils.File.Abstract;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Soenneker.Utils.SingletonDictionary;

namespace Soenneker.Google.Credentials;

/// <inheritdoc cref="IGoogleCredentialsUtil"/>
public class GoogleCredentialsUtil: IGoogleCredentialsUtil
{
    private readonly SingletonDictionary<ICredential> _credentials;

    public GoogleCredentialsUtil(IFileUtil fileUtil)
    {
        _credentials = new SingletonDictionary<ICredential>(async args =>
        {
            var fileName = (string)args!.First();

            string path = Path.Combine(Environment.CurrentDirectory, "Resources", fileName);

            MemoryStream stream = await fileUtil.ReadFileToMemoryStream(path);

            GoogleCredential credential = GoogleCredential.FromStream(stream).CreateScoped(["https://www.googleapis.com/auth/indexing"]);

            await stream.DisposeAsync();

            return credential.UnderlyingCredential;
        });
    }

    public ValueTask<ICredential> Get(string fileName)
    {
        return _credentials.Get(fileName, fileName);
    }

    public ValueTask Remove(string fileName)
    {
        return _credentials.Remove(fileName);
    }

    public void RemoveSync(string fileName)
    {
        _credentials.RemoveSync(fileName);
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
