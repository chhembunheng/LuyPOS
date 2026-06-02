using LuyPOS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuyPOS.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
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

        entity.HasIndex(product => product.Sku)
            .IsUnique()
            .HasFilter("deleted_at IS NULL")
            .HasDatabaseName("idx_products_sku_active");

        entity.HasIndex(product => product.Name).HasDatabaseName("idx_products_name");
        entity.HasIndex(product => product.IsActive).HasDatabaseName("idx_products_is_active");
        entity.HasIndex(product => product.DeletedAt).HasDatabaseName("idx_products_deleted_at");
    }
}
