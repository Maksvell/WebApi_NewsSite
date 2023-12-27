using DAL.Entities;

namespace DAL.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
    Task<Tag> GetByName(string name);
}
