using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Username);

        builder.HasIndex(u => u.Email)
            .IsUnique();
        
        builder
            .Property(u => u.Username)
            .HasColumnName("username")
            .HasMaxLength(50);

        builder
            .Property(u => u.Email)
            .IsRequired()
            .HasColumnName("email")
            .HasMaxLength(50);
    }
}