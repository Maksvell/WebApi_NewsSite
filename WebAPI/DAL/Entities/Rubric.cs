
namespace DAL.Entities;

public class Rubric : BaseEntity
{
    public string Name { get; set; } = null!;
    public List<News> News { get; } = [];

}
