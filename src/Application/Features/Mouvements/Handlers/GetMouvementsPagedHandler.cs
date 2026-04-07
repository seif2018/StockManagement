using Application.DTOs;
using Application.Features.Mouvements.Queries;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Mouvements.Handlers;

public class GetMouvementsPagedHandler : IRequestHandler<GetMouvementsPagedQuery, PaginatedResult<MouvementStock>>
{
    private readonly IApplicationDbContext _context;

    public GetMouvementsPagedHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<MouvementStock>> Handle(GetMouvementsPagedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Mouvements.AsQueryable();

        // Filtre (recherche textuelle)
        // Filtre textuel
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {

            var term = $"%{request.SearchTerm}%";
            query = query.Where(m =>
                EF.Functions.Like(m.ReferenceArticle, term) ||
                (m.Commentaire != null && EF.Functions.Like(m.Commentaire, term))
            );
        }

        // Tri dynamique sans package externe (switch)
        query = request.SortBy switch
        {
            "ReferenceArticle" => request.Descending
                ? query.OrderByDescending(m => m.ReferenceArticle)
                : query.OrderBy(m => m.ReferenceArticle),
            "Type" => request.Descending
                ? query.OrderByDescending(m => m.Type)
                : query.OrderBy(m => m.Type),
            "Quantite" => request.Descending
                ? query.OrderByDescending(m => m.Quantite)
                : query.OrderBy(m => m.Quantite),
            _ => request.Descending
                ? query.OrderByDescending(m => m.Date)
                : query.OrderBy(m => m.Date)
        };

        // Pagination
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<MouvementStock>(items, totalCount, request.PageNumber, request.PageSize);
    }
}