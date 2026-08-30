using System;
using System.Threading.Tasks;
using Soenneker.Google.Credentials.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Google.Credentials.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class GoogleCredentialsUtilTests : HostedUnitTest
{
    private readonly IGoogleCredentialsUtil _util;

    public GoogleCredentialsUtilTests(Host host) : base(host)
    {
        _util = Resolve<IGoogleCredentialsUtil>(true);
    }

    [Test]
    public void Default()
    {

    }

    [Test]
    public async Task Get_rejects_paths_outside_local_resources()
    {
        Func<Task> act = async () => await _util.Get("..\\service-account.json", []);

        await Assert.That(act).Throws<InvalidOperationException>();
    }
}
