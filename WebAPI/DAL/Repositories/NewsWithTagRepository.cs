using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class NewsWithTagRepository(NewsSiteContext context) : BaseRepository<NewsWithTag>(context), INewsWithTagRepository
{
    public IEnumerable<NewsWithTag> GetByNewsId(int id)
    {
        return ApplyFilter(_context.Set<NewsWithTag>().Where(x => x.NewsId.Equals(id))).ToList();
    }
    public IEnumerable<NewsWithTag> GetByTagId(int id)
    {
        return ApplyFilter(_context.Set<NewsWithTag>().Where(x => x.TagId.Equals(id))).ToList();
    }
}
