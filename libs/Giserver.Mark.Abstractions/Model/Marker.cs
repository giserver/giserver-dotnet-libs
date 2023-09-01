namespace Giserver.Mark.Abstractions.Model;

[Table("marker_item")]
public class Marker : ModelBase
{
    public Geometry Geom { get; set; }

    public string LayerId { get; set; }

    /// <summary>
    /// 样式存储
    /// </summary>
    [Column(TypeName = "jsonb")]
    public JsonDocument Style { get; set; }

    /// <summary>
    /// 其他属性
    /// </summary>
    [Column(TypeName = "jsonb")]
    public JsonDocument? Props { get; set; }
}