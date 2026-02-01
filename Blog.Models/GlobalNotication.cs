using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models;

[Table("global_notications")]
public partial class GlobalNotication
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [Column("title")]
    [StringLength(32)]
    public string? Title { get; set; }

    [Column("icon")]
    [StringLength(256)]
    public string? Icon { get; set; }

    [Column("content")]
    [StringLength(64)]
    public string? Content { get; set; }

    [Column("href")]
    [StringLength(256)]
    public string? Href { get; set; }
}
