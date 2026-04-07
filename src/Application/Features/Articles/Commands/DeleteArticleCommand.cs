using MediatR;

namespace Application.Features.Articles.Commands;

public record DeleteArticleCommand(string Reference) : IRequest;