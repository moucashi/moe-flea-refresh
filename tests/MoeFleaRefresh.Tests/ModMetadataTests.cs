using Xunit;

namespace MoeFleaRefresh.Tests;

public sealed class ModMetadataTests
{
    [Fact]
    public void UsesPublishedModGuidAndVersion()
    {
        var metadata = new ModMetadata();

        Assert.Equal("moe.flea.refresh", metadata.ModGuid);
        Assert.Equal("1.1.0", metadata.Version.ToString());
    }
}
