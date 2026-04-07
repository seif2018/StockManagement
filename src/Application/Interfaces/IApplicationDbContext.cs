using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Article> Articles { get; }
    DbSet<MouvementStock> Mouvements { get; }
    DbSet<Inventaire> Inventaires { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}