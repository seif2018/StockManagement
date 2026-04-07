using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Infrastructure.SignalR;
public class StockHub : Hub
{
    public async Task NotifyStockUpdated(string reference, int nouveauStock, string raison)
    {
        await Clients.All.SendAsync("StockUpdated", new { reference, nouveauStock, raison });
    }
}
