using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Mappers;

public class NewsMapper(IUnitOfWork unit) : INewsMapper
{
    private readonly IUnitOfWork _unit = unit;

    public async Task<NewsDTO> MapNewsToDTO(News news)
    {
        var authorName = await _unit.AuthorRepository.GetById(news.AuthorId);
        var rubricName = await _unit.RubricRepository.GetById(news.RubricId);

        return new NewsDTO()
        {
            Id = news.Id,
            Title = news.Title,
            Body = news.Body,
            Tags = MapTagsToDTO(news.Tags),
            Date = news.Date,
            AuthorName = authorName.Name,
            RubricName = rubricName.Name
        };
    }

    public async Task<News> MapNewsFromDTO(NewsDTO news)
    {
        var authorId = await _unit.AuthorRepository.GetByName(news.AuthorName);
        var rubricId = await _unit.RubricRepository.GetByName(news.RubricName);
        var tags = await CreateMapTagsFromDTO(news.Tags);
        return new News()
        {
            Id = news.Id,
            Title = news.Title,
            Body = news.Body,
            Tags = tags,
            Date = news.Date,
            AuthorId = authorId.Id,
            RubricId = rubricId.Id
        };
    }

    public async Task<IEnumerable<NewsDTO>> MapNewsToDTO(IEnumerable<News> list)
    {
        List<NewsDTO> news = [];
        foreach (var item in list)
        {
            var newsItem = await MapNewsToDTO(item);
            news.Add(newsItem);
        }
        return news;
    }

    public async Task<IEnumerable<News>> MapNewsFromDTO(IEnumerable<NewsDTO> list)
    {
        List<News> news = [];
        foreach (var item in list)
        {
            var newsItem = await MapNewsFromDTO(item);
            news.Add(newsItem);
        }
        return news;
    }

    public async Task<List<Tag>> CreateMapTagsFromDTO(List<string> tagNames)
    {
        var tags = new List<Tag>();
        foreach (var tag in tagNames)
        {
            var tg = await _unit.TagRepository.GetByName(tag) ?? throw new NullReferenceException();
            tags.Add(tg);
        }
        return tags;
    }

    public async Task<List<Tag>> UpdateMapTagsFromDTO(int id, List<string> tagNames)
    {
        var newsWithTags = _unit.NewsWithTagRepository.GetByNewsId(id);
        foreach (var nwt in newsWithTags)
        {
            await _unit.NewsWithTagRepository.Delete(nwt);
        }

        await _unit.SaveChangesAsync();

        return await CreateMapTagsFromDTO(tagNames);
    }

    public List<string> MapTagsToDTO(List<Tag> tags)
    {
        var tagNames = new List<string>();
        foreach (var tag in tags)
        {
            tagNames.Add(tag.Name);
        }
        return tagNames;
    }


}
