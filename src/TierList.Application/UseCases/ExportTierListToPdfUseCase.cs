using TierList.Application.Ports.Repositories;
using TierList.Application.Ports.Services;

namespace TierList.Application.UseCases;

public class ExportTierListToPdfUseCase
{
    private readonly IUserTierListRepository _tierListRepository;
    private readonly IPdfGeneratorService _pdfGeneratorService;
    private readonly IStorageService _storageService;

    public ExportTierListToPdfUseCase(
        IUserTierListRepository tierListRepository,
        IPdfGeneratorService pdfGeneratorService,
        IStorageService storageService)
    {
        _tierListRepository = tierListRepository;
        _pdfGeneratorService = pdfGeneratorService;
        _storageService = storageService;
    }

    public async Task<string> ExecuteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tierList = await _tierListRepository.GetByUserIdWithItemsAsync(userId, cancellationToken);
        if (tierList == null)
        {
            throw new InvalidOperationException("User tier list not found");
        }

        if (!tierList.IsPaid)
        {
            throw new InvalidOperationException("Payment required to export tier list");
        }

        var pdfBytes = await _pdfGeneratorService.GenerateTierListPdfAsync(tierList, cancellationToken);

        var fileName = $"tierlist-{userId}-{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
        var pdfUrl = await _storageService.UploadPdfAsync(pdfBytes, fileName, cancellationToken);

        tierList.SetPdfUrl(pdfUrl);
        await _tierListRepository.UpdateAsync(tierList, cancellationToken);

        return pdfUrl;
    }
}
