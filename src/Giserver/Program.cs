#if DEBUG

using Giserver.NetTopologySuite.Swagger.Swashbuckle;

#endif

using Giserver.Mark.EFCore.Extensions;
using Giserver.GeoQuery.Extensions;
using Giserver.NetTopologySuite.Serialize;

var builder = WebApplication.CreateSlimBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var isDev = builder.Environment.IsDevelopment();

configuration.AddJsonFile(isDev ? "appsettings.dev.json" : "appsettings.json");

services.AddGeoMarker(configuration.GetConnectionString("geo_marker")!);
services.AddGeoQuery();

services.AddEndpointsApiExplorer();

#if DEBUG
services.AddSwaggerGen(o =>
{
    o.AddGeometry(GeoSerializeType.Geojson);
});
#endif

var app = builder.Build();

#if DEBUG
app.UseSwagger();
app.UseSwaggerUI();
#endif

app.UseGeoMarker(options =>
{
    options.RouteHandlerBuilderAction = b =>
    {
        b.WithTags("geo_marker");
    };
});
app.UseGeoQuery(configuration.GetConnectionString("geo_query")!, b =>
{
    b.GeobufRouteHandlerOption.RouteHandlerBuilderAction = x => x.WithTags("geo_query");
    b.GeoJsonRouteHandlerOption.RouteHandlerBuilderAction = x => x.WithTags("geo_query");
    b.MVTRouteHandlerOption.RouteHandlerBuilderAction = x => x.WithTags("geo_query");
});

app.Run();