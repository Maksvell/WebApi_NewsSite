using DAL.Entities;

namespace BLL.DTOs;

public class NewsDTO : BaseDTO
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public DateTime Date { get; set; }
    public string AuthorName { get; set; } = null!;
    public string RubricName { get; set; } = null!;
    public List<string> Tags { get; set; } = [];
}