using Domain.ValueObjects;
using Domain.Events;
using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Entities;

public abstract class Article
{
    protected Article() { } // pour EF Core
    public int Id { get; private set; }  // Clé primaire technique
    public ReferenceEAN13 Reference { get; private set; }
    public string Nom { get; private set; }
    public Prix PrixHT { get; private set; }
    public Prix PrixTTC { get; protected set; }
    public int QuantiteStock { get; private set; }
    public DateTime? DernierInventaire { get; private set; }
    private readonly List<MouvementStock> _mouvements = new();
    public IReadOnlyCollection<MouvementStock> Mouvements => _mouvements.AsReadOnly();

    protected Article(ReferenceEAN13 reference, string nom, decimal prixHT)
    {
        Reference = reference;
        Nom = nom;
        PrixHT = new Prix(prixHT);
        PrixTTC = new Prix(0); // initialisation temporaire (sous-classes recalculent)
        QuantiteStock = 0;
    }

    public void UpdateDetails(string nom, decimal prixHT)
    {
        Nom = nom;
        PrixHT = new Prix(prixHT);
        MettreAJourPrixTTC();
    }
    public void Approvisionner(int quantite, string? commentaire = null)
    {
        if (quantite <= 0) throw new ArgumentException("La quantitÃ© doit Ãªtre positive");
        QuantiteStock += quantite;
        var mouvement = new MouvementStock(Reference.Value, TypeMouvement.Approvisionnement, quantite, commentaire);
        _mouvements.Add(mouvement);
        AddDomainEvent(new StockUpdatedEvent(Reference.Value, QuantiteStock, "Approvisionnement"));
    }

    public void AjusterStockParInventaire(int nouvelleQuantite)
    {
        var ancienne = QuantiteStock;
        QuantiteStock = nouvelleQuantite;
        DernierInventaire = DateTime.UtcNow;
        var mouvement = new MouvementStock(Reference.Value, TypeMouvement.Inventaire, nouvelleQuantite - ancienne, "Inventaire");
        _mouvements.Add(mouvement);
        AddDomainEvent(new StockUpdatedEvent(Reference.Value, QuantiteStock, "Inventaire"));
    }

    public abstract decimal CalculerTVA();
    public abstract void MettreAJourPrixTTC();

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
