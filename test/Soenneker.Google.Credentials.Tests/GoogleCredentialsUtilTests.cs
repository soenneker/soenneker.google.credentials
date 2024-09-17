using Soenneker.Google.Credentials.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Google.Credentials.Tests;

[Collection("Collection")]
public class GoogleCredentialsUtilTests : FixturedUnitTest
{
    private readonly IGoogleCredentialsUtil _util;

    public GoogleCredentialsUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IGoogleCredentialsUtil>(true);
    }
}
