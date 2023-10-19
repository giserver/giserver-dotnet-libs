using Giserver.Mark.EFCore.Extensions;
using Giserver.GeoQuery.Extensions;
using Giserver.NetTopologySuite.Serialize;
using Giserver.NetTopologySuite.Swagger.Swashbuckle;

var builder = WebApplication.CreateSlimBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var isDev = builder.Environment.IsDevelopment();

configuration.AddJsonFile(isDev ? "appsettings.dev.json" : "appsettings.json");

services.AddGeoMarker(configuration.GetConnectionString("geo_marker")!);
services.AddGeoQuery(options =>
{
    options.ConnectionString = configuration.GetConnectionString("geo_query")!;
});

services.AddCors();

services.AddEndpointsApiExplorer();

if (isDev)
{
    services.AddSwaggerGen(o =>
    {
        o.AddGeometry(GeoSerializeType.Geojson);
    });
}

var app = builder.Build();

if (isDev)
{
    app.UseCors(b =>
    {
        b.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod();
    });

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGeoMarker(options =>
{
    options.RouteHandlerBuilderAction = b =>
    {
        b.WithTags("geo_marker");
    };
});
app.UseGeoQuery(b =>
{
    b.GeobufRouteHandlerOption.RouteHandlerBuilderAction = x => x.WithTags("geo_query");
    b.GeoJsonRouteHandlerOption.RouteHandlerBuilderAction = x => x.WithTags("geo_query");
    b.MVTRouteHandlerOption.RouteHandlerBuilderAction = x => x.WithTags("geo_query");
});

app.Run();