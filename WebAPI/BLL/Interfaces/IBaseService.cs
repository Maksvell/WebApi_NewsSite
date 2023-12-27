using BLL.DTOs;

namespace BLL.Interfaces;

public interface IBaseService<T> where T : BaseDTO
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task Add(T entity);
    Task Update(int id, T entity);
    Task Delete(int id);
}
