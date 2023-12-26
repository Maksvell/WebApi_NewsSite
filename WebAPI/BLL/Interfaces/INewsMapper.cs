using BLL.DTOs;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces;

public interface INewsMapper
{
    public Task<NewsDTO> MapNewsToDTO(News news);
    public Task<News> MapNewsFromDTO(NewsDTO news);
    public Task<IEnumerable<NewsDTO>> MapNewsToDTO(IEnumerable<News> list);
    public Task<IEnumerable<News>> MapNewsFromDTO(IEnumerable<NewsDTO> list);
    public Task<List<Tag>> CreateMapTagsFromDTO(List<string> tagNames);
    public Task<List<Tag>> UpdateMapTagsFromDTO(int id, List<string> tagNames);
    public List<string> MapTagsToDTO(List<Tag> tags);
}
