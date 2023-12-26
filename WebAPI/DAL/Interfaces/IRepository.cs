using DAL.Entities;

namespace DAL.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}
