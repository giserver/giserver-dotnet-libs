using Giserver.Gdal;
using MaxRev.Gdal.Core;

namespace UnitTest.Giserver.Gdal;

public class ConverterTest
{
    [Fact]
    public void DxfConvertTest()
    {
        GdalBase.ConfigureAll();

        string dxfFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "1.dxf");
        string kmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "1.kml");

        var jsons = Converter.VectorFileToGeometries(kmlFile)!.Select(x => x.ExportToJson(null)).ToList();
    }
}