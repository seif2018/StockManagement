using MediatR;
using Application.DTOs;
using System;

namespace Application.Features.Articles.Commands;

public record CreateArticleCommand : IRequest<ArticleDto>
{
    public string Reference { get; init; } = "";
    public string Nom { get; init; } = "";
    public decimal PrixHT { get; init; }
    public string TypeArticle { get; init; } = "";
    public DateTime? DLC { get; init; }
    public string? TypeVente { get; init; }
    public string? Packaging { get; init; }
}
