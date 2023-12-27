using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/tags")]
public class TagController(ITagService service) : Controller
{
    private protected readonly ITagService _tagService = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tags = await _tagService.GetAll();
        return Ok(tags);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var tag = await _tagService.GetById(id);
            return Ok(tag);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddTag([FromBody] TagDTO tag)
    {
        try
        {
            await _tagService.Add(tag);
            return Created();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] TagDTO tag)
    {
        try
        {
            await _tagService.Update(id, tag);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        try
        {
            await _tagService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}