using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class News : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public DateTime Date { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
    public int RubricId { get; set; }
    public Rubric Rubric { get; set; } = null!;

    [ForeignKey("NewsId")]
    public List<NewsWithTag> NewsWithTags { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];

}
