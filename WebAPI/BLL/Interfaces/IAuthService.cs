using DAL.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace BLL.Interfaces;

public interface IAuthService
{
    Task<Author> GetEntity(LoginRequest author);
    string CreateToken(Author author);
}
