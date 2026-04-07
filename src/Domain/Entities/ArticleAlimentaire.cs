using Domain.Enums;
using Domain.ValueObjects;
using System;

namespace Domain.Entities;

public class ArticleAlimentaire : Article
{
    private ArticleAlimentaire() { } // pour EF Core
    public DateTime DLC { get; private set; }
    public TypeVenteAlimentaire TypeVente { get; private set; }

    public ArticleAlimentaire(ReferenceEAN13 reference, string nom, decimal prixHT, DateTime dlc, TypeVenteAlimentaire typeVente)
        : base(reference, nom, prixHT)
    {
        DLC = dlc;
        TypeVente = typeVente;
        MettreAJourPrixTTC();
    }
    public void UpdateAlimentaire(string nom, decimal prixHT, DateTime dlc, TypeVenteAlimentaire typeVente)
    {
        UpdateDetails(nom, prixHT);
        DLC = dlc;
        TypeVente = typeVente;
        MettreAJourPrixTTC();
    }
    public override decimal CalculerTVA() => TypeVente == TypeVenteAlimentaire.Emporter ? 0.055m : 0.10m;

    public override void MettreAJourPrixTTC()
    {
        var tva = CalculerTVA();
        PrixTTC = new Prix(PrixHT.Valeur * (1 + tva));
    }
}
