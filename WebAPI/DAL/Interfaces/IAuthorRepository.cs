using DAL.Entities;

namespace DAL.Interfaces;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author> GetByName(string name);
    Task<Author> GetByEmailAndPassword(string email, string password);
}
