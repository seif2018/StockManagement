using Application.Features.Articles.Commands;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Articles.Handlers;

public class InventaireHandler : IRequestHandler<InventaireCommand>
{
    private readonly IApplicationDbContext _context;

    public InventaireHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(InventaireCommand request, CancellationToken cancellationToken)
    {
        if (request.NouvelleQuantite < 0)
            throw new ArgumentException("La quantité après inventaire ne peut pas être négative.");
        var article = await _context.Articles
        .FirstOrDefaultAsync(a => a.Reference.Value == request.Reference, cancellationToken) ?? throw new KeyNotFoundException();
        var ancienneQuantite = article.QuantiteStock;
        article.AjusterStockParInventaire(request.NouvelleQuantite);

        // Créer et ajouter l'entrée dans la table Inventaires
        var inventaire = new Inventaire(request.Reference, ancienneQuantite, request.NouvelleQuantite);
        await _context.Inventaires.AddAsync(inventaire, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
