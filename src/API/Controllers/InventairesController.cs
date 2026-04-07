using Application.Features.Inventaires.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventairesController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventairesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export()
    {
        var data = await _mediator.Send(new GetInventairesExportQuery());
        return Ok(data);
    }
}