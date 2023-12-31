﻿namespace Giserver.GeoQuery.Querys;

internal class GeoQuery : IGeoQuery
{
    public async Task<byte[]> GetGeoBufferAsync(string connectionString, string table, string geomColumn,
        string schema = "public",
        string[]? columns = null,
        string? filter = null,
        bool centroid = false)
    {
        connectionString.ThrowIfNullOrWhiteSpace(nameof(connectionString));
        schema.ThrowIfNullOrWhiteSpace(nameof(schema));
        table.ThrowIfNullOrWhiteSpace(nameof(table));
        geomColumn.ThrowIfNullOrWhiteSpace(nameof(geomColumn));

        var tableString = GetPgSqlTableString(schema, table);
        var geomColumnString = GetPgSqlGeomColumnString(geomColumn, centroid);
        var columnsString = GetPgSqlColumnsString(columns);
        columnsString = columnsString != null ? $",{columnsString}" : "";
        var whereString = filter != null ? $"WHERE {filter}" : "";

        var sql = $"""
            SELECT ST_AsGeobuf(q, 'geom')
            FROM (SELECT ST_Transform({geomColumnString}, 4326) AS geom {columnsString} FROM {tableString} {whereString}) as q;
            """;

        return await QuerySingleValueAsync<byte[]>(connectionString, sql);
    }

    public async Task<string> GetGeoJsonAsync(string connectionString, string table, string geomColumn,
        string schema = "public",
        string? idColumn = null,
        string[]? columns = null,
        string? filter = null,
        bool centroid = false)
    {
        connectionString.ThrowIfNullOrWhiteSpace(nameof(connectionString));
        schema.ThrowIfNullOrWhiteSpace(nameof(schema));
        table.ThrowIfNullOrWhiteSpace(nameof(table));
        geomColumn.ThrowIfNullOrWhiteSpace(nameof(geomColumn));

        var tableString = GetPgSqlTableString(schema, table);
        var geomColumnString = GetPgSqlGeomColumnString(geomColumn, centroid);
        var idColumnString = idColumn != null ? $",{GetPgSqlColumnString(idColumn)} as id" : "";
        var columnsString = GetPgSqlColumnsString(columns) ?? "";

        var parameters = TranslateFilter(ref filter);
        var filterString = filter != null ? $"WHERE {filter}" : "";

        var sql = $"""
            SELECT row_to_json(fc)
            FROM (SELECT 'FeatureCollection' AS type,COALESCE (array_to_json(array_agg(f)),'[]'::json) AS features
                  FROM (SELECT 'Feature' AS type {idColumnString}, ST_AsGeoJSON({geomColumnString})::json AS geometry,
                               (SELECT row_to_json(t) FROM (SELECT {columnsString}) AS t) AS properties
                        FROM {tableString} {filterString} ) AS f
                       ) AS fc
            """;

        return await QuerySingleValueAsync<string>(connectionString, sql, parameters);
    }

    public async Task<byte[]> GetMvtBufferAsync(string connectionString, string table, string geomColumn, int z, int x, int y,
        string schema = "public",
        string[]? columns = null,
        string? filter = null,
        bool centroid = false)
    {
        connectionString.ThrowIfNullOrWhiteSpace(nameof(connectionString));
        schema.ThrowIfNullOrWhiteSpace(nameof(schema));
        table.ThrowIfNullOrWhiteSpace(nameof(table));
        geomColumn.ThrowIfNullOrWhiteSpace(nameof(geomColumn));

        var tableString = GetPgSqlTableString(schema, table);
        var geomColumnString = GetPgSqlGeomColumnString(geomColumn, centroid);
        var columnsString = GetPgSqlColumnsString(columns);
        columnsString = columnsString != null ? $",{columnsString}" : "";
        var filterString = filter != null ? $" AND {filter}" : "";

        var sql = $"""
            WITH mvt_geom as (
                 SELECT ST_AsMVTGeom(ST_Transform({geomColumnString}, 3857),ST_TileEnvelope({z}, {x}, {y})) as geom {columnsString}
                 FROM {tableString},(SELECT ST_SRID({geomColumnString}) AS srid FROM {tableString} LIMIT 1) a
                 WHERE ST_Intersects({geomColumnString},ST_Transform(ST_TileEnvelope({z}, {x}, {y}),srid)) {filterString})
            SELECT ST_AsMVT(mvt_geom.*, '{table}', 4096, 'geom') AS mvt from mvt_geom;
            """;

        return await QuerySingleValueAsync<byte[]>(connectionString, sql);
    }

    private static string GetPgSqlTableString(string schema, string table)
    {
        return $"{GetPgSqlColumnString(schema)}.{GetPgSqlColumnString(table)}";
    }

    private static string GetPgSqlGeomColumnString(string geomColumn, bool centroid)
    {
        var colString = GetPgSqlColumnString(geomColumn);
        return centroid ? $"ST_Centroid({colString})" : $"{colString}";
    }

    private static string? GetPgSqlColumnsString(string[]? columns)
    {
        if (columns == null || columns.Length == 0)
            return null;

        return string.Join(',', columns.Select(x => GetPgSqlColumnString(x)));
    }

    private static string GetPgSqlColumnString(string column)
    {
        return $"\"{column}\"";
    }

    private static Array? TranslateFilter(ref string? filter)
    {
        string[] filter_opration_keys = ["=", ">", "<", "<>", ">=", "<=", "LIKE"];

        if (filter is not null)
        {
            var ret = new List<NpgsqlParameter>();
            var ands = filter.ToUpper().Split(" AND ", StringSplitOptions.RemoveEmptyEntries);
            filter = "";

            for (var i = 0; i < ands.Length; i++)
            {
                var and = ands[i];
                var ors = and.Split(" OR ");

                for (var j = 0; j < ors.Length; j++)
                {
                    var or = ors[j];
                    foreach (var key in filter_opration_keys)
                    {
                        var indexofkey = or.IndexOf(key);
                        if (indexofkey == -1) continue;

                        var col = or[..indexofkey].Trim();
                        var value = or[(indexofkey + key.Length)..].Trim();

                        filter += $"{col} {key} @{col}";
                        
                        if(int.TryParse(value,out var intV))
                        {
                            ret.Add(new NpgsqlParameter($"@{col}", intV));
                        }

                        else if(double.TryParse(value,out var doubleV))
                        {
                            ret.Add(new NpgsqlParameter($"@{col}", doubleV));
                        }

                        else
                        {
                            ret.Add(new NpgsqlParameter($"@{col}", value));
                        }
                    }

                    if (j < ors.Length - 1)
                    {
                        filter += " OR ";
                    }
                }

                if (i < ands.Length - 1)
                {
                    filter += " AND ";
                }
            }

            if (ret.Count > 0)
                return ret.ToArray();
        }

        filter = null;
        return null;
    }

    private static async Task<T> QuerySingleValueAsync<T>(string connectionString, string sql, Array? parameters = null)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);

        if (parameters != null)
            cmd.Parameters.AddRange(parameters);

        await using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();

        return (T)reader[0];
    }
}