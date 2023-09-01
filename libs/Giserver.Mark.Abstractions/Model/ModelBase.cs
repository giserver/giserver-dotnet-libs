namespace Giserver.Mark.Abstractions.Model;

public abstract class ModelBase
{
    [Key]
    public string Id { get; set; }

    /// <summary>
    /// 租户id
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public long Date { get; set; }
}