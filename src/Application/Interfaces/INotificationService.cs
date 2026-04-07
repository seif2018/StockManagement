using System.Threading.Tasks;

namespace Application.Interfaces;
public interface INotificationService
{
    Task NotifyStockUpdated(string reference, int nouveauStock, string raison);
}
