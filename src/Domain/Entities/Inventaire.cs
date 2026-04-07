namespace Domain.Entities;

public class Inventaire
{
    public int Id { get; private set; }
    public string ReferenceArticle { get; private set; }
    public int AncienneQuantite { get; private set; }
    public int NouvelleQuantite { get; private set; }
    public DateTime Date { get; private set; }

    public Inventaire(string referenceArticle, int ancienneQuantite, int nouvelleQuantite)
    {
        ReferenceArticle = referenceArticle;
        AncienneQuantite = ancienneQuantite;
        NouvelleQuantite = nouvelleQuantite;
        Date = DateTime.UtcNow;
    }
}