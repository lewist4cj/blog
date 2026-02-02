using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain;

[Table("banner_models")]
public partial class BannerModel:BaseEntity
{
    // [Key]
    // [Column("id")]
    // public ulong Id { get;}

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [Column("cover")]
    public string? Cover { get; set; }

    [Column("href")]
    public string? Href { get; set; }
}
