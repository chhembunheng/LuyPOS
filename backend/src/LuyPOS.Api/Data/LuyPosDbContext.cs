using System.Text;
using LuyPOS.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LuyPOS.Api.Data;

public sealed class LuyPosDbContext(DbContextOptions<LuyPosDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserTranslation> UserTranslations => Set<UserTranslation>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RoleTranslation> RoleTranslations => Set<RoleTranslation>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<PermissionTranslation> PermissionTranslations => Set<PermissionTranslation>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<MenuTranslation> MenuTranslations => Set<MenuTranslation>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<MenuPermission> MenuPermissions => Set<MenuPermission>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<UserOtp> UserOtps => Set<UserOtp>();
    public DbSet<UserForgotPassword> UserForgotPasswords => Set<UserForgotPassword>();
    public DbSet<LoginHistory> LoginHistories => Set<LoginHistory>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<AuditLogDetail> AuditLogDetails => Set<AuditLogDetail>();
    public DbSet<UserActivity> UserActivities => Set<UserActivity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureProducts(modelBuilder);
        ConfigureUsers(modelBuilder);
        ConfigureAccessControl(modelBuilder);
        ConfigureSessionsAndSecurity(modelBuilder);
        ConfigureAudit(modelBuilder);

        UseSnakeCaseColumnNames(modelBuilder);
    }

    public override int SaveChanges()
    {
        StampTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        StampTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private static void ConfigureProducts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(product => product.Id);
            entity.Property(product => product.Sku).HasMaxLength(64).IsRequired();
            entity.Property(product => product.Name).HasMaxLength(160).IsRequired();
            entity.Property(product => product.Description).HasMaxLength(500);
            entity.Property(product => product.UnitPrice).HasPrecision(18, 2);
            entity.Property(product => product.CostPrice).HasPrecision(18, 2);
            entity.Property(product => product.QuantityOnHand).HasDefaultValue(0);
            entity.Property(product => product.ReorderLevel).HasDefaultValue(0);
            entity.Property(product => product.IsActive).HasDefaultValue(true);
            entity.Property(product => product.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(product => product.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(product => product.Sku).IsUnique().HasFilter("deleted_at IS NULL").HasDatabaseName("idx_products_sku_active");
            entity.HasIndex(product => product.Name).HasDatabaseName("idx_products_name");
            entity.HasIndex(product => product.IsActive).HasDatabaseName("idx_products_is_active");
            entity.HasIndex(product => product.DeletedAt).HasDatabaseName("idx_products_deleted_at");
        });
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Password).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Enabled).HasDefaultValue(true);
            entity.Property(x => x.EmailVerified).HasDefaultValue(false);
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(x => x.Email).IsUnique().HasDatabaseName("idx_users_email");
            entity.HasIndex(x => x.CreatedAt).HasDatabaseName("idx_users_created_at");
            entity.HasIndex(x => x.Enabled).HasDatabaseName("idx_user_enabled");
            entity.HasIndex(x => x.EmailVerified).HasDatabaseName("idx_user_email_verified");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("user_profiles");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Phone).HasMaxLength(20);
            entity.Property(x => x.Address).HasMaxLength(255);
            entity.Property(x => x.Avatar).HasMaxLength(255);
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.UserId).IsUnique().HasDatabaseName("uk_user_profile");
            entity.HasOne(x => x.User).WithOne(x => x.Profile).HasForeignKey<UserProfile>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserTranslation>(entity =>
        {
            entity.ToTable("user_translations");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FirstName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(255).IsRequired();
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.UserId).HasDatabaseName("idx_user_translations_user_id");
            entity.HasIndex(x => x.LanguageId).HasDatabaseName("idx_user_translations_language_id");
            entity.HasIndex(x => x.DeletedAt).HasDatabaseName("idx_user_translations_deleted_at");
            entity.HasIndex(x => new { x.UserId, x.LanguageId }).IsUnique().HasFilter("deleted_at IS NULL").HasDatabaseName("idx_user_translations_user_lang_active");
            entity.HasOne(x => x.User).WithMany(x => x.Translations).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Language).WithMany(x => x.UserTranslations).HasForeignKey(x => x.LanguageId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("languages", table => table.HasCheckConstraint("check_direction", "direction IN ('ltr','rtl')"));
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(255).IsRequired();
            entity.Property(x => x.NativeName).HasMaxLength(255);
            entity.Property(x => x.Code).HasMaxLength(10).IsRequired();
            entity.Property(x => x.Direction).HasMaxLength(3).HasDefaultValue("ltr").IsRequired();
            entity.Property(x => x.FlagImage).HasMaxLength(500);
            entity.Property(x => x.IsActive).HasDefaultValue(true);
            entity.Property(x => x.IsDefault).HasDefaultValue(false);
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.Code).IsUnique().HasDatabaseName("unique_language_code");
            entity.HasIndex(x => x.DeletedAt).HasDatabaseName("idx_languages_deleted_at");
        });
    }

    private static void ConfigureAccessControl(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(x => x.Id);
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.DeletedAt).HasDatabaseName("idx_roles_deleted_at");
            entity.HasIndex(x => x.CreatedAt).HasDatabaseName("idx_roles_created_at");
        });

        modelBuilder.Entity<RoleTranslation>(entity =>
        {
            entity.ToTable("role_translations");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(255).IsRequired();
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.RoleId).HasDatabaseName("idx_role_translations_role_id");
            entity.HasIndex(x => x.LanguageId).HasDatabaseName("idx_role_translations_language_id");
            entity.HasIndex(x => x.DeletedAt).HasDatabaseName("idx_role_translations_deleted_at");
            entity.HasIndex(x => new { x.RoleId, x.LanguageId }).IsUnique().HasFilter("deleted_at IS NULL").HasDatabaseName("idx_role_translations_role_lang_active");
            entity.HasOne(x => x.Role).WithMany(x => x.Translations).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Language).WithMany(x => x.RoleTranslations).HasForeignKey(x => x.LanguageId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("permissions");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Slug).HasMaxLength(255).IsRequired();
            entity.Property(x => x.IsMenu).HasDefaultValue(false);
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.Slug).IsUnique().HasDatabaseName("slug");
            entity.HasIndex(x => x.IsMenu).HasDatabaseName("idx_permissions_is_menu");
            entity.HasIndex(x => x.DeletedAt).HasDatabaseName("idx_permissions_deleted_at");
        });

        modelBuilder.Entity<PermissionTranslation>(entity =>
        {
            entity.ToTable("permission_translations");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(255);
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.PermissionId).HasDatabaseName("idx_permission_translations_permission_id");
            entity.HasIndex(x => x.LanguageId).HasDatabaseName("idx_permission_translations_language_id");
            entity.HasIndex(x => x.DeletedAt).HasDatabaseName("idx_permission_translations_deleted_at");
            entity.HasIndex(x => new { x.PermissionId, x.LanguageId }).IsUnique().HasFilter("deleted_at IS NULL").HasDatabaseName("idx_permission_translations_perm_lang_active");
            entity.HasOne(x => x.Permission).WithMany(x => x.Translations).HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Language).WithMany(x => x.PermissionTranslations).HasForeignKey(x => x.LanguageId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("menus");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Icon).HasMaxLength(255);
            entity.Property(x => x.Slug).HasMaxLength(255).IsRequired();
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.ParentId).HasDatabaseName("idx_menus_parent_id");
            entity.HasIndex(x => x.Slug).IsUnique().HasDatabaseName("slug_menus");
            entity.HasIndex(x => x.DeletedAt).HasDatabaseName("idx_menus_deleted_at");
            entity.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MenuTranslation>(entity =>
        {
            entity.ToTable("menu_translations");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(255);
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.MenuId).HasDatabaseName("idx_menu_translations_menu_id");
            entity.HasIndex(x => x.LanguageId).HasDatabaseName("idx_menu_translations_language_id");
            entity.HasIndex(x => x.DeletedAt).HasDatabaseName("idx_menu_translations_deleted_at");
            entity.HasIndex(x => new { x.MenuId, x.LanguageId }).IsUnique().HasFilter("deleted_at IS NULL").HasDatabaseName("idx_menu_translations_menu_lang_active");
            entity.HasOne(x => x.Menu).WithMany(x => x.Translations).HasForeignKey(x => x.MenuId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Language).WithMany(x => x.MenuTranslations).HasForeignKey(x => x.LanguageId).OnDelete(DeleteBehavior.Restrict);
        });

        ConfigureJoinEntity<UserRole>(modelBuilder, "user_roles", "idx_user_roles_user_role_active");
        ConfigureJoinEntity<UserPermission>(modelBuilder, "user_permissions", "idx_user_permissions_user_perm_active");
        ConfigureJoinEntity<RolePermission>(modelBuilder, "role_permissions", "idx_role_permissions_role_perm_active");
        ConfigureJoinEntity<MenuPermission>(modelBuilder, "menu_permissions", "idx_menu_permissions_menu_perm_active");

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasOne(x => x.User).WithMany(x => x.UserPermissions).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Permission).WithMany(x => x.UserPermissions).HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasOne(x => x.Role).WithMany(x => x.RolePermissions).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Permission).WithMany(x => x.RolePermissions).HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<MenuPermission>(entity =>
        {
            entity.HasOne(x => x.Menu).WithMany(x => x.MenuPermissions).HasForeignKey(x => x.MenuId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Permission).WithMany(x => x.MenuPermissions).HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureSessionsAndSecurity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.ToTable("user_sessions");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.SessionId).HasMaxLength(255).IsRequired();
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.UserId).HasDatabaseName("idx_user_sessions_user_id");
            entity.HasIndex(x => x.SessionId).IsUnique().HasDatabaseName("session_id");
            entity.HasIndex(x => x.DeletedAt).HasDatabaseName("idx_user_sessions_deleted_at");
            entity.HasIndex(x => x.CreatedAt).HasDatabaseName("idx_user_sessions_created_at");
            entity.HasOne(x => x.User).WithMany(x => x.Sessions).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Token).HasMaxLength(255).IsRequired();
            entity.Property(x => x.ExpiryDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(x => x.Revoked).HasDefaultValue(false);
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.UserId).HasDatabaseName("idx_refresh_tokens_user_id");
            entity.HasIndex(x => x.Token).IsUnique().HasDatabaseName("token");
            entity.HasIndex(x => x.DeletedAt).HasDatabaseName("idx_refresh_tokens_deleted_at");
            entity.HasIndex(x => x.CreatedAt).HasDatabaseName("idx_refresh_tokens_created_at");
            entity.HasIndex(x => x.ExpiryDate).HasDatabaseName("idx_refresh_token_expiry");
            entity.HasIndex(x => x.Revoked).HasDatabaseName("idx_refresh_token_revoked");
            entity.HasOne(x => x.User).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserOtp>(entity =>
        {
            entity.ToTable("user_otp");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.OtpCode).HasMaxLength(10).IsRequired();
            entity.Property(x => x.Purpose).HasMaxLength(50);
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.UserId).HasDatabaseName("idx_otp_user_id");
            entity.HasIndex(x => x.OtpCode).HasDatabaseName("idx_otp_code");
            entity.HasIndex(x => x.ExpiresAt).HasDatabaseName("idx_otp_expires_at");
            entity.HasIndex(x => x.Purpose).HasDatabaseName("idx_otp_purpose");
            entity.HasOne(x => x.User).WithMany(x => x.Otps).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserForgotPassword>(entity =>
        {
            entity.ToTable("user_forgot_password");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Token).HasMaxLength(255).IsRequired();
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex(x => x.Token).IsUnique().HasDatabaseName("token_user_forgot_password");
            entity.HasIndex(x => x.UserId).HasDatabaseName("idx_forgot_password_user_id");
            entity.HasIndex(x => x.ExpiresAt).HasDatabaseName("idx_forgot_password_expires_at");
            entity.HasOne(x => x.User).WithMany(x => x.ForgotPasswordTokens).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<LoginHistory>(entity =>
        {
            entity.ToTable("login_histories");
            entity.HasKey(x => x.Id);
            ConfigureClientInfo(entity);
            ConfigureSoftDeleteTimestamps(entity);
            entity.Property(x => x.LoginAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(x => x.UserId).HasDatabaseName("idx_login_histories_user_id");
            entity.HasIndex(x => x.LoginAt).HasDatabaseName("idx_login_histories_login_at");
            entity.HasIndex(x => x.LogoutAt).HasDatabaseName("idx_login_histories_logout_at");
            entity.HasIndex(x => x.IpAddress).HasDatabaseName("idx_login_histories_ip_address");
            entity.HasIndex(x => x.DeletedAt).HasDatabaseName("idx_login_histories_deleted_at");
            entity.HasOne(x => x.User).WithMany(x => x.LoginHistories).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserActivity>(entity =>
        {
            entity.ToTable("user_activities");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.SessionId).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(20).IsRequired();
            entity.Property(x => x.IpAddress).HasMaxLength(45);
            entity.Property(x => x.Location).HasMaxLength(255);
            entity.Property(x => x.Country).HasMaxLength(100);
            entity.Property(x => x.City).HasMaxLength(100);
            entity.Property(x => x.DeviceInfo).HasMaxLength(500);
            entity.Property(x => x.BrowserInfo).HasMaxLength(255);
            entity.Property(x => x.CurrentPage).HasMaxLength(255);
            entity.HasIndex(x => x.UserId).HasDatabaseName("idx_user_id");
            entity.HasIndex(x => x.Status).HasDatabaseName("idx_status");
            entity.HasIndex(x => x.LastActivityAt).HasDatabaseName("idx_last_activity");
            entity.HasIndex(x => x.SessionId).HasDatabaseName("idx_session_id");
            entity.HasOne(x => x.User).WithMany(x => x.Activities).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureAudit(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("audit_logs");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Action).HasMaxLength(255).IsRequired();
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            ConfigureClientInfo(entity);
            entity.HasIndex(x => x.UserId).HasDatabaseName("idx_audit_logs_user_id");
            entity.HasIndex(x => x.Action).HasDatabaseName("idx_audit_logs_action");
            entity.HasIndex(x => x.CreatedAt).HasDatabaseName("idx_audit_logs_created_at");
            entity.HasIndex(x => x.IpAddress).HasDatabaseName("idx_audit_logs_ip_address");
            entity.HasOne(x => x.User).WithMany(x => x.AuditLogs).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AuditLogDetail>(entity =>
        {
            entity.ToTable("audit_log_details");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.KeyName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(x => x.AuditLogId).HasDatabaseName("idx_audit_log_details_audit_log_id");
            entity.HasIndex(x => x.KeyName).HasDatabaseName("idx_audit_log_details_key_name");
            entity.HasOne(x => x.AuditLog).WithMany(x => x.Details).HasForeignKey(x => x.AuditLogId).OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureJoinEntity<TEntity>(ModelBuilder modelBuilder, string tableName, string activeUniqueIndexName)
        where TEntity : class
    {
        modelBuilder.Entity<TEntity>(entity =>
        {
            entity.ToTable(tableName);
            entity.HasKey("Id");
            ConfigureSoftDeleteTimestamps(entity);
            entity.HasIndex("DeletedAt").HasDatabaseName($"idx_{tableName}_deleted_at");
        });

        if (typeof(TEntity) == typeof(UserRole))
        {
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasIndex(x => x.UserId).HasDatabaseName("idx_user_roles_user_id");
                entity.HasIndex(x => x.RoleId).HasDatabaseName("idx_user_roles_role_id");
                entity.HasIndex(x => new { x.UserId, x.RoleId }).IsUnique().HasFilter("deleted_at IS NULL").HasDatabaseName(activeUniqueIndexName);
            });
        }
        else if (typeof(TEntity) == typeof(UserPermission))
        {
            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.HasIndex(x => x.UserId).HasDatabaseName("idx_user_permissions_user_id");
                entity.HasIndex(x => x.PermissionId).HasDatabaseName("idx_user_permissions_permission_id");
                entity.HasIndex(x => new { x.UserId, x.PermissionId }).IsUnique().HasFilter("deleted_at IS NULL").HasDatabaseName(activeUniqueIndexName);
            });
        }
        else if (typeof(TEntity) == typeof(RolePermission))
        {
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasIndex(x => x.RoleId).HasDatabaseName("idx_role_permissions_role_id");
                entity.HasIndex(x => x.PermissionId).HasDatabaseName("idx_role_permissions_permission_id");
                entity.HasIndex(x => new { x.RoleId, x.PermissionId }).IsUnique().HasFilter("deleted_at IS NULL").HasDatabaseName(activeUniqueIndexName);
            });
        }
        else if (typeof(TEntity) == typeof(MenuPermission))
        {
            modelBuilder.Entity<MenuPermission>(entity =>
            {
                entity.HasIndex(x => x.MenuId).HasDatabaseName("idx_menu_permissions_menu_id");
                entity.HasIndex(x => x.PermissionId).HasDatabaseName("idx_menu_permissions_permission_id");
                entity.HasIndex(x => new { x.MenuId, x.PermissionId }).IsUnique().HasFilter("deleted_at IS NULL").HasDatabaseName(activeUniqueIndexName);
            });
        }
    }

    private static void ConfigureSoftDeleteTimestamps<TEntity>(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TEntity> entity)
        where TEntity : class
    {
        entity.Property<DateTime?>("CreatedAt").HasDefaultValueSql("CURRENT_TIMESTAMP");
        entity.Property<DateTime?>("UpdatedAt").HasDefaultValueSql("CURRENT_TIMESTAMP");
        entity.Property<DateTime?>("DeletedAt");
    }

    private static void ConfigureClientInfo<TEntity>(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TEntity> entity)
        where TEntity : class
    {
        entity.Property<string?>("IpAddress").HasMaxLength(45);
        entity.Property<string?>("UserAgent").HasMaxLength(255);
        entity.Property<string?>("Browser").HasMaxLength(255);
        entity.Property<string?>("Platform").HasMaxLength(255);
    }

    private void StampTimestamps()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                SetDateTimeProperty(entry, "CreatedAt", now);
                SetDateTimeProperty(entry, "UpdatedAt", now);
            }
            else if (entry.State == EntityState.Modified)
            {
                SetDateTimeProperty(entry, "UpdatedAt", now);
            }
        }
    }

    private static void SetDateTimeProperty(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, string propertyName, DateTime value)
    {
        var property = entry.Metadata.FindProperty(propertyName);
        if (property is null || property.ClrType != typeof(DateTime?))
        {
            return;
        }

        var currentValue = entry.Property(propertyName).CurrentValue;
        if (currentValue is null || propertyName == "UpdatedAt")
        {
            entry.Property(propertyName).CurrentValue = value;
        }
    }

    private static void UseSnakeCaseColumnNames(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.Name));
            }
        }
    }

    private static string ToSnakeCase(string value)
    {
        var builder = new StringBuilder(value.Length + 8);

        for (var i = 0; i < value.Length; i++)
        {
            var current = value[i];
            if (char.IsUpper(current) && i > 0)
            {
                builder.Append('_');
            }

            builder.Append(char.ToLowerInvariant(current));
        }

        return builder.ToString();
    }
}
