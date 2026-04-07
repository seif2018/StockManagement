using Application.DTOs;
using MediatR;
using System;

namespace Application.Features.Articles.Commands;

public record UpdateArticleCommand : IRequest<ArticleDto>
{
    public string Reference { get; init; } = "";
    public string Nom { get; init; } = "";
    public decimal PrixHT { get; init; }
    public DateTime? DLC { get; init; }
    public string? TypeVente { get; init; }
    public string? Packaging { get; init; }
}