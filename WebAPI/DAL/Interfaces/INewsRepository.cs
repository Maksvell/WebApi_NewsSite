using DAL.Entities;

namespace DAL.Interfaces;

public interface INewsRepository : IRepository<News>
{
    Task<IEnumerable<News>> GetAllByRubricId(int id);
    Task<IEnumerable<News>> GetAllByTagId(int id);
    Task<IEnumerable<News>> GetAllByAuthorId(int id);
}
