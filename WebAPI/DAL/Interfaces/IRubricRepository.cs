using DAL.Entities;

namespace DAL.Interfaces;

public interface IRubricRepository : IRepository<Rubric>
{
    Task<Rubric> GetByName(string name);
}
