using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class NewsSiteContext(DbContextOptions<NewsSiteContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().
            HasMany(a => a.News)
            .WithOne(n => n.Author);

        modelBuilder.Entity<Rubric>().
            HasMany(a => a.News)
            .WithOne(n => n.Rubric);

        modelBuilder.Entity<Tag>()
            .HasMany(a => a.NewsWithTags)
            .WithOne(n => n.Tag)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<News>()
            .HasMany(a => a.NewsWithTags)
            .WithOne(n => n.News)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<News>()
            .HasMany(e => e.Tags)
            .WithMany(e => e.News)
            .UsingEntity<NewsWithTag>();

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Author> Authors { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Rubric> Rubrics { get; set; }
    public DbSet<NewsWithTag> NewsWithTags { get; set; }

}
