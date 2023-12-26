using BLL.DTOs;

namespace BLL.Interfaces;

public interface IAuthorService : IBaseService<AuthorDTO>
{
    public Task<bool> CheckIfRegistered(string email);
}
