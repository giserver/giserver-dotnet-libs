using System.IO;

namespace UnitTest.Giserver.CAD;

public class CadFileFormatConvertTest
{
    private readonly string dwgFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "1.dwg");
    private readonly string dxfFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "1.dxf");

    public CadFileFormatConvertTest()
    {
        if (File.Exists(dxfFile))
            File.Delete(dxfFile);
    }

    [Fact]
    public async Task DwgFileToDxfFile()
    {
        await new ACADConverter().DwgToDxfAsync(dwgFile, dxfFile);

        Assert.True(File.Exists(dxfFile));
    }

    [Fact]
    public async Task DwgFileToDxfStream()
    {
        using var stream = new FileStream(dxfFile, FileMode.Create, FileAccess.Write);

        await new ACADConverter().DwgToDxfAsync(dwgFile, stream);

        Assert.True(File.Exists(dxfFile));
        Assert.True(new FileInfo(dxfFile).Length > 0);
    }

    [Fact]
    public async Task DwgStreamToDxfFile()
    {
        using var stream = new FileStream(dwgFile, FileMode.Open, FileAccess.Read);

        await new ACADConverter().DwgToDxfAsync(stream, dxfFile);

        Assert.True(File.Exists(dxfFile));
    }

    [Fact]
    public async Task DwgStreamToDxfStream()
    {
        using var dwgStream = new FileStream(dwgFile, FileMode.Open, FileAccess.Read);
        using var dxfStream = new FileStream(dxfFile, FileMode.Create, FileAccess.Write);

        await new ACADConverter().DwgToDxfAsync(dwgStream, dxfStream);

        Assert.True(File.Exists(dxfFile));
        Assert.True(new FileInfo(dxfFile).Length > 0);
    }
}