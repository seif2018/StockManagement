using Application.DTOs;
using Application.Features.Articles.Commands;
using Application.Features.Articles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ArticlesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetArticlesQuery()));

    [HttpPost]
    public async Task<IActionResult> Create(CreateArticleCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        
    }
    [HttpPut("{reference}")]
    public async Task<IActionResult> Update(string reference, UpdateArticleCommand command)
    {
        if (reference != command.Reference)
            return BadRequest("La référence de l'URL ne correspond pas à celle du corps.");

        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{reference}")]
    public async Task<IActionResult> Delete(string reference)
    {
        try
        {
            await _mediator.Send(new DeleteArticleCommand(reference));
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = "Article non trouvé." });
        }
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetArticlesWithPaginationQuery(page, pageSize));
        return Ok(result);
    }

    [HttpPost("approvisionner")]
    public async Task<IActionResult> Approvisionner(ApprovisionnerCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("inventaire")]
    public async Task<IActionResult> Inventaire(InventaireCommand command)
    {
 try
    {
        await _mediator.Send(command);
        return Ok();
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { error = ex.Message });
    }
    }
}
