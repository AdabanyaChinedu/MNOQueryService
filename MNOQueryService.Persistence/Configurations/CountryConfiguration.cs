using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNOQueryService.Domain.Entities;

namespace Application.Persistence.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
       
        public CountryConfiguration()
        {
        }

        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(c => c.CountryCode).IsUnique();

            builder.Property(x => x.CountryIso)
                .IsRequired()
                .HasMaxLength(2);

            builder.Property(x => x.CountryCode)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
