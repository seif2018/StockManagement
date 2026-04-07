using Application.Features.Mouvements.Queries;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Mouvements.Handlers;

public class GetMouvementsHandler : IRequestHandler<GetMouvementsQuery, IEnumerable<MouvementStock>>
{
    private readonly IApplicationDbContext _context;

    public GetMouvementsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MouvementStock>> Handle(GetMouvementsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Mouvements
            .OrderByDescending(m => m.Date)
            .ToListAsync(cancellationToken);
    }
}