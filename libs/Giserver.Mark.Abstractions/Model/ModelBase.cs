namespace Giserver.Mark.Abstractions.Model;

public abstract class ModelBase
{
    [Key]
    public string Id { get; set; }

    public string? TenantId { get; set; }

    public string Name { get; set; }

    public long Date { get; set; }
}