using Microsoft.EntityFrameworkCore;

namespace LuyPOS.Api.Data;

public static class DevelopmentSchemaUpdater
{
    public static async Task EnsureMatchesModelAsync(LuyPosDbContext dbContext)
    {
        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE IF EXISTS refresh_tokens
                ALTER COLUMN token TYPE character varying(1000);

            ALTER TABLE IF EXISTS login_histories
                ALTER COLUMN user_agent TYPE character varying(1000);

            ALTER TABLE IF EXISTS audit_logs
                ALTER COLUMN user_agent TYPE character varying(1000);
            """);
    }
}
