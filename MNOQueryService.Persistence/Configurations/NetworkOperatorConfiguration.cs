using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNOQueryService.Domain.Entities;

namespace Application.Persistence.Configurations
{
    public class NetworkOperatorConfiguration : IEntityTypeConfiguration<NetworkOperator>
    {
       
        public NetworkOperatorConfiguration()
        {
        }

        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<NetworkOperator> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OperatorCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Operator)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(o => o.Country)
            .WithMany(c => c.Operators)
            .HasForeignKey(o => o.CountryId)
            .IsRequired();
        }
    }
}
