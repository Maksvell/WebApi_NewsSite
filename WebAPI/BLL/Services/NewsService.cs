using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using BLL.Mappers;
using BLL.Validators;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services;

public class NewsService(IUnitOfWork unit) : BaseSevice<NewsDTO>(unit), INewsService
{
    protected private readonly NewsValidator _validator = new();
    protected private readonly new NewsMapper _mapper = new(unit);
    public async Task<IEnumerable<NewsDTO>> GetAll()
    {
        var entities = await _unit.NewsRepository.GetAll();
        var collection = await _mapper.MapNewsToDTO(entities);
        return collection;
    }

    public async Task<NewsDTO> GetById(int id)
    {
        var entity = await _unit.NewsRepository.GetById(id) ?? throw new NullReferenceException();
        var news = await _mapper.MapNewsToDTO(entity);
        return news;
    }

    public async Task Add(NewsDTO entity)
    {
        _validator.Validate(entity);

        var news = await _mapper.MapNewsFromDTO(entity);
        await _unit.NewsRepository.Add(news);
        await _unit.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var news = await _unit.NewsRepository.GetById(id) ?? throw new NullReferenceException();
        await _unit.NewsRepository.Delete(news);
        await _unit.SaveChangesAsync();
    }

    public async Task Update(int id, NewsDTO entity)
    {
        _validator.Validate(entity);

        var news = await _unit.NewsRepository.GetById(id) ?? throw new NullReferenceException();
        var author = await _unit.AuthorRepository.GetByName(entity.AuthorName);
        var rubric = await _unit.RubricRepository.GetByName(entity.RubricName);
        var tags = await _mapper.UpdateMapTagsFromDTO(id, entity.Tags);

        news.Title = entity.Title;
        news.Body = entity.Body;
        news.Date = entity.Date;
        news.AuthorId = author.Id;
        news.Author = author;
        news.RubricId = rubric.Id;
        news.Rubric = rubric;
        news.Tags = tags;

        await _unit.NewsRepository.Update(news);
        await _unit.SaveChangesAsync();
    }

    public async Task<IEnumerable<NewsDTO>> GetAllByRubricId(int id)
    {
        var entities = await _unit.NewsRepository.GetAllByRubricId(id);
        var collection = await _mapper.MapNewsToDTO(entities);
        return collection;
    }

    public async Task<IEnumerable<NewsDTO>> GetAllByTagId(int id)
    {
        var entities = await _unit.NewsRepository.GetAllByTagId(id);
        var collection = await _mapper.MapNewsToDTO(entities);
        return collection;
    }

    public async Task<IEnumerable<NewsDTO>> GetAllByAuthorId(int id)
    {
        var entities = await _unit.NewsRepository.GetAllByAuthorId(id);
        var collection = await _mapper.MapNewsToDTO(entities);
        return collection;
    }
}
