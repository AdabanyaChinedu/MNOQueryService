using Application.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using MNOQueryService.Domain.Entities;
using MNOQueryService.Domain.Interfaces;

namespace MNOQueryService.Persistence.DatabaseContext
{
    public class MNODbContext : DbContext, IMNODbContext
    {
        public MNODbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Country> Countries => this.Set<Country>();
        public DbSet<NetworkOperator> Operators => this.Set<NetworkOperator>();

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new NetworkOperatorConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
