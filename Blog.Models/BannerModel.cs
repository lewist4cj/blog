using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models;

[Table("banner_models")]
public partial class BannerModel
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [Column("cover")]
    public string? Cover { get; set; }

    [Column("href")]
    public string? Href { get; set; }
}
