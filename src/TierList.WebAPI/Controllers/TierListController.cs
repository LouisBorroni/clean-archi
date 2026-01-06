using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TierList.Application.DTOs;
using TierList.Application.UseCases;

namespace TierList.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TierListController : ControllerBase
{
    private readonly UpdateUserTierListUseCase _updateTierListUseCase;
    private readonly ExportTierListToPdfUseCase _exportPdfUseCase;

    public TierListController(
        UpdateUserTierListUseCase updateTierListUseCase,
        ExportTierListToPdfUseCase exportPdfUseCase)
    {
        _updateTierListUseCase = updateTierListUseCase;
        _exportPdfUseCase = exportPdfUseCase;
    }

    [HttpPut]
    [ProducesResponseType(typeof(TierListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateTierList([FromBody] UpdateTierListRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserId();
            var tierList = await _updateTierListUseCase.ExecuteAsync(userId, request, cancellationToken);
            return Ok(tierList);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("export")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ExportToPdf(CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserId();
            var pdfUrl = await _exportPdfUseCase.ExecuteAsync(userId, cancellationToken);
            return Ok(new { pdfUrl });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID in token");
        }

        return userId;
    }
}
