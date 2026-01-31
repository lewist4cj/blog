using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog.Models;

[Table("banner_models")]
public class BannerModel : BaseModel
{
    [Column("cover")]
    public string? Cover { get; set; }
    [Column("href")]
    public string? Href { get; set; }
}