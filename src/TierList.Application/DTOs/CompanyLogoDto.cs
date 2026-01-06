namespace TierList.Application.DTOs;

public record CompanyLogoDto(
    Guid Id,
    string CompanyName,
    string Domain,
    string LogoUrl
);
