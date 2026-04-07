using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class ArticleNonAlimentaire : Article
{
    private ArticleNonAlimentaire() { } // pour EF Core
    public NiveauPackaging Packaging { get; private set; }

    public ArticleNonAlimentaire(ReferenceEAN13 reference, string nom, decimal prixHT, NiveauPackaging packaging)
        : base(reference, nom, prixHT)
    {
        Packaging = packaging;
        MettreAJourPrixTTC();
    }
    public void UpdateNonAlimentaire(string nom, decimal prixHT, NiveauPackaging packaging)
    {
        UpdateDetails(nom, prixHT);
        Packaging = packaging;
        MettreAJourPrixTTC();
    }

    public override decimal CalculerTVA() => 0.20m;

    public override void MettreAJourPrixTTC()
    {
        PrixTTC = new Prix(PrixHT.Valeur * (1 + CalculerTVA()));
    }
}
