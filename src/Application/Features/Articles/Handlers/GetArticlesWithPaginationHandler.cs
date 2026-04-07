using Application.DTOs;
using Application.Features.Articles.Queries;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Articles.Handlers;

public class GetArticlesWithPaginationHandler : IRequestHandler<GetArticlesWithPaginationQuery, PaginatedResult<ArticleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetArticlesWithPaginationHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ArticleDto>> Handle(GetArticlesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Articles.AsQueryable();
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        var dtos = _mapper.Map<IEnumerable<ArticleDto>>(items);
        return new PaginatedResult<ArticleDto>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}