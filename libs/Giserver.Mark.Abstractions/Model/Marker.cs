namespace Giserver.Mark.Abstractions.Model;

[Table("marker_item")]
public class Marker : ModelBase
{
    public Geometry Geom { get; set; }

    public string LayerId { get; set; }

    [Column(TypeName = "jsonb")]
    public JsonDocument Style { get; set; }
}