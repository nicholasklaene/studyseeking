using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.EntityConfigurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts");
        
        builder.HasKey(p => p.Id);

        builder
            .HasOne(p => p.Category)
            .WithMany(c => c.Posts)
            .HasForeignKey(p => p.CategoryId);
        
        builder
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId);

        builder
            .Property(p => p.Id)
            .HasColumnName("id");
        builder
            .Property(p => p.CategoryId)
            .HasColumnName("category_id");
        builder
            .Property(p => p.UserId)
            .HasColumnName("user_id");
        builder
            .Property(p => p.Title)
            .IsRequired()
            .HasColumnName("title")
            .HasMaxLength(128);
        builder
            .Property(p => p.Preview)
            .IsRequired()
            .HasColumnName("preview")
            .HasMaxLength(256);
        builder
            .Property(p => p.IsActive)
            .IsRequired()
            .HasColumnName("is_active")
            .HasDefaultValue(true);
        builder
            .Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        builder
            .Property(p => p.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");
    }
}
