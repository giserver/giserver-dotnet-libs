namespace Giserver.GeoQuery.Extensions;

public abstract class GeoQueryRouteHandlerOption
{
    public bool Allowed { get; set; } = true;

    public Action<RouteHandlerBuilder>? RouteHandlerBuilderAction { get; set; } = null;

    public abstract string GetRoutePatter(string prefix, bool isConnectionStringTemplate);

    public abstract Delegate GetRouteDelegate(bool isConnectionStringTemplate);
}

public class MVTRouteHandlerOption : GeoQueryRouteHandlerOption
{
    public override Delegate GetRouteDelegate(bool isConnectionStringTemplate)
    {
        return isConnectionStringTemplate ?
            async ([FromServices] IOptions<GeoQueryOptions> options, [FromServices] IGeoQuery geoQuery, string database, string table, string geomColumn, int z, int x, int y, string? schema, string? columns, string? filter, bool? centroid) =>
            {
                var bytes = await geoQuery.GetMvtBufferAsync(options.Value.ConnectionString.Format(database), table, geomColumn, z, x, y, schema ?? "public", columns.SpliteByComma(), filter, centroid ?? false);
                return Results.Bytes(bytes, "application/x-protobuf");
            }
        :
            async ([FromServices] IOptions<GeoQueryOptions> options, [FromServices] IGeoQuery geoQuery, string table, string geomColumn, int z, int x, int y, string? schema, string? columns, string? filter, bool? centroid) =>
            {
                var bytes = await geoQuery.GetMvtBufferAsync(options.Value.ConnectionString, table, geomColumn, z, x, y, schema ?? "public", columns.SpliteByComma(), filter, centroid ?? false);
                return Results.Bytes(bytes, "application/x-protobuf");
            };
    }

    public override string GetRoutePatter(string prefix, bool isConnectionStringTemplate)
    {
        return prefix + $"/mvt/{(isConnectionStringTemplate ? "{database}/" : "")}{{table}}/{{geomColumn}}/{{z:int}}/{{x:int}}/{{y:int}}.pbf";
    }
}

public class GeobufRouteHandlerOption : GeoQueryRouteHandlerOption
{
    public override Delegate GetRouteDelegate(bool isConnectionStringTemplate)
    {
        return isConnectionStringTemplate ?
            async ([FromServices] IOptions<GeoQueryOptions> options, [FromServices] IGeoQuery geoQuery, string database, string table, string geomColumn, string? schema, string? columns, string? filter, bool? centroid) =>
            {
                var bytes = await geoQuery.GetGeoBufferAsync(options.Value.ConnectionString.Format(database), table, geomColumn, schema ?? "public", columns.SpliteByComma(), filter, centroid ?? false);
                return Results.Bytes(bytes, "application/x-protobuf");
            }
        :
            async ([FromServices] IOptions<GeoQueryOptions> options, [FromServices] IGeoQuery geoQuery, string table, string geomColumn, string? schema, string? columns, string? filter, bool? centroid) =>
            {
                var bytes = await geoQuery.GetGeoBufferAsync(options.Value.ConnectionString, table, geomColumn, schema ?? "public", columns.SpliteByComma(), filter, centroid ?? false);
                return Results.Bytes(bytes, "application/x-protobuf");
            };
    }

    public override string GetRoutePatter(string prefix, bool isConnectionStringTemplate)
    {
        return prefix + $"/geobuf/{(isConnectionStringTemplate ? "{database}/" : "")}{{table}}/{{geomColumn}}.pbf";
    }
}

public class GeoJsonRouteHandlerOption : GeoQueryRouteHandlerOption
{
    public override Delegate GetRouteDelegate(bool isConnectionStringTemplate)
    {
        return isConnectionStringTemplate ?
            async ([FromServices] IOptions<GeoQueryOptions> options, [FromServices] IGeoQuery geoQuery, string database, string table, string geomColumn, string? schema, string? idColumn, string? columns, string? filter, bool? centroid) =>
            {
                var geoJson = await geoQuery.GetGeoJsonAsync(options.Value.ConnectionString.Format(database), table, geomColumn, schema ?? "public", idColumn, columns.SpliteByComma(), filter, centroid ?? false);
                return Results.Text(geoJson, "application/json");
            }
        :
            async ([FromServices] IOptions<GeoQueryOptions> options, [FromServices] IGeoQuery geoQuery, string table, string geomColumn, string? schema, string? idColumn, string? columns, string? filter, bool? centroid) =>
            {
                var geoJson = await geoQuery.GetGeoJsonAsync(options.Value.ConnectionString, table, geomColumn, schema ?? "public", idColumn, columns.SpliteByComma(), filter, centroid ?? false);
                return Results.Text(geoJson, "application/json");
            };
    }

    public override string GetRoutePatter(string prefix, bool isConnectionStringTemplate)
    {
        return prefix + $"/geojson/{(isConnectionStringTemplate ? "{database}/" : "")}{{table}}/{{geomColumn}}";
    }
}

public class GeoQueryRouteOptions
{
    public string Prefix = "geo";
    public bool IsConnectionStringTemplate = true;

    public MVTRouteHandlerOption MVTRouteHandlerOption { get; } = new();

    public GeobufRouteHandlerOption GeobufRouteHandlerOption { get; } = new();

    public GeoJsonRouteHandlerOption GeoJsonRouteHandlerOption { get; } = new();
}

public static class WebAppExtensions
{
    public static WebApplication UseGeoQuery(this WebApplication app,
        Action<GeoQueryRouteOptions>? optionsBuilder = null)
    {
        var options = new GeoQueryRouteOptions();
        optionsBuilder?.Invoke(options);

        var properties = options.GetType().GetProperties()
            .Where(x => x.PropertyType.IsAssignableTo(typeof(GeoQueryRouteHandlerOption)))
            .Select(x => (GeoQueryRouteHandlerOption)x.GetValue(options)!);

        foreach (var routeHandlerOption in properties)
        {
            if (routeHandlerOption.Allowed)
            {
                var routeHandler = app.MapGet(
                    routeHandlerOption.GetRoutePatter(options.Prefix, options.IsConnectionStringTemplate),
                    routeHandlerOption.GetRouteDelegate(options.IsConnectionStringTemplate));

                routeHandlerOption.RouteHandlerBuilderAction?.Invoke(routeHandler);
            }
        }

        return app;
    }
}