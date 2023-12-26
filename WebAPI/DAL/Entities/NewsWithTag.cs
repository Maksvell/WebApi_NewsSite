namespace DAL.Entities;

public class NewsWithTag : BaseEntity
{ 
    public int NewsId { get; set; } 
    public int TagId { get; set; } 
    public Tag Tag { get; set; } = null!;
    public News News { get; set; } = null!; 
}
