using Domain.Enums;

namespace Domain.Entities;

public class MouvementStock
{
    public int Id { get; private set; }
    public string ReferenceArticle { get; private set; }
    public TypeMouvement Type { get; private set; }
    public int Quantite { get; private set; }
    public DateTime Date { get; private set; }
    public string? Commentaire { get; private set; }

    public MouvementStock(string referenceArticle, TypeMouvement type, int quantite, string? commentaire = null)
    {
        ReferenceArticle = referenceArticle;
        Type = type;
        Quantite = quantite;
        Date = DateTime.UtcNow;
        Commentaire = commentaire;
    }
}