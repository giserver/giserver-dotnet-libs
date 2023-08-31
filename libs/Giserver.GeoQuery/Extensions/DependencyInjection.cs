namespace Giserver.GeoQuery.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddGeoQuery(this IServiceCollection services, Action<GeoQueryOptions> optionsBuilder)
    {
        services.Configure(optionsBuilder);
        services.AddSingleton<IGeoQuery, Querys.GeoQuery>();
        return services;
    }
}