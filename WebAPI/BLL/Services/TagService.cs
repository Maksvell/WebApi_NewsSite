using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using BLL.Validators;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services;

public class TagService(IUnitOfWork unit, IMapper mapper) : BaseSevice<TagDTO>(unit, mapper), ITagService
{
    protected private readonly TagValidator _validator = new();
    public async Task<IEnumerable<TagDTO>> GetAll()
    {
        var entities = await _unit.TagRepository.GetAll();
        var collection = _mapper.Map<IEnumerable<Tag>, IEnumerable<TagDTO>>(entities);
        return collection;
    }

    public async Task<TagDTO> GetById(int id)
    {
        var entity = await _unit.TagRepository.GetById(id) ?? throw new NullReferenceException();
        var tag = _mapper.Map<TagDTO>(entity);
        return tag;
    }

    public async Task Add(TagDTO entity)
    {
        _validator.Validate(entity);

        var tag = _mapper.Map<Tag>(entity);
        await _unit.TagRepository.Add(tag);
        await _unit.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var tag = await _unit.TagRepository.GetById(id) ?? throw new NullReferenceException();
        await _unit.TagRepository.Delete(tag);
        await _unit.SaveChangesAsync();
    }

    public async Task Update(int id, TagDTO entity)
    {
        _validator.Validate(entity);

        var tag = await _unit.TagRepository.GetById(id) ?? throw new NullReferenceException();

        tag.Name = entity.Name;

        await _unit.TagRepository.Update(tag);
        await _unit.SaveChangesAsync();
    }
}
