using Application.DTOs;
using Application.Features.Inventaires.Queries;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Inventaires.Handlers;

public class GetInventairesExportHandler : IRequestHandler<GetInventairesExportQuery, List<InventaireExportDto>>
{
    private readonly IApplicationDbContext _context;

    public GetInventairesExportHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<InventaireExportDto>> Handle(GetInventairesExportQuery request, CancellationToken cancellationToken)
    {
        // Récupérer tous les inventaires
        var inventaires = await _context.Inventaires.ToListAsync(cancellationToken);
        var result = new List<InventaireExportDto>();

        foreach (var inv in inventaires)
        {
            // Récupérer l'article correspondant
            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.Reference.Value == inv.ReferenceArticle, cancellationToken);

            if (article != null)
            {
                result.Add(new InventaireExportDto
                {
                    ReferenceArticle = inv.ReferenceArticle,
                    NomArticle = article.Nom,
                    PrixHT = article.PrixHT.Valeur,
                    PrixTTC = article.PrixTTC.Valeur,
                    TypeArticle = article is ArticleAlimentaire ? "Alimentaire" : "NonAlimentaire",
                    AncienneQuantite = inv.AncienneQuantite,
                    NouvelleQuantite = inv.NouvelleQuantite,
                    Ecart = inv.NouvelleQuantite - inv.AncienneQuantite,
                    Date = inv.Date
                });
            }
        }

        return result.OrderByDescending(x => x.Date).ToList();
    }
}