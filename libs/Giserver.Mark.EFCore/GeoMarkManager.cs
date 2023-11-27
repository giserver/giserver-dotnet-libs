namespace Giserver.Mark.EFCore;

internal class GeoMarkManager<T> : IGeoMarkerManager where T : DbContext
{
    private readonly T dbContext;

    public GeoMarkManager(T dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Layer> CreateLayerAsync(Layer layer)
    {
        await dbContext.AddAsync(layer);
        await dbContext.SaveChangesAsync();

        return layer;
    }

    public async Task<Marker?> CreateMarkerAsync(Marker marker)
    {
        // 需要判断LayerId是否存在
        if (!await dbContext.Set<Layer>().AnyAsync(x => x.Id == marker.LayerId))
            return null;

        await dbContext.AddAsync(marker);
        await dbContext.SaveChangesAsync();

        return marker;
    }

    public async Task<Layer> DeleteLayerAsync(string id)
    {
        var layer = new Layer() { Id = id };
        dbContext.Attach(layer);
        dbContext.Remove(layer);
        await dbContext.SaveChangesAsync();
        return layer;
    }

    public async Task<Marker> DeleteMarkerAsync(string id)
    {
        var marker = new Marker() { Id = id };
        dbContext.Attach(marker);
        dbContext.Remove(marker);
        await dbContext.SaveChangesAsync();
        return marker;
    }

    public async Task<IEnumerable<Layer>> GetAllLayerAsync(string? tenantId)
    {
        var entities = dbContext.Set<Layer>().AsNoTracking();
        entities = entities.Where(x => x.TenantId == tenantId);

        return await entities.ToListAsync();
    }

    public async Task<IEnumerable<Marker>> GetAllMarkerAsync(string? tenantId)
    {
        var entities = dbContext.Set<Marker>().AsNoTracking();
        entities = entities.Where(x => x.TenantId == tenantId);

        return await entities.ToListAsync();
    }

    public async Task<Layer> UpdateLayerAsync(Layer layer)
    {
        var entity = await dbContext.FindAsync<Layer>(layer.Id);
        if (entity != null)
        {
            entity.Name = layer.Name;
            await dbContext.SaveChangesAsync();
        }

        return layer;
    }

    public async Task<Marker> UpdateMarkerAsync(Marker marker)
    {
        var entity = await dbContext.FindAsync<Marker>(marker.Id);
        if (entity != null)
        {
            entity.Name = marker.Name;
            entity.Geom = marker.Geom;
            entity.Style = marker.Style;
            entity.Props = marker.Props;
            await dbContext.SaveChangesAsync();
        }

        return marker;
    }
}