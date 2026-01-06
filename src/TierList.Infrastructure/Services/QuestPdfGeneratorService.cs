using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TierList.Application.Ports.Services;
using TierList.Domain.Entities;
using TierList.Domain.Enums;

namespace TierList.Infrastructure.Services;

public class QuestPdfGeneratorService : IPdfGeneratorService
{
    public Task<byte[]> GenerateTierListPdfAsync(UserTierList tierList, CancellationToken cancellationToken = default)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Size(PageSizes.A4);

                page.Header().Element(ComposeHeader);
                page.Content().Element(content => ComposeContent(content, tierList));
                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Generated on: ");
                    text.Span(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC"));
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }

    private void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().AlignCenter().Text("Company Logo Tier List").FontSize(24).Bold();
            column.Item().AlignCenter().Text("Personal Ranking").FontSize(14);
        });
    }

    private void ComposeContent(IContainer container, UserTierList tierList)
    {
        container.PaddingVertical(20).Column(column =>
        {
            var groupedItems = tierList.Items
                .GroupBy(item => item.Rank)
                .OrderBy(g => g.Key)
                .ToList();

            foreach (var group in groupedItems)
            {
                column.Item().PaddingVertical(10).Row(row =>
                {
                    row.RelativeItem(1).Background(GetTierColor(group.Key))
                        .Padding(10)
                        .Text(GetTierLabel(group.Key))
                        .FontSize(20)
                        .Bold()
                        .FontColor(Colors.White);

                    row.RelativeItem(4).Border(1).Padding(10).Column(tierColumn =>
                    {
                        foreach (var item in group)
                        {
                            tierColumn.Item().PaddingBottom(5).Text(item.CompanyLogo.CompanyName);
                        }
                    });
                });
            }
        });
    }

    private string GetTierLabel(TierRank rank)
    {
        return rank switch
        {
            TierRank.S => "S - Les chefs-d'œuvre du branding",
            TierRank.A => "A - Très bons logos",
            TierRank.B => "B - Ça passe",
            TierRank.C => "C - Médiocres",
            TierRank.D => "D - Les flops visuels",
            _ => rank.ToString()
        };
    }

    private string GetTierColor(TierRank rank)
    {
        return rank switch
        {
            TierRank.S => "#FF6B6B",
            TierRank.A => "#FFA500",
            TierRank.B => "#FFD700",
            TierRank.C => "#90EE90",
            TierRank.D => "#87CEEB",
            _ => "#808080"
        };
    }
}
