# giserver [中文](./README.ZH.md)

提供了一些常用的 webgis api

- marker 存储
- 通过 postgis 对 `mvt` `geobuf` `geojson` 数据进行查询
- 支持 swagger 中 geojson 或者 wkt 序列化

## Packages

| Package                                                                                                                       | Nuget                                                                                                                                                                  | Downloads                                                                               |
| ----------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- |
| [Giserver.GeoQuery](https://www.nuget.org/packages/Giserver.GeoQuery)                                                         | [![Nuget](https://img.shields.io/nuget/v/Giserver.GeoQuery)](https://www.nuget.org/packages/Giserver.GeoQuery)                                                         | ![Nuget](https://img.shields.io/nuget/dt/Giserver.GeoQuery)                             |
| [Giserver.Mark.EFCore](https://www.nuget.org/packages/Giserver.Mark.EFCore)                                                   | [![Nuget](https://img.shields.io/nuget/v/Giserver.Mark.EFCore)](https://www.nuget.org/packages/Giserver.Mark.EFCore)                                                   | ![Nuget](https://img.shields.io/nuget/dt/Giserver.Mark.EFCore)                          |
| [Giserver.NetTopologySuite.Swagger.NSwag](https://www.nuget.org/packages/Giserver.NetTopologySuite.Swagger.NSwag)             | [![Nuget](https://img.shields.io/nuget/v/Giserver.NetTopologySuite.Swagger.NSwag)](https://www.nuget.org/packages/Giserver.NetTopologySuite.Swagger.NSwag)             | ![Nuget](https://img.shields.io/nuget/dt/Giserver.NetTopologySuite.Swagger.NSwag)       |
| [Giserver.NetTopologySuite.Swagger.Swashbuckle](https://www.nuget.org/packages/Giserver.NetTopologySuite.Swagger.Swashbuckle) | [![Nuget](https://img.shields.io/nuget/v/Giserver.NetTopologySuite.Swagger.Swashbuckle)](https://www.nuget.org/packages/Giserver.NetTopologySuite.Swagger.Swashbuckle) | ![Nuget](https://img.shields.io/nuget/dt/Giserver.NetTopologySuite.Swagger.Swashbuckle) |

#### Giserver.GeoQuery

```csharp
services.AddGeoQuery(options =>
{
    options.ConnectionString = configuration.GetConnectionString("geo_query")!;
});

app.UseGeoQuery(options =>
{
    options.Prefix = "api/geo";
    options.IsConnectionStringTemplate = false;
    options.GeoJsonRouteHandlerOption.Allowed = false;
});
```

#### Giserver.Mark.EFCore

```csharp
services.AddGeoMarker(configuration.GetConnectionString("geo_marker")!);

app.UseGeoMarker(options =>
{

});
```

#### Giserver.NetTopologySuite.Swagger.NSwag

```csharp
builder.Services.AddSwaggerDocument(settings =>
{
    settings.TypeMappers.AddGeometry(GeoSerializeType.Geojson);
});
```

#### Giserver.NetTopologySuite.Swagger.Swashbuckle

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.AddGeometry(GeoSerializeType.Geojson);
});
```
