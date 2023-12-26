using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class BaseRepository<T>(NewsSiteContext context) : IRepository<T> where T : BaseEntity
{
    private protected readonly NewsSiteContext _context = context;

    public async Task<T> GetById(int id)
    {
        return await _context.Set<T>().FindAsync(id) ?? throw new NullReferenceException();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task Add(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _context.Set<T>().Remove(entity!);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    protected IQueryable<T> ApplyFilter(IQueryable<T> query)
    {
        return query;
    }
}
