using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MNOQueryService.Domain.Entities;
using System.Collections.Generic;

namespace MNOQueryService.Domain.Interfaces
{
    public interface IMNODbContext
    {
        DbSet<Country> Countries { get; }

        DbSet<NetworkOperator> Operators { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets database object.
        /// </summary>
        public DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int SaveChanges();
    }
}
