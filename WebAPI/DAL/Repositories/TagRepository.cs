using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class TagRepository(NewsSiteContext context) : BaseRepository<Tag>(context), ITagRepository
{
    public async Task<Tag> GetByName(string name)
    {
        return await _context.Set<Tag>().FirstOrDefaultAsync(tag => tag.Name.Equals(name)) ?? throw new NullReferenceException();
    }
}
