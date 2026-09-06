using MoeFleaRefresh.Localization;
using Xunit;

namespace MoeFleaRefresh.Tests;

public sealed class FleaRefreshLocalizerTests
{
    [Theory]
    [InlineData("ch")]
    [InlineData("zh-CN")]
    [InlineData("zh_Hans")]
    [InlineData("cn")]
    public void DetectsChineseLocaleCodes(string languageCode)
    {
        Assert.True(FleaRefreshLocalizer.IsChinese(languageCode));
    }

    [Theory]
    [InlineData("en")]
    [InlineData("de-DE")]
    [InlineData("system")]
    [InlineData(null)]
    public void OtherLocaleCodesUseEnglishFallback(string? languageCode)
    {
        Assert.False(FleaRefreshLocalizer.IsChinese(languageCode));
    }
}
