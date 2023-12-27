using BLL.DTOs;

namespace BLL.Interfaces;

public interface INewsService : IBaseService<NewsDTO>
{
    Task<IEnumerable<NewsDTO>> GetAllByRubricId(int id);
    Task<IEnumerable<NewsDTO>> GetAllByTagId(int id);
    Task<IEnumerable<NewsDTO>> GetAllByAuthorId(int id);
}
