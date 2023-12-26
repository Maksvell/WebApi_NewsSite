using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class RubricRepository(NewsSiteContext context) : BaseRepository<Rubric>(context), IRubricRepository
{
    public async Task<Rubric> GetByName(string name)
    {
        return await _context.Set<Rubric>().FirstOrDefaultAsync(rubric => rubric.Name.Equals(name)) ?? throw new NullReferenceException();
    }
}
