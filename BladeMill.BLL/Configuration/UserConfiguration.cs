using BladeMill.BLL.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BladeMill.BLL.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserDto>
    {
        public void Configure(EntityTypeBuilder<UserDto> builder)
        {
            //builder.ToTable("Movie", "Movies");
            builder.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(50).IsRequired();
            builder.HasIndex(e => e.Sso).IsUnique();
            builder.Property(e => e.Sso).HasMaxLength(9).IsFixedLength();

            //gdzie to dodac aby z migrowalo?
            //builder.ToSqlQuery<UserDto>($"ALTER TABLE Uzytkownicy ADD CONSTRAINT check_firstname_isnotempty CHECK (FirstName NOT IN (''))");
            //builder.ToSqlQuery<UserDto>($"ALTER TABLE Uzytkownicy ADD CONSTRAINT check_lastname_isnotempty CHECK (LastName NOT IN (''))");
            //builder.ToSqlQuery<UserDto>($"ALTER TABLE Uzytkownicy ADD CONSTRAINT check_lenght_Sso CHECK (len(Sso) = 9)");

            //builder.Property(e => e.YearOfProduction).HasColumnName("Year");
            //builder.Property(e => e.Tags).HasConversion(new TagsConverter());
            //builder.HasOne(e => e.Genre).WithMany().HasForeignKey("GenreKey").OnDelete(DeleteBehavior.SetNull);
            //builder.HasOne(e => e.Director).WithMany(e => e.AsDirector).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
