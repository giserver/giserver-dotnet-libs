namespace Giserver.GeoQuery.Querys;

public interface IGeoQuery
{
    /// <summary>
    /// get mapbox-vector-tiles buffer
    /// 获取mapbox矢量切片buffer
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="table">name of table</param>
    /// <param name="geomColumn">name of geometry column</param>
    /// <param name="z">{z}</param>
    /// <param name="x">{x}</param>
    /// <param name="y">{y}</param>
    /// <param name="schema">default: public</param>
    /// <param name="columns">eg: id,name</param>
    /// <param name="filter">eg: name like 'scc%'</param>
    /// <param name="centroid">default false</param>
    /// <returns></returns>
    Task<byte[]> GetMvtBufferAsync(string connectionString, string table, string geomColumn, int z, int x, int y,
        string schema = "public",
        string[]? columns = null,
        string? filter = null,
        bool centroid = false);

    /// <summary>
    /// get GeoBuf
    /// 获取GeoBuf格式的数据
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="table">name of table</param>
    /// <param name="geomColumn">name of geometry column</param>
    /// <param name="schema">default: public</param>
    /// <param name="columns">eg: id,name</param>
    /// <param name="filter">eg: name like 'scc%'</param>
    /// <param name="centroid">default false</param>
    /// <returns></returns>
    Task<byte[]> GetGeoBufferAsync(string connectionString, string table, string geomColumn,
        string schema = "public",
        string[]? columns = null,
        string? filter = null,
        bool centroid = false);

    /// <summary>
    /// get GeoJson
    /// 获取GeoJson
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="table">name of table</param>
    /// <param name="geomColumn">name of geometry column</param>
    /// <param name="schema">default: public</param>
    /// <param name="idColumn">name of id column</param>
    /// <param name="columns">eg: id,name</param>
    /// <param name="filter">eg: name like 'scc%'</param>
    /// <param name="centroid">default false</param>
    /// <returns></returns>
    Task<string> GetGeoJsonAsync(string connectionString, string table, string geomColumn,
        string schema = "public",
        string? idColumn = null,
        string[]? columns = null,
        string? filter = null,
        bool centroid = false);
}