using OSGeo.OGR;

namespace Giserver.Gdal;

public class Converter
{
    public static IEnumerable<Geometry>? VectorFileToGeometries(string path)
    {
        using var ds = Ogr.Open(path, 0);
        if (ds != null)
            for (var i = 0; i < ds.GetLayerCount(); i++)
            {
                var layer = ds.GetLayerByIndex(i);
                if (layer == null) continue;

                Feature? feature = layer.GetNextFeature();
                while (feature != null)
                {
                    var geometry = feature.GetGeometryRef();
                    yield return geometry;
                    feature = layer.GetNextFeature();
                }
            }
    }
}