namespace Giserver.GeoQuery.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddGeoQuery(this IServiceCollection services) =>
        services.AddSingleton<IGeoQuery, Querys.GeoQuery>();
}