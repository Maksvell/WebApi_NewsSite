using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;

namespace DAL;

public class UnitOfWork(NewsSiteContext context) : IUnitOfWork
{
    private readonly NewsSiteContext _context = context;

    public IAuthorRepository AuthorRepository => new AuthorRepository(_context);
    public INewsRepository NewsRepository => new NewsRepository(_context);
    public ITagRepository TagRepository => new TagRepository(_context);
    public IRubricRepository RubricRepository => new RubricRepository(_context);
    public INewsWithTagRepository NewsWithTagRepository => new NewsWithTagRepository(_context);

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
