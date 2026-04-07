using MediatR;
using Application.Features.Articles.Queries;
using Application.Interfaces;
using Application.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace Application.Features.Articles.Handlers;

public class GetArticlesHandler : IRequestHandler<GetArticlesQuery, IEnumerable<ArticleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetArticlesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ArticleDto>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
    {
        var articles = await _context.Articles.ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ArticleDto>>(articles);
    }
}
