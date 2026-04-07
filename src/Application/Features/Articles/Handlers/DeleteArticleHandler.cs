using Application.Features.Articles.Commands;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Articles.Handlers;

public class DeleteArticleHandler : IRequestHandler<DeleteArticleCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteArticleHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _context.Articles
            .FirstOrDefaultAsync(a => a.Reference.Value == request.Reference, cancellationToken);

        if (article == null)
            return;

        // Supprimer d'abord les mouvements et inventaires associés (cascade manuelle)
        var mouvements = _context.Mouvements.Where(m => m.ReferenceArticle == request.Reference);
        var inventaires = _context.Inventaires.Where(i => i.ReferenceArticle == request.Reference);

        _context.Mouvements.RemoveRange(mouvements);
        _context.Inventaires.RemoveRange(inventaires);

        // Enfin supprimer l'article
        _context.Articles.Remove(article);

        await _context.SaveChangesAsync(cancellationToken);
    }
}