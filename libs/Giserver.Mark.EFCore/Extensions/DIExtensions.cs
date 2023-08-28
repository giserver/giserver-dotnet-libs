namespace Giserver.Mark.EFCore.Extensions;

public static class DIExtensions
{
    public static void AddGeoMarker(this IServiceCollection services, string postgresqlConnectionString, Action<DbContextOptionsBuilder>? optionsAction = null)
    {
#if NET6_0
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new GeoJsonConverterFactory());
        });
#elif NET7_0_OR_GREATER
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new GeoJsonConverterFactory());
        });
#endif
        services.AddDbContext<GeoMarkDbContext>(options =>
        {
            optionsAction?.Invoke(options);
            options.UseNpgsql(postgresqlConnectionString, o => o.UseNetTopologySuite());
        });

        services.AddScoped<IGeoMarkerManager, GeoMarkManager<GeoMarkDbContext>>();
    }
}