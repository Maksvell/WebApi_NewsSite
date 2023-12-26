using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/news")]
public class NewsController(INewsService newsService) : Controller
{
    private protected readonly INewsService _newsService = newsService;
   
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var collection = await _newsService.GetAll();
        return Ok(collection);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var news = await _newsService.GetById(id);
            return Ok(news);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/by-author/{id}")]
    public async Task<IActionResult> GetAllByAuthor(int id)
    {
        try
        {
            var news = await _newsService.GetAllByAuthorId(id);
            return Ok(news);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/by-rubric/{id}")]
    public async Task<IActionResult> GetAllByRubric(int id)
    {
        try
        {
            var news = await _newsService.GetAllByRubricId(id);
            return Ok(news);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/by-tag/{id}")]
    public async Task<IActionResult> GetAllByTag(int id)
    {
        try
        {
            var news = await _newsService.GetAllByTagId(id);
            return Ok(news);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddNews([FromBody] NewsDTO news)
    {
        try
        {
            await _newsService.Add(news);
            return Created($"{Uri.UriSchemeNews}", news);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNews(int id, [FromBody] NewsDTO news)
    {
        try
        {
            await _newsService.Update(id, news);
            return Created($"{Uri.UriSchemeNews}", news);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNews(int id)
    {
        try
        {
            await _newsService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
