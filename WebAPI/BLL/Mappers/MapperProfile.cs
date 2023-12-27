using AutoMapper;
using DAL.Entities;
using BLL.DTOs;

namespace BLL.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AuthorDTO, Author>().ReverseMap();

        CreateMap<RubricDTO, Rubric>().ReverseMap();

        CreateMap<TagDTO, Tag>().ReverseMap();
    }
}
