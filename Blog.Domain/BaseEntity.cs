using SqlSugar;

namespace Blog.Domain;

public class BaseEntity
{
    [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
    public ulong Id { get; set; }
}
