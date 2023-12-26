using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.Repositories;

public class AuthorRepository(NewsSiteContext context) : BaseRepository<Author>(context), IAuthorRepository
{
    public async Task<Author> GetByEmailAndPassword(string email, string password)
    {
        return await _context.Set<Author>().FirstOrDefaultAsync(author => author.Email.Equals(email) && author.Password.Equals(password)) ?? throw new NullReferenceException();
    }

    public async Task<Author> GetByName(string name)
    {
        return await _context.Set<Author>().FirstOrDefaultAsync(author => author.Name.Equals(name)) ?? throw new NullReferenceException();
    }
}
