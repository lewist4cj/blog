using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain;

public class BaseEntity
{
    [Key]
    [Column("id")]
    public ulong Id { get;}
}