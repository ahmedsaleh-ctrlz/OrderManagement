using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using OrderManagement.Domain.Entites;

namespace OrderManagement.Infrastructure.Persistence.EntitesConfiguration
{
    public class WarehouseEntityConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasQueryFilter(options => !options.IsDeleted);
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(w => w.Location)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
