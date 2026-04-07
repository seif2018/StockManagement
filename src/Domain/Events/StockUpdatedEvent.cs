namespace Domain.Events;
public class StockUpdatedEvent : IDomainEvent
{
    public string Reference { get; }
    public int NouveauStock { get; }
    public string Raison { get; }

    public StockUpdatedEvent(string reference, int nouveauStock, string raison)
    {
        Reference = reference;
        NouveauStock = nouveauStock;
        Raison = raison;
    }
}
