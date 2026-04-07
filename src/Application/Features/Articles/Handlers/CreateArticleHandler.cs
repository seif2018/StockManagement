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
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Articles.Handlers;

public class CreateArticleHandler : IRequestHandler<CreateArticleCommand, ArticleDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateArticleHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ArticleDto> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        if (request.PrixHT < 0)
            throw new ArgumentException("Le prix HT ne peut pas être négative.");

        var reference = new ReferenceEAN13(request.Reference);
        Article article;

        // Vérifier si la référence existe déjà
        var exists = await _context.Articles.AnyAsync(a => a.Reference.Value == request.Reference, cancellationToken);
        if (exists)
            throw new InvalidOperationException($"Un article avec la référence {request.Reference} existe déjà.");
        if (request.TypeArticle == "Alimentaire")
        {
            if (!request.DLC.HasValue) throw new ArgumentException("DLC requise");
            var typeVente = Enum.Parse<TypeVenteAlimentaire>(request.TypeVente ?? "LesDeux");
            article = new ArticleAlimentaire(reference, request.Nom, request.PrixHT, request.DLC.Value, typeVente);
        }
        else
        {
            var packaging = Enum.Parse<NiveauPackaging>(request.Packaging ?? "Neuf");
            article = new ArticleNonAlimentaire(reference, request.Nom, request.PrixHT, packaging);
        }
        await _context.Articles.AddAsync(article, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ArticleDto>(article);
    }
}
