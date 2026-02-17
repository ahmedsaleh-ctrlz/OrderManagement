using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entites;

namespace OrderManagement.Infrastructure.Persistence.EntitesConfiguration
{
    public class WarehouseUserEntityConfiguration : IEntityTypeConfiguration<WarehouseUser>
    {
        public void Configure(EntityTypeBuilder<WarehouseUser> builder)
        {
            builder.HasKey(wu => wu.Id);

            builder.HasOne(wu => wu.User)
                   .WithMany()
                   .HasForeignKey(wu => wu.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(wu => wu.Warehouse)
                   .WithMany()
                   .HasForeignKey(wu => wu.WarehouseId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(wu => wu.UserId)
                   .IsUnique(); 
        }

    }
}
