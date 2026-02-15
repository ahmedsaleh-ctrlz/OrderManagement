using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entites;

namespace OrderManagement.Infrastructure.Persistence.EntitesConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) 
        {
            builder.HasQueryFilter(options => !options.IsDeleted);
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(options => options.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(u => u.Email)
            .IsUnique();
            

        }
    }
}
