namespace UnitTest.Giserver.GeoQuery;

public class StringExtensionsTest
{
    [Fact]
    public void ThrowIfNullTest()
    {
        string? value = null;

        Assert.Throws<ArgumentNullException>(() =>
        {
            value.ThrowIfNullOrWhiteSpace(nameof(value));
        });
    }

    [Fact]
    public void ThrowIfWhiteSpaceTest()
    {
        string? value = "  ";

        Assert.Throws<ArgumentNullException>(() =>
        {
            value?.ThrowIfNullOrWhiteSpace(nameof(value));
        });
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData(" ", null)]
    [InlineData("id,name", new[] { "id", "name" })]
    public void SpliteByCommaTest(string? value, string[]? expected)
    {
        var result = value.SpliteByComma();

        Assert.Equal(expected, result);
    }
}