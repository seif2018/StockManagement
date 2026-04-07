using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;

    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Article> Articles => Set<Article>();
    public DbSet<MouvementStock> Mouvements => Set<MouvementStock>();
    public DbSet<Inventaire> Inventaires => Set<Inventaire>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuration de l'entité Article
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(a => a.Id);

            // Configuration de Reference (owned type) avec index unique
            entity.OwnsOne(a => a.Reference, reference =>
            {
                reference.Property(r => r.Value)
                    .HasColumnName("Reference")
                    .HasMaxLength(13)
                    .IsRequired();
                reference.HasIndex(r => r.Value).IsUnique(); // Index unique
            });

            // Configuration de PrixHT
            entity.OwnsOne(a => a.PrixHT, prix =>
            {
                prix.Property(p => p.Valeur)
                    .HasColumnName("PrixHT")
                    .HasColumnType("decimal(18,2)");
            });

            // Configuration de PrixTTC
            entity.OwnsOne(a => a.PrixTTC, prix =>
            {
                prix.Property(p => p.Valeur)
                    .HasColumnName("PrixTTC")
                    .HasColumnType("decimal(18,2)");
            });

            // Discriminator pour l'héritage
            entity.HasDiscriminator<string>("TypeArticle")
                .HasValue<ArticleAlimentaire>("Alimentaire")
                .HasValue<ArticleNonAlimentaire>("NonAlimentaire");
        });

        // Configuration de MouvementStock (sans relation)
        modelBuilder.Entity<MouvementStock>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.ReferenceArticle).HasMaxLength(13).IsRequired();
            entity.Property(m => m.Type).HasConversion<string>();
        });

        // Configuration de Inventaire (sans relation)
        modelBuilder.Entity<Inventaire>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ReferenceArticle).HasMaxLength(13).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Article>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var entity in ChangeTracker.Entries<Article>())
        {
            entity.Entity.ClearDomainEvents();
        }

        return result;
    }
}