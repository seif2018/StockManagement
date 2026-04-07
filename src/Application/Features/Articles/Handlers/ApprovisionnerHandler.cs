using Application.Features.Articles.Commands;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Articles.Handlers;

public class ApprovisionnerHandler : IRequestHandler<ApprovisionnerCommand>
{
    private readonly IApplicationDbContext _context;

    public ApprovisionnerHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ApprovisionnerCommand request, CancellationToken cancellationToken)
    {
        if (request.Quantite < 0)
            throw new ArgumentException("La quantité d'approvisionnement doit être positive.");

        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Reference.Value == request.Reference, cancellationToken);
        if (article == null) throw new KeyNotFoundException($"Article {request.Reference} non trouvé");
        article.Approvisionner(request.Quantite);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
