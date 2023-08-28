namespace Giserver.Mark.Abstractions;

public interface IGeoMarkerManager
{
    Task<IEnumerable<Layer>> GetAllLayerAsync(string? tenantId);

    Task<Layer> CreateLayerAsync(Layer layer);

    Task<Layer> DeleteLayerAsync(string id);

    Task<Layer> UpdateLayerAsync(Layer layer);

    Task<IEnumerable<Marker>> GetAllMarkerAsync(string? tenantId);

    Task<Marker?> CreateMarkerAsync(Marker marker);

    Task<Marker> DeleteMarkerAsync(string id);

    Task<Marker> UpdateMarkerAsync(Marker marker);
}