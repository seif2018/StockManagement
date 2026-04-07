using Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Articles.Queries;

public record GetArticlesWithPaginationQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<ArticleDto>>;

//public record PaginatedResult<T>(IEnumerable<T> Items, int TotalCount, int PageNumber, int PageSize);