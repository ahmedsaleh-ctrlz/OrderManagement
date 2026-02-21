using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using OrderManagement.Domain.Entites;

namespace OrderManagement.Infrastructure.Persistence.EntitesConfiguration
{
    public class ProductStockEntityConfiguration : IEntityTypeConfiguration<ProductStock>
    {
        public void Configure(EntityTypeBuilder<ProductStock> builder)
        {
            
            builder.HasKey(ps => ps.Id);

            builder.HasIndex(ps => new { ps.ProductId, ps.WarehouseId })
                .IsUnique();

            builder.HasOne(ps => ps.Product)
            .WithMany(p=>p.ProductStocks)
            .HasForeignKey(ps => ps.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ps=> ps.Warehouse)
                .WithMany()
                .HasForeignKey(ps => ps.WarehouseId);

            builder.Property(ps => ps.Quantity)
                .IsRequired();
        }

    }
}
