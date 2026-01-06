namespace TierList.Application.DTOs;

public record AuthResponse(
    string Token,
    string Email,
    string Username,
    bool IsAdmin
);
