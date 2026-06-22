using SqlSugar;

namespace Blog.Domain;

public class BaseEntity
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public ulong Id { get; set; }
}
