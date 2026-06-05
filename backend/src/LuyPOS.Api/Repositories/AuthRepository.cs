using LuyPOS.Api.Data;
using LuyPOS.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LuyPOS.Api.Repositories;

public sealed class AuthRepository(LuyPosDbContext dbContext) : IAuthRepository
{
    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.AnyAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.AnyAsync(user => user.Name == username, cancellationToken);
    }

    public async Task<User?> GetByUsernameOrEmailAsync(
        string usernameOrEmail,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(
            candidate => candidate.Name == usernameOrEmail || candidate.Email == usernameOrEmail,
            cancellationToken);
    }

    public async Task<IReadOnlyCollection<string>> GetRoleNamesAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.UserRoles
            .AsNoTracking()
            .Where(userRole => userRole.UserId == userId && userRole.DeletedAt == null)
            .SelectMany(userRole => userRole.Role.Translations
                .Where(translation => translation.DeletedAt == null)
                .Select(translation => translation.Name))
            .Distinct()
            .OrderBy(role => role)
            .ToListAsync(cancellationToken);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }

    public async Task AddLoginHistoryAsync(LoginHistory loginHistory, CancellationToken cancellationToken = default)
    {
        await dbContext.LoginHistories.AddAsync(loginHistory, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
