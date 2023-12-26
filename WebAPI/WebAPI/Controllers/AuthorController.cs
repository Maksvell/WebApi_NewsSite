using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/authors")]
public class AuthorController(IAuthorService authorService) : Controller
{
    private protected readonly IAuthorService _authorService = authorService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var authors = await _authorService.GetAll();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var author = await _authorService.GetById(id);
            return Ok(author);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        try
        {
            await _authorService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorDTO author)
    {
        try
        {
            await _authorService.Update(id, author);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
