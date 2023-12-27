using DAL.Interfaces;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class NewsRepository(NewsSiteContext context) : BaseRepository<News>(context), INewsRepository
{
    public async Task<IEnumerable<News>> GetAllByAuthorId(int id)
    {
        return await _context.Set<News>()
            .Where(news => news.AuthorId == id)
            .ToListAsync();
    }

    public async Task<IEnumerable<News>> GetAllByRubricId(int id)
    {
        return await _context.Set<News>()
            .Where(news => news.RubricId == id)
            .ToListAsync();
    }

    public async Task<IEnumerable<News>> GetAllByTagId(int id)
    {
        return await _context.Set<News>()
            .Where(news => news.AuthorId == id)
            .ToListAsync();
    }
}
