using Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Inventaires.Queries;

public record GetInventairesExportQuery : IRequest<List<InventaireExportDto>>;