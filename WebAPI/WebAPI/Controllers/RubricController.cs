using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/rubrics")]
public class RubricController(IRubricService rubricService) : Controller
{
    private protected readonly IRubricService _rubricService = rubricService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rubrics = await _rubricService.GetAll();
        return Ok(rubrics);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var rubric = await _rubricService.GetById(id);
            return Ok(rubric);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddRubric([FromBody] RubricDTO rubric)
    {
        try
        {
            await _rubricService.Add(rubric);
            return Created();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRubric(int id, [FromBody] RubricDTO rubric)
    {
        try
        {
            await _rubricService.Update(id, rubric);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRubric(int id)
    {
        try
        {
            await _rubricService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}