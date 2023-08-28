using Giserver.NetTopologySuite.Serialize;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;

namespace Giserver.NetTopologySuite.Swagger.NSwag;

public static class Extension
{
    public static void AddGeometry(this ICollection<ITypeMapper> mappers, GeoSerializeType type)
    {
        var geoMapper = type == GeoSerializeType.Geojson
            ? SerializeSwaggerMappers.GeometryGeojsonMapper
            : SerializeSwaggerMappers.GeometryWktMapper;

        var jsonObjectType = type == GeoSerializeType.Geojson ? JsonObjectType.Object : JsonObjectType.String;

        foreach (var item in
                 geoMapper.Where(item => mappers.All(x => x.MappedType != item.Key)))
        {
            mappers.Add(new PrimitiveTypeMapper(item.Key,
                schema4 =>
                {
                    schema4.Type = jsonObjectType;
                    schema4.Example = item.Value;
                }));
        }
    }
}