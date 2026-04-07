using Application.DTOs;
using Application.Features.Articles.Commands;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Articles.Handlers;

public class UpdateArticleHandler : IRequestHandler<UpdateArticleCommand, ArticleDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateArticleHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ArticleDto> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        if (request.PrixHT < 0)
            throw new ArgumentException("Le prix HT ne peut pas être négative.");
        var article = await _context.Articles
            .FirstOrDefaultAsync(a => a.Reference.Value == request.Reference, cancellationToken)
            ?? throw new KeyNotFoundException();

        if (article is ArticleAlimentaire aa)
        {
            if (!request.DLC.HasValue) throw new ArgumentException("DLC requise");
            var typeVente = Enum.Parse<TypeVenteAlimentaire>(request.TypeVente ?? "LesDeux");
            aa.UpdateAlimentaire(request.Nom, request.PrixHT, request.DLC.Value, typeVente);
        }
        else if (article is ArticleNonAlimentaire ana)
        {
            var packaging = Enum.Parse<NiveauPackaging>(request.Packaging ?? "Neuf");
            ana.UpdateNonAlimentaire(request.Nom, request.PrixHT, packaging);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ArticleDto>(article);
    }
}