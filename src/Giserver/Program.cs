using Giserver.Mark.EFCore.Extensions;
using Giserver.GeoQuery.Extensions;
using Giserver.NetTopologySuite.Serialize;
using Giserver.NetTopologySuite.Swagger.Swashbuckle;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Logging.ClearProviders();
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

if (isDev)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(o =>
    {
        o.AddGeometry(GeoSerializeType.Geojson);
    });
}

var app = builder.Build();

if (isDev)
{
    app.UseCors(c =>
    {
        c.SetIsOriginAllowed(_ => true)
         .AllowAnyHeader()
         .AllowAnyMethod();
    });

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors(c =>
    {
        c.SetIsOriginAllowed(x => x.StartsWith("http://localhost"))
         .AllowAnyHeader()
         .AllowAnyMethod();
    });
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