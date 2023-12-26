using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class AuthController : Controller
{
    private protected readonly IAuthService _auth;
    private protected readonly IAuthorService _author;

    public AuthController(IAuthService auth, IAuthorService author) {  _auth = auth; _author = author; }

    [HttpPost("api/register")]
    public async Task<IActionResult> RegisterAuthor([FromBody] AuthorDTO author)
    {
        try
        {
            await _author.Add(author);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("api/login")]
    public async Task<IActionResult> LoginAuthor([FromBody] LoginRequest author)
    {
        try
        {
            var entity = _auth.GetEntity(author);
            string token = _auth.CreateToken(await entity);

            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
