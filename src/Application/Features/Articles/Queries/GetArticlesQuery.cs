using MediatR;
using Application.DTOs;
using System.Collections.Generic;
namespace Application.Features.Articles.Queries;

public record GetArticlesQuery : IRequest<IEnumerable<ArticleDto>>;
