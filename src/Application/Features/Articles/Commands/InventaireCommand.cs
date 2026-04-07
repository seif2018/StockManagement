using MediatR;
namespace Application.Features.Articles.Commands;

public record InventaireCommand(string Reference, int NouvelleQuantite) : IRequest;
