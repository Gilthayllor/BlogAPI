using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAPI.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("NVARCHAR");

            builder.HasMany(x => x.Posts)
                .WithOne(x => x.Category)
                .HasForeignKey("CategoryId")
                .HasConstraintName("FK_Category_Post")
                .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}
