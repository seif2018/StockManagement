using MediatR;
using Application.DTOs;
using Domain.Entities;

namespace Application.Features.Mouvements.Queries;

public record GetMouvementsPagedQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string SortBy = "Date",
    bool Descending = true,
    string? SearchTerm = null
) : IRequest<PaginatedResult<MouvementStock>>;