using MediatR;
using Domain.Events;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace Infrastructure.DomainEventHandlers;
public class StockUpdatedEventHandler : INotificationHandler<StockUpdatedEvent>
{
    private readonly IMessageProducer _messageProducer;
    private readonly INotificationService _notificationService;
    private readonly ILogger<StockUpdatedEventHandler> _logger;

    public StockUpdatedEventHandler(IMessageProducer messageProducer, INotificationService notificationService, ILogger<StockUpdatedEventHandler> logger)
    {
        _messageProducer = messageProducer;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Handle(StockUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stock mis Ã  jour: {Reference} -> {NouveauStock}", notification.Reference, notification.NouveauStock);
        await _messageProducer.ProduceAsync("stock-events", notification);
        await _notificationService.NotifyStockUpdated(notification.Reference, notification.NouveauStock, notification.Raison);
    }
}
