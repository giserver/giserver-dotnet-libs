namespace Giserver.NetTopologySuite.Swagger.Swashbuckle;

public static class Extension
{
    public static void AddGeometry(this SwaggerGenOptions options, GeoSerializeType type)
    {
        switch (type)
        {
            case GeoSerializeType.Geojson:
                options.SchemaFilter<SwashbuckleGeojsonSchemaFilter>();
                break;

            case GeoSerializeType.Wkt:
                options.SchemaFilter<SwashbuckleWktSchemaFilter>();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}