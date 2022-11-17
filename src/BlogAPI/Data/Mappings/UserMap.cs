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
                .HasMaxLength(60)
                .HasColumnType("NVARCHAR");

            builder.Property(x => x.About)
                .IsRequired(false)
                .HasMaxLength(100)
                .HasColumnType("NVARCHAR");

            builder.Property(x => x.Image)
                .IsRequired(false)
                .HasMaxLength(255)
                .HasColumnType("NVARCHAR");

            builder.HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<Dictionary<string, string>>(
                    "UserRole",
                    user => user
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_UserRole_RoleId")
                        .OnDelete(DeleteBehavior.Cascade),
                    role => role
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserRole_UserId")
                        .OnDelete(DeleteBehavior.Cascade));

        }
    }
}
