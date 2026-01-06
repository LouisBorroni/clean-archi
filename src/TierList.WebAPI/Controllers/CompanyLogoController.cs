using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TierList.Application.DTOs;
using TierList.Application.UseCases;

namespace TierList.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyLogoController : ControllerBase
{
    private readonly AddCompanyLogoUseCase _addLogoUseCase;
    private readonly GetAllLogosUseCase _getAllLogosUseCase;

    public CompanyLogoController(
        AddCompanyLogoUseCase addLogoUseCase,
        GetAllLogosUseCase getAllLogosUseCase)
    {
        _addLogoUseCase = addLogoUseCase;
        _getAllLogosUseCase = getAllLogosUseCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CompanyLogoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddLogo([FromBody] AddCompanyLogoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var logo = await _addLogoUseCase.ExecuteAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetAllLogos), new { id = logo.Id }, logo);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Maximum"))
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<CompanyLogoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllLogos(CancellationToken cancellationToken)
    {
        var logos = await _getAllLogosUseCase.ExecuteAsync(cancellationToken);
        return Ok(logos);
    }
}
