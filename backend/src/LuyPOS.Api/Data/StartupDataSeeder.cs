using LuyPOS.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LuyPOS.Api.Data;

public static class StartupDataSeeder
{
    private const string DefaultAdminRoleName = "Admin";
    private const string DefaultAdminUsername = "administrator";
    private const string DefaultAdminEmail = "administrator@luypos.local";
    private const string DefaultAdminPassword = "Administrator@123";

    public static async Task SeedAsync(
        LuyPosDbContext dbContext,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var language = await EnsureEnglishLanguageAsync(dbContext, cancellationToken);
        var adminRole = await EnsureAdminRoleAsync(dbContext, language.Id, cancellationToken);
        var administrator = await EnsureAdministratorUserAsync(dbContext, configuration, cancellationToken);

        await EnsureUserRoleAsync(dbContext, administrator.Id, adminRole.Id, cancellationToken);
    }

    private static async Task<Language> EnsureEnglishLanguageAsync(
        LuyPosDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var language = await dbContext.Languages
            .FirstOrDefaultAsync(candidate => candidate.Code == "en", cancellationToken);

        if (language is not null)
        {
            return language;
        }

        language = new Language
        {
            Name = "English",
            NativeName = "English",
            Code = "en",
            Direction = "ltr",
            IsActive = true,
            IsDefault = !await dbContext.Languages.AnyAsync(cancellationToken)
        };

        await dbContext.Languages.AddAsync(language, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return language;
    }

    private static async Task<Role> EnsureAdminRoleAsync(
        LuyPosDbContext dbContext,
        long languageId,
        CancellationToken cancellationToken)
    {
        var roleTranslation = await dbContext.RoleTranslations
            .Include(candidate => candidate.Role)
            .FirstOrDefaultAsync(
                candidate => candidate.Name == DefaultAdminRoleName
                    && candidate.LanguageId == languageId
                    && candidate.DeletedAt == null
                    && candidate.Role.DeletedAt == null,
                cancellationToken);

        if (roleTranslation is not null)
        {
            return roleTranslation.Role;
        }

        var role = new Role();
        await dbContext.Roles.AddAsync(role, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await dbContext.RoleTranslations.AddAsync(new RoleTranslation
        {
            RoleId = role.Id,
            LanguageId = languageId,
            Name = DefaultAdminRoleName
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return role;
    }

    private static async Task<User> EnsureAdministratorUserAsync(
        LuyPosDbContext dbContext,
        IConfiguration configuration,
        CancellationToken cancellationToken)
    {
        var username = configuration.GetValue<string>("Seed:AdminUser:Username") ?? DefaultAdminUsername;
        var email = configuration.GetValue<string>("Seed:AdminUser:Email") ?? DefaultAdminEmail;
        var password = configuration.GetValue<string>("Seed:AdminUser:Password") ?? DefaultAdminPassword;

        var user = await dbContext.Users.FirstOrDefaultAsync(
            candidate => candidate.Email == email || candidate.Name == username,
            cancellationToken);

        if (user is not null)
        {
            user.Enabled = true;
            user.EmailVerified = true;
            user.EmailVerifiedAt ??= DateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
            return user;
        }

        user = new User
        {
            Name = username,
            Email = email,
            Enabled = true,
            EmailVerified = true,
            EmailVerifiedAt = DateTime.UtcNow
        };
        user.Password = new PasswordHasher<User>().HashPassword(user, password);

        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }

    private static async Task EnsureUserRoleAsync(
        LuyPosDbContext dbContext,
        long userId,
        long roleId,
        CancellationToken cancellationToken)
    {
        var alreadyAssigned = await dbContext.UserRoles.AnyAsync(
            candidate => candidate.UserId == userId
                && candidate.RoleId == roleId
                && candidate.DeletedAt == null,
            cancellationToken);

        if (alreadyAssigned)
        {
            return;
        }

        await dbContext.UserRoles.AddAsync(new UserRole
        {
            UserId = userId,
            RoleId = roleId
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
