using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.Persistence.EntitesConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> options) 
        {
            options.HasQueryFilter(options => !options.IsDeleted);
            options.HasKey(u => u.Id);
            options.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
            options.Property(options => options.Email)
                .IsRequired()
                .HasMaxLength(100);
            options.Property(options => options.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

        }
    }
}
