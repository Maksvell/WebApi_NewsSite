using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using BLL.Validators;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services;

public class RubricService(IUnitOfWork unit, IMapper mapper) : BaseSevice<RubricDTO>(unit, mapper), IRubricService
{
    protected private readonly RubricValidator _validator = new();
    public async Task<IEnumerable<RubricDTO>> GetAll()
    {
        var entities = await _unit.RubricRepository.GetAll();
        var collection = _mapper.Map<IEnumerable<Rubric>, IEnumerable<RubricDTO>>(entities);
        return collection;
    }

    public async Task<RubricDTO> GetById(int id)
    {
        var entity = await _unit.RubricRepository.GetById(id) ?? throw new NullReferenceException();
        var rubric = _mapper.Map<RubricDTO>(entity);
        return rubric;
    }

    public async Task Add(RubricDTO entity)
    {
        _validator.Validate(entity);

        var rubric = _mapper.Map<Rubric>(entity);
        await _unit.RubricRepository.Add(rubric);
        await _unit.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var rubric = await _unit.RubricRepository.GetById(id) ?? throw new NullReferenceException();
        await _unit.RubricRepository.Delete(rubric);
        await _unit.SaveChangesAsync();
    }

    public async Task Update(int id, RubricDTO entity)
    {
        _validator.Validate(entity);

        var rubric = await _unit.RubricRepository.GetById(id) ?? throw new NullReferenceException();

        rubric.Name = entity.Name;

        await _unit.RubricRepository.Update(rubric);
        await _unit.SaveChangesAsync();
    }
}
