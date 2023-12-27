namespace DAL.Entities;

public class Author : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;  
    public string Password { get; set; } = null!;
    public List<News> News { get; } = [];

}
