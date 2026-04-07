using MediatR;
namespace Application.Features.Articles.Commands;

public record ApprovisionnerCommand(string Reference, int Quantite) : IRequest;
