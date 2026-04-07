using Application.Interfaces;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Infrastructure.Services;
public class NotificationService : INotificationService
{
    private readonly IHubContext<StockHub> _hubContext;

    public NotificationService(IHubContext<StockHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyStockUpdated(string reference, int nouveauStock, string raison)
    {
        await _hubContext.Clients.All.SendAsync("StockUpdated", new { reference, nouveauStock, raison });
    }
}
