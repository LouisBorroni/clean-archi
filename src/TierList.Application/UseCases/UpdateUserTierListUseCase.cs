using TierList.Application.DTOs;
using TierList.Application.Ports.Repositories;
using TierList.Domain.Entities;

namespace TierList.Application.UseCases;

public class UpdateUserTierListUseCase
{
    private readonly IUserTierListRepository _tierListRepository;
    private readonly ICompanyLogoRepository _logoRepository;

    public UpdateUserTierListUseCase(
        IUserTierListRepository tierListRepository,
        ICompanyLogoRepository logoRepository)
    {
        _tierListRepository = tierListRepository;
        _logoRepository = logoRepository;
    }

    public async Task<TierListDto> ExecuteAsync(Guid userId, UpdateTierListRequest request, CancellationToken cancellationToken = default)
    {
        var tierList = await _tierListRepository.GetByUserIdWithItemsAsync(userId, cancellationToken);

        if (tierList == null)
        {
            tierList = new UserTierList(userId);
            tierList = await _tierListRepository.CreateAsync(tierList, cancellationToken);
        }

        var allLogos = await _logoRepository.GetAllAsync(cancellationToken);
        var logoDict = allLogos.ToDictionary(l => l.Id);

        foreach (var item in tierList.Items.ToList())
        {
            tierList.RemoveItem(item);
        }

        foreach (var (logoId, rank) in request.Items)
        {
            if (!logoDict.ContainsKey(logoId))
            {
                throw new InvalidOperationException($"Logo with ID {logoId} does not exist");
            }

            var newItem = new TierListItem(tierList.Id, logoId, rank);
            tierList.AddItem(newItem);
        }

        await _tierListRepository.UpdateAsync(tierList, cancellationToken);

        var updatedTierList = await _tierListRepository.GetByUserIdWithItemsAsync(userId, cancellationToken);

        return MapToDto(updatedTierList!, logoDict);
    }

    private static TierListDto MapToDto(UserTierList tierList, Dictionary<Guid, CompanyLogo> logoDict)
    {
        var items = tierList.Items.Select(item =>
        {
            var logo = logoDict[item.CompanyLogoId];
            return new TierListItemResponse(
                item.Id,
                new CompanyLogoDto(logo.Id, logo.CompanyName, logo.Domain, logo.LogoUrl),
                item.Rank
            );
        }).ToList();

        return new TierListDto(tierList.Id, tierList.UserId, tierList.IsPaid, tierList.PdfUrl, items);
    }
}
