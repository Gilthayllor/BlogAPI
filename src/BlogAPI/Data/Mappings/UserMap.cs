using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace BlogAPI.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("NVARCHAR");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("NVARCHAR");

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("NVARCHAR");

            builder.Property(x => x.About)
                .IsRequired(false)
                .HasMaxLength(255)
                .HasColumnType("NVARCHAR");
        }
    }
}
