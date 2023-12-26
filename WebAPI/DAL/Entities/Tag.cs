using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = null!;

    [ForeignKey("TagId")]
    public List<NewsWithTag> NewsWithTags { get; } = [];
    public List<News> News { get; } = [];
}
