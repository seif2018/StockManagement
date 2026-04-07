using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Mouvements.Queries;

public record GetMouvementsQuery : IRequest<IEnumerable<MouvementStock>>;