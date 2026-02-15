using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using OrderManagement.Domain.Entites;

namespace OrderManagement.Infrastructure.Persistence.EntitesConfiguration
{
    public class ProductEntityConfiguration :IEntityTypeConfiguration<Product>  
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasQueryFilter(options => !options.IsDeleted);
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.SKU)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(p => p.SKU)
                .IsUnique();

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.RowVersion)
                .IsRowVersion();
                
        }

    }
}
