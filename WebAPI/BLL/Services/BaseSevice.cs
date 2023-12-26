using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using FluentValidation;

namespace BLL.Services;

public abstract class BaseSevice<T>(IUnitOfWork unit, IMapper mapper = null)
{
    private protected readonly IUnitOfWork _unit = unit;
    private protected readonly IMapper _mapper = mapper;
}
