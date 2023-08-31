# giserver [中文](./README.ZH.md)

Giserver provide `Geo Apis` base on http
* marker storage
* `mvt` `geobuf` `geojson` query with postgis
* api swagger document

you can also use libs from nuget, make your geography dev quickly

## Packages
| Package | Nuget | Downloads |
|-|-|-|
|[Giserver.GeoQuery](https://www.nuget.org/packages/Giserver.GeoQuery) | [![Nuget](https://img.shields.io/nuget/v/Giserver.GeoQuery)](https://www.nuget.org/packages/Giserver.GeoQuery) | ![Nuget](https://img.shields.io/nuget/dt/Giserver.GeoQuery)
|[Giserver.Mark.EFCore](https://www.nuget.org/packages/Giserver.Mark.EFCore) | [![Nuget](https://img.shields.io/nuget/v/Giserver.Mark.EFCore)](https://www.nuget.org/packages/Giserver.Mark.EFCore) | ![Nuget](https://img.shields.io/nuget/dt/Giserver.Mark.EFCore)
|[Giserver.NetTopologySuite.Swagger.NSwag](https://www.nuget.org/packages/Giserver.NetTopologySuite.Swagger.NSwag) | [![Nuget](https://img.shields.io/nuget/v/Giserver.NetTopologySuite.Swagger.NSwag)](https://www.nuget.org/packages/Giserver.NetTopologySuite.Swagger.NSwag) | ![Nuget](https://img.shields.io/nuget/dt/Giserver.NetTopologySuite.Swagger.NSwag)
|[Giserver.NetTopologySuite.Swagger.Swashbuckle](https://www.nuget.org/packages/Giserver.NetTopologySuite.Swagger.Swashbuckle) | [![Nuget](https://img.shields.io/nuget/v/Giserver.NetTopologySuite.Swagger.Swashbuckle)](https://www.nuget.org/packages/Giserver.NetTopologySuite.Swagger.Swashbuckle) | ![Nuget](https://img.shields.io/nuget/dt/Giserver.NetTopologySuite.Swagger.Swashbuckle)

#### Giserver.GeoQuery
use `postgis` query geo-format data

``` csharp
builder.Services.AddGeoQuery();

app.UseGeoQuery(app.Configuration.GetConnectionString("Template"), options =>
{
    options.Prefix = "api/geo";
    options.IsConnectionStringTemplate = false;
    options.GeoJsonRouteHandlerOption.Allowed = false;
});
```

#### Giserver.Mark.EFCore
``` csharp
services.AddGeoMarker(configuration.GetConnectionString("geo_marker")!);

app.UseGeoMarker(options =>
{
   
});
```

#### Giserver.NetTopologySuite.Swagger.NSwag
``` csharp
builder.Services.AddSwaggerDocument(settings =>
{
    settings.TypeMappers.AddGeometry(GeoSerializeType.Geojson);
});
```

#### Giserver.NetTopologySuite.Swagger.Swashbuckle
``` csharp
builder.Services.AddSwaggerGen(options =>
{
    options.AddGeometry(GeoSerializeType.Geojson);
});
```