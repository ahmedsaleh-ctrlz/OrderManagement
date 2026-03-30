using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entites;


namespace OrderManagement.Infrastructure.Persistence.EntitesConfiguration
{
    public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TokenHash)
                .IsRequired()
                .HasMaxLength(200);

           
            builder.HasOne(x => x.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            
            builder.Property(x => x.ExpiryDate)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // Index مهم للأداء
            builder.HasIndex(x => x.TokenHash)
                .IsUnique();
        }
    }
    
}
