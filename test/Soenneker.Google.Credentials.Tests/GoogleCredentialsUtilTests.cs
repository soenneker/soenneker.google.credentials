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
}
