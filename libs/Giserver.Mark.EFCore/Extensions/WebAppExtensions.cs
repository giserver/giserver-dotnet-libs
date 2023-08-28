namespace Giserver.Mark.EFCore.Extensions;

public class GeoMarkOptions
{
    public string Prefix { get; set; } = "geo";

    public Action<RouteHandlerBuilder>? RouteHandlerBuilderAction { get; set; }
}

public static class WebAppExtensions
{
    public static WebApplication UseGeoMarker(this WebApplication app, Action<GeoMarkOptions>? optionsBuilder = null)
    {
        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<GeoMarkDbContext>();
        dbContext.Database.EnsureCreated();

        var options = new GeoMarkOptions();
        optionsBuilder?.Invoke(options);

        var handlerBuilders = new[] {
        app.MapGet($"{options.Prefix}/layers", async (IGeoMarkerManager geoMarkerManager, string? tenantId) =>
        {
            var layers = await geoMarkerManager.GetAllLayerAsync(tenantId);
            return Results.Ok(layers);
        }),

        app.MapPost($"{options.Prefix}/layers", async (IGeoMarkerManager geoMarkerManager, Layer layer) =>
        {
            await geoMarkerManager.CreateLayerAsync(layer);
            return Results.Ok();
        }),

        app.MapPut($"{options.Prefix}/layers", async (IGeoMarkerManager geoMarkerManager, Layer layer) =>
        {
            await geoMarkerManager.UpdateLayerAsync(layer);
            return Results.Ok();
        }),

        app.MapDelete($"{options.Prefix}/layers/{{id}}", async (IGeoMarkerManager geoMarkerManager, string id) =>
        {
            await geoMarkerManager.DeleteLayerAsync(id);
            return Results.Ok();
        }),

        app.MapGet($"{options.Prefix}/markers", async (IGeoMarkerManager geoMarkerManager, string? tenantId) =>
        {
            var markers = await geoMarkerManager.GetAllMarkerAsync(tenantId);
            var fc = new FeatureCollection();
            foreach (var marker in markers)
            {
                var attributes = new AttributesTable();
                foreach (var property in marker.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(Geometry))
                        continue;

                    var value = property.GetValue(marker);
                    attributes.Add(property.Name[0].ToString().ToLower() + property.Name[1..], value);
                }
                fc.Add(new Feature(marker.Geom, attributes));
            }
            return Results.Ok(fc);
        }),

        app.MapPost($"{options.Prefix}/markers", async (IGeoMarkerManager geoMarkerManager, Marker marker) =>
        {
            await geoMarkerManager.CreateMarkerAsync(marker);
            return Results.Ok();
        }),

        app.MapPut($"{options.Prefix}/markers", async (IGeoMarkerManager geoMarkerManager, Marker marker) =>
        {
            await geoMarkerManager.UpdateMarkerAsync(marker);
            return Results.Ok();
        }),

        app.MapDelete($"{options.Prefix}/markers/{{id}}", async (IGeoMarkerManager geoMarkerManager, string id) =>
        {
            await geoMarkerManager.DeleteMarkerAsync(id);
            return Results.Ok();
        })};

        if (options.RouteHandlerBuilderAction is not null)
        {
            foreach (var builder in handlerBuilders)
            {
                options.RouteHandlerBuilderAction(builder);
            }
        }

        return app;
    }
}