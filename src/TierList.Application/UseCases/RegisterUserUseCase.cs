using TierList.Application.DTOs;
using TierList.Application.Ports.Repositories;
using TierList.Application.Ports.Services;
using TierList.Domain.Entities;

namespace TierList.Application.UseCases;

public class RegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterUserUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> ExecuteAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var existingUsername = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (existingUsername != null)
        {
            throw new InvalidOperationException("Username already taken");
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = new User(request.Email, passwordHash, request.Username);

        await _userRepository.CreateAsync(user, cancellationToken);

        var token = _jwtTokenService.GenerateToken(user.Id, user.Email, user.Username, user.IsAdmin);

        return new AuthResponse(token, user.Email, user.Username, user.IsAdmin);
    }
}
