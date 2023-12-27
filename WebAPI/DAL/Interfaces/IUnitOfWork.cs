using DAL.Entities;
using DAL.Repositories;

namespace DAL.Interfaces;

public interface IUnitOfWork
{
    IAuthorRepository AuthorRepository { get; }
    INewsRepository NewsRepository { get; }
    ITagRepository TagRepository { get; }
    IRubricRepository RubricRepository { get; }
    INewsWithTagRepository NewsWithTagRepository { get; }
    Task SaveChangesAsync();
}
