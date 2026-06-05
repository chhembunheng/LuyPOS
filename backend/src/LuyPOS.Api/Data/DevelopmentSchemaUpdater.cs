using Microsoft.EntityFrameworkCore;

namespace LuyPOS.Api.Data;

public static class DevelopmentSchemaUpdater
{
    public static async Task EnsureMatchesModelAsync(LuyPosDbContext dbContext)
    {
        await dbContext.Database.ExecuteSqlRawAsync("""
            CREATE EXTENSION IF NOT EXISTS pgcrypto;

            ALTER TABLE IF EXISTS products
                ADD COLUMN IF NOT EXISTS guid uuid;

            UPDATE products
            SET guid = gen_random_uuid()
            WHERE guid IS NULL;

            ALTER TABLE IF EXISTS products
                ALTER COLUMN guid SET NOT NULL;

            CREATE UNIQUE INDEX IF NOT EXISTS idx_products_guid
                ON products (guid);

            ALTER TABLE IF EXISTS refresh_tokens
                ALTER COLUMN token TYPE character varying(1000);

            ALTER TABLE IF EXISTS login_histories
                ALTER COLUMN user_agent TYPE character varying(1000);

            ALTER TABLE IF EXISTS audit_logs
                ALTER COLUMN user_agent TYPE character varying(1000);
            """);
    }
}
